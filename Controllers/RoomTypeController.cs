using AutoMapper;
using g2hotel_server.DTOs;
using g2hotel_server.Entities;
using g2hotel_server.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace g2hotel_server.Controllers
{
    public class RoomTypeController : BaseApiController
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _unitOfWork;

        public RoomTypeController(IMapper mapper, IUnitOfWork unitOfWork, IMemoryCache memoryCache, IPhotoService photoService)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _memoryCache = memoryCache;
            _photoService = photoService;
        }

        [HttpGet("get-roomTypes")]
        public async Task<ActionResult<IEnumerable<RoomTypeDTO>>> GetRoomTypes()
        {
            var roomTypesEntity = await _unitOfWork.RoomTypeRepository.GetRoomTypesAsync();
            var roomTypes = _mapper.Map<IEnumerable<RoomTypeDTO>>(roomTypesEntity);
            return Ok(roomTypes);
        }

        [HttpGet("{checkIn}/{checkOut}")]
        public async Task<ActionResult<IEnumerable<RoomTypeDTO>>> GetRoomTypesWithDateFiler(DateTime checkIn, DateTime checkOut)
        {
            var roomTypesEntity = await _unitOfWork.RoomTypeRepository.GetRoomTypesAsync();
            var rooms = await _unitOfWork.RoomRepository.GetRoomsByCheckInDateAsync(checkIn, checkOut);
            foreach (var roomType in roomTypesEntity)
            {
                roomType.Rooms = new List<Room>();
                foreach (var room in rooms)
                {
                    if (room.RoomTypeId == roomType.Id)
                    {
                        roomType.Rooms.Add(room);
                    }
                }
            }
            var roomTypes = _mapper.Map<IEnumerable<RoomTypeDTO>>(roomTypesEntity);
            // var rooms = await _unitOfWork.RoomRepository.GetRoomsByCheckInDateAsync(dateFilterDTO.checkIn, dateFilterDTO.checkOut);
            // var roomDTOs = _mapper.Map<IEnumerable<RoomDTO>>(rooms);
            return Ok(roomTypes);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RoomTypeDTO>> GetRoomTypeById(int id)
        {
            var roomTypeEntity = await _unitOfWork.RoomTypeRepository.GetRoomTypeById(id);
            var roomType = _mapper.Map<RoomTypeDTO>(roomTypeEntity);
            return Ok(roomType);
        }

        [HttpPost]
        [Authorize(Policy = "RequireMemberRole")]
        public async Task<ActionResult<RoomTypeDTO>> AddRoomType(RoomTypeDTO roomTypeDTO)
        {
            var roomTypesEntity = _mapper.Map<RoomType>(roomTypeDTO);
            var result = _unitOfWork.RoomTypeRepository.AddRoomType(roomTypesEntity);
            if (await _unitOfWork.Complete())
            {
                var cacheKey = "room_type_added";
                //checks if cache entries exists
                if (!_memoryCache.TryGetValue(cacheKey, out RoomType roomTypeAdded))
                {
                    //if not, add it
                    {
                        //calling the server
                        roomTypeAdded = result;

                        //setting up cache options
                        var cacheExpiryOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpiration = DateTime.Now.AddSeconds(600),
                            Priority = CacheItemPriority.High,
                            SlidingExpiration = TimeSpan.FromSeconds(20)
                        };
                        //setting cache entries
                        _memoryCache.Set(cacheKey, roomTypeAdded, cacheExpiryOptions);
                    }
                }
                return Ok();
            };
            return BadRequest("Problem adding room type");
        }

        [HttpPost("add-photo")]
        [Authorize(Policy = "RequireMemberRole")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var roomTypeAdded = _memoryCache.Get<RoomType>("room_type_added");
            if (roomTypeAdded == null)
            {
                return BadRequest("Room typed not added");
            }
            var roomType = _unitOfWork.RoomTypeRepository.GetRoomTypeById(roomTypeAdded.Id).Result;
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (roomType.Photos == null)
            {
                photo.IsMain = true;
            }
            roomType.Photos = new List<Photo>();
            roomType.Photos.Add(photo);

            if (await _unitOfWork.Complete())
            {
                //remove cache entry
                _memoryCache.Remove("room_type_added");
                return Ok();
            }
            return BadRequest("Problem addding photo");
        }

        [HttpPost("add-multi-photo/{roomTypeId}")]
        [Authorize(Policy = "RequireMemberRole")]
        public async Task<ActionResult<PhotoDTO>> AddMultiPhotoWithRoomTypeId(IList<IFormFile> files, int roomTypeId)
        {
            var roomType = _unitOfWork.RoomTypeRepository.GetRoomTypeById(roomTypeId).Result;

            // List<Photo> photos = new List<Photo>();
            foreach (var file in files)
            {
                var result = await _photoService.AddPhotoAsync(file);

                if (result.Error != null) return BadRequest(result.Error.Message);

                var photo = new Photo
                {
                    Url = result.SecureUrl.AbsoluteUri,
                    PublicId = result.PublicId
                };

                if (roomType.Photos == null)
                {
                    photo.IsMain = true;
                }
                // photos.Add(photo);
                roomType.Photos?.Add(photo);
            }
            // room.Photos = photos;

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Problem addding photo");
        }

        [HttpDelete("delete-roomType/{id}")]
        [Authorize(Policy = "RequireMemberRole")]
        public async Task<ActionResult<RoomTypeDTO>> DeleteRoomType(int id)
        {
            var roomTypesEntity = await _unitOfWork.RoomTypeRepository.GetRoomTypeById(id);
            if (roomTypesEntity == null)
            {
                return NotFound();
            }
            _unitOfWork.RoomTypeRepository.Delete(roomTypesEntity);
            if (await _unitOfWork.Complete())
            {
                return Ok();
            }
            return BadRequest("Problem deleting service");
        }

        [HttpPut]
        [Authorize(Policy = "RequireMemberRole")]
        public async Task<ActionResult<RoomTypeDTO>> UpdateRoomType(RoomTypeDTO roomTypeDTO)
        {
            var roomType = await _unitOfWork.RoomTypeRepository.GetRoomTypeById(roomTypeDTO.Id);
            _mapper.Map(roomTypeDTO, roomType);
            _unitOfWork.RoomTypeRepository.Update(roomType);
            if (roomType == null)
                return NotFound($"Room type with Id = {roomTypeDTO.Id} not found");
            if (await _unitOfWork.Complete()) return NoContent();
            return BadRequest("Failed to update Room Type");
        }

        [HttpDelete("delete-photo")]
        [Authorize(Policy = "RequireMemberRole")]
        public async Task<ActionResult> DeletePhoto(DeletePhotoRoomTypeDTO deletePhotoRoomTypeDTO)
        {
            var roomType = await _unitOfWork.RoomTypeRepository.GetRoomTypeById(deletePhotoRoomTypeDTO.roomTypeId);

            if (roomType.Photos == null)
            {
                return BadRequest("No photos to delete");
            }
            var photo = roomType.Photos.FirstOrDefault(x => x.Id == deletePhotoRoomTypeDTO.photoId);

            if (photo == null) return NotFound();

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            roomType.Photos.Remove(photo);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to delete the photo");
        }
    }
}