using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using g2hotel_server.DTOs;
using g2hotel_server.Entities;
using g2hotel_server.Extensions;
using g2hotel_server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace g2hotel_server.Controllers
{
    public class VNPayController : BaseApiController
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMailService _mailService;
        public VNPayController(IUnitOfWork unitOfWork, IMapper mapper, IMemoryCache memoryCache, IMailService mailService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _memoryCache = memoryCache;
            _mailService = mailService;
        }

        //Pay with VNPay
        [HttpPost("vnpay")]
        public IActionResult VNPay(VNPayDTO vnPayInfo)
        {
            //Get Config Info
            string vnp_Returnurl = "https://g2hotel.vercel.app/checkout/vnpay_return"; //URL nhan ket qua tra ve 
            string vnp_Url = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html"; //URL thanh toan cua VNPAY 
            string vnp_TmnCode = "S5IL4BIM"; //Ma website
            string vnp_HashSecret = "BSIAQWBYSLMYWGHDATVYDXLBYQZJWZYL"; //Chuoi bi mat
            if (string.IsNullOrEmpty(vnp_TmnCode) || string.IsNullOrEmpty(vnp_HashSecret))
            {
                return BadRequest("Vui lòng cấu hình các tham số: vnp_TmnCode,vnp_HashSecret trong file web.config");
            }
            //Get payment input
            PaymentDTO order = new PaymentDTO();
            //Save order to db
            order.OrderId = DateTime.Now.Ticks; // Giả lập mã giao dịch hệ thống merchant gửi sang VNPAY
            order.TotalPayment = vnPayInfo.vnp_Bill_TotalPrice; // Giả lập số tiền thanh toán hệ thống merchant gửi sang VNPAY 100,000 VND
            order.PayStatus = "0"; //0: Trạng thái thanh toán "chờ thanh toán" hoặc "Pending"
            order.CreatedDate = DateTime.Now; // Giả lập thời gian tạo đơn hàng
            order.PaymentTypeId = 7; // Giả lập loại thanh toán "VNPay"

            //Build URL for VNPAY
            VnPayLibrary vnpay = new VnPayLibrary();

            vnpay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnpay.AddRequestData("vnp_Command", "pay");
            vnpay.AddRequestData("vnp_TmnCode", vnp_TmnCode);
            vnpay.AddRequestData("vnp_Amount", (Math.Round(order.TotalPayment) * 100).ToString()); //Số tiền thanh toán. Số tiền không mang các ký tự phân tách thập phân, phần nghìn, ký tự tiền tệ. Để gửi số tiền thanh toán là 100,000 VND (một trăm nghìn VNĐ) thì merchant cần nhân thêm 100 lần (khử phần thập phân), sau đó gửi sang VNPAY là: 10000000
            vnpay.AddRequestData("vnp_CreateDate", order.CreatedDate.ToString("yyyyMMddHHmmss"));
            vnpay.AddRequestData("vnp_CurrCode", "VND");
            vnpay.AddRequestData("vnp_Locale", "vn");
            vnpay.AddRequestData("vnp_OrderInfo", "Thanh toan don hang:" + order.OrderId);
            vnpay.AddRequestData("vnp_ReturnUrl", vnp_Returnurl);
            vnpay.AddRequestData("vnp_TxnRef", order.OrderId.ToString()); // Mã tham chiếu của giao dịch tại hệ thống của merchant. Mã này là duy nhất dùng để phân biệt các đơn hàng gửi sang VNPAY. Không được trùng lặp trong ngày

            //Add Params of 2.1.0 Version
            vnpay.AddRequestData("vnp_ExpireDate", vnPayInfo.vnp_ExpireDate.ToString("yyyyMMddHHmmss")); //Thời gian hết hạn thanh toán. YYYYMMDDHHmmss


            //Billing
            if (vnPayInfo.vnp_Bill_Mobile == null)
            {
                return BadRequest("Vui lòng nhập số điện thoại");
            }
            vnpay.AddRequestData("vnp_Bill_Mobile", vnPayInfo.vnp_Bill_Mobile.Trim());
            if (vnPayInfo.vnp_Bill_Email == null)
            {
                return BadRequest("Vui lòng nhập email");
            }
            vnpay.AddRequestData("vnp_Bill_Email", vnPayInfo.vnp_Bill_Email.Trim());

            if (vnPayInfo.vnp_Bill_FirstName == null)
            {
                return BadRequest("Vui lòng nhập tên của bạn");
            }
            vnpay.AddRequestData("vnp_Bill_FirstName", vnPayInfo.vnp_Bill_FirstName.Trim());

            if (vnPayInfo.vnp_Bill_LastName == null)
            {
                return BadRequest("Vui lòng nhập họ của bạn");
            }
            vnpay.AddRequestData("vnp_Bill_LastName", vnPayInfo.vnp_Bill_LastName.Trim());

            var cacheKey = "payment_cache_added";
            //checks if cache entries exists
            if (!_memoryCache.TryGetValue(cacheKey, out PaymentCacheDTO paymentCache))
            {
                //if not, add it
                {
                    //calling the server
                    paymentCache = new PaymentCacheDTO
                    {
                        paymentDTO = order,
                        vNPayDTO = vnPayInfo
                    };

                    //setting up cache options
                    var cacheExpiryOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpiration = DateTime.Now.AddSeconds(600),
                        Priority = CacheItemPriority.High,
                        SlidingExpiration = TimeSpan.FromMinutes(10)
                    };
                    //setting cache entries
                    _memoryCache.Set(cacheKey, paymentCache, cacheExpiryOptions);
                }
            }
            string paymentUrl = vnpay.CreateRequestUrl(vnp_Url, vnp_HashSecret);
            return Ok(paymentUrl);
        }

        //delete payment and detail room payment when order is cancel
        [HttpDelete("cancel")]
        public async Task<IActionResult> DeletePayment(CancelInvoiceDTO cancelInvoiceDTO)
        {
            if (cancelInvoiceDTO.StatusInvoice != "00")
            {
                //get detail payment
                var payment = await _unitOfWork.PaymentRepository.GetPaymentByOrderIdAsync(cancelInvoiceDTO.OrderId);
                if (payment == null)
                {
                    return NotFound("Không tìm thấy đơn hàng");
                }
                //get detail room payment
                var detailRoomPayments = await _unitOfWork.DetailRoomPaymentRepository.GetDetailRoomPaymentsByPaymentIdAsync(payment.Id);
                if (detailRoomPayments == null)
                {
                    return NotFound("Không tìm thấy chi tiết đơn hàng");
                }

                //loop detail room payment and update amount room
                foreach (var item in detailRoomPayments)
                {
                    var room = await _unitOfWork.RoomRepository.GetRoomByIdAsync(item.RoomId);
                    if (room == null)
                    {
                        return NotFound("Không tìm thấy phòng");
                    }
                    room.Amount = room.Amount + item.Amount;
                }

                //delete detail room payment
                _unitOfWork.DetailRoomPaymentRepository.RemoveDetailRoomPaymentsRange(detailRoomPayments);
            }

            if (await _unitOfWork.Complete())
            {
                return Ok();
            }
            return BadRequest("Có lỗi xảy ra khi xóa chi tiết đơn hàng");
        }

        //update payment when order is paid
        [HttpPut("paid")]
        public async Task<IActionResult> UpdatePayment(PaidInvoiceDTO paidInvoiceDTO)
        {
            if (paidInvoiceDTO.vnp_ResponseCode == "00")
            {
                var paymentCacheAdded = _memoryCache.Get<PaymentCacheDTO>("payment_cache_added");
                if (paymentCacheAdded.vNPayDTO == null)
                {
                    return NotFound("Không tìm thấy chi tiết đơn hàng");
                }

                var listRoomEntity = _mapper.Map<List<Room>>(paymentCacheAdded.vNPayDTO.vnp_Bill_RoomSelecteds);// lay ra danh sach phong da chon trong cach
                var listServiceSelected = paymentCacheAdded.vNPayDTO.vnp_Bill_ServiceSelecteds;// lay ra danh sach dich vu da chon trong cach   //new

                if (paymentCacheAdded.paymentDTO != null)
                {
                    paymentCacheAdded.paymentDTO.PayStatus = "1"; // 0: chưa thanh toán, 1: đã thanh toán
                }
                var orderEntity = _unitOfWork.PaymentRepository.AddPayment(_mapper.Map<Payment>(paymentCacheAdded.paymentDTO));// them payment vao db

                //add room to detail room payment
                if (orderEntity.DetailRoomPayments == null)
                {
                    orderEntity.DetailRoomPayments = new List<DetailRoomPayment>();
                }
                foreach (var room in listRoomEntity)
                {
                    var detailRoomPayment = new DetailRoomPayment
                    {
                        PaymentId = orderEntity.Id,
                        RoomId = room.Id,
                        CheckInDate = paymentCacheAdded.vNPayDTO.vnp_Bill_CheckIn.Date.AddHours(14),
                        CheckOutDate = paymentCacheAdded.vNPayDTO.vnp_Bill_CheckOut.Date.AddHours(12),
                        Amount = room.Amount
                    };
                    Room roomEntity = _unitOfWork.RoomRepository.GetRoomByIdAsync(room.Id).Result;
                    if (roomEntity == null)
                    {
                        return BadRequest("Không tìm thấy phòng");
                    }
                    roomEntity.Amount -= room.Amount;
                    orderEntity.DetailRoomPayments.Add(detailRoomPayment);
                }

                //add service to detail service payment
                if (orderEntity.DetailServicePayments == null)
                {
                    orderEntity.DetailServicePayments = new List<DetailServicePayment>();
                }
                if (listServiceSelected != null)
                {
                    foreach (var service in listServiceSelected)
                    {
                        var detailServicePayment = new DetailServicePayment
                        {
                            PaymentId = orderEntity.Id,
                            ServiceId = service.Id,
                            Amount = service.Amount
                        };
                        orderEntity.DetailServicePayments.Add(detailServicePayment);
                    }
                }

                Customer customer = new Customer();
                if (paymentCacheAdded.vNPayDTO.vnp_Bill_Mobile != null)
                {
                    customer.Phone = paymentCacheAdded.vNPayDTO.vnp_Bill_Mobile.Trim();
                }
                if (paymentCacheAdded.vNPayDTO.vnp_Bill_FirstName != null && paymentCacheAdded.vNPayDTO.vnp_Bill_LastName != null)
                {
                    customer.FullName = paymentCacheAdded.vNPayDTO.vnp_Bill_FirstName.Trim() + " " + paymentCacheAdded.vNPayDTO.vnp_Bill_LastName.Trim();
                }
                if (paymentCacheAdded.vNPayDTO.vnp_Bill_Email != null)
                {
                    customer.Email = paymentCacheAdded.vNPayDTO.vnp_Bill_Email.Trim();
                }
                if (customer.Payments == null)
                {
                    customer.Payments = new List<Payment>();
                }
                customer.Payments.Add(orderEntity);

                _unitOfWork.CustomerRepository.AddCustomer(customer);

                if (await _unitOfWork.Complete())
                {
                    //send mail
                    // Gửi email
                    MailDTO mailReceived = new MailDTO
                    {
                        To = customer.Email,
                        Subject = "Thanh toán thành công",
                        Html = $"Cảm ơn quý khách đã sử dụng dịch vụ đặt phòng của chúng tôi! <br/> Mã hóa đơn của quý khách là: <strong>{orderEntity.OrderId}</strong> <br/> Quý khách có thể tra cứu thông tin đơn hàng tại <a href='https://g2hotel.vercel.app/check-payment'>đây</a>"
                    };
                    _mailService.Send(mailReceived);

                    //remove cache
                    _memoryCache.Remove("payment_cache_added");
                    return Ok();
                }
            }
            return BadRequest("Có lỗi xảy ra khi cập nhật trạng thái đơn hàng");
        }
    }
}