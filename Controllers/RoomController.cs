using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using g2hotel_server.DTOs;
using g2hotel_server.Entities;
using g2hotel_server.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace g2hotel_server.Controllers
{
    public class RoomController : BaseApiController
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IMapper _mapper;
        private readonly IPhotoService _photoService;
        private readonly IUnitOfWork _unitOfWork;
        public RoomController(IMapper mapper, IUnitOfWork unitOfWork, IPhotoService photoService, IMemoryCache memoryCache)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _photoService = photoService;
            _memoryCache = memoryCache;
        }

        [HttpPost("add-room")]
        public async Task<ActionResult<RoomDTO>> AddRoom(RoomDTO roomDto)
        {
            if (_unitOfWork.RoomRepository.RoomCodeExists(roomDto.Code).Result)
            {
                return BadRequest("Room code already exists");
            }

            var room = _mapper.Map<Room>(roomDto);
            var roomAddedEntity = _unitOfWork.RoomRepository.AddRoom(room);
            if (await _unitOfWork.Complete())
            {
                var cacheKey = "room_added";
                //checks if cache entries exists
                if (!_memoryCache.TryGetValue(cacheKey, out Room roomAdded))
                {
                    //if not, add it
                    {
                        //calling the server
                        roomAdded = roomAddedEntity;

                        //setting up cache options
                        var cacheExpiryOptions = new MemoryCacheEntryOptions
                        {
                            AbsoluteExpiration = DateTime.Now.AddSeconds(600),
                            Priority = CacheItemPriority.High,
                            SlidingExpiration = TimeSpan.FromSeconds(20)
                        };
                        //setting cache entries
                        _memoryCache.Set(cacheKey, roomAdded, cacheExpiryOptions);
                    }
                }
                return Ok();
            }

            return BadRequest("Problem adding room");
        }

        [HttpGet("get-rooms")]
        public async Task<ActionResult<IEnumerable<RoomDTO>>> GetRooms()
        {
            var roomsEntity = await _unitOfWork.RoomRepository.GetRoomsAsync();
            var rooms = _mapper.Map<IEnumerable<RoomDTO>>(roomsEntity);
            return Ok(rooms);
        }

        //Get room by id
        [HttpGet("{id}")]
        public async Task<ActionResult<RoomDTO>> GetRoomById(int id)
        {
            var roomEntity = await _unitOfWork.RoomRepository.GetRoomByIdAsync(id);
            var room = _mapper.Map<RoomDTO>(roomEntity);
            return Ok(room);
        }

        //Delete room by id
        [HttpDelete("delete-room/{id}")]
        public async Task<ActionResult<RoomDTO>> DeleteRoom(int id)
        {
            var roomEntity = await _unitOfWork.RoomRepository.GetRoomByIdAsync(id);
            if (roomEntity == null)
            {
                return NotFound();
            }
            _unitOfWork.RoomRepository.Delete(roomEntity);
            if (await _unitOfWork.Complete())
            {
                return Ok();
            }
            return BadRequest("Problem deleting room");
        }

        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var roomAdded = _memoryCache.Get<Room>("room_added");
            if (roomAdded == null)
            {
                return BadRequest("Room not added");
            }
            var room = _unitOfWork.RoomRepository.GetRoomByCodeAsync(roomAdded.Code).Result;
            var result = await _photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (room.Photos == null)
            {
                photo.IsMain = true;
            }
            room.Photos = new List<Photo>();
            room.Photos.Add(photo);

            if (await _unitOfWork.Complete())
            {
                //remove cache entry
                _memoryCache.Remove("room_added");
                return Ok();
            }
            return BadRequest("Problem addding photo");
        }

        [HttpPost("add-multi-photo")]
        public async Task<ActionResult<PhotoDTO>> AddMultiPhoto(IList<IFormFile> files)
        {
            var roomAdded = _memoryCache.Get<Room>("room_added");
            if (roomAdded == null)
            {
                return BadRequest("Room not added");
            }
            var room = _unitOfWork.RoomRepository.GetRoomByCodeAsync(roomAdded.Code).Result;
            // var room = _unitOfWork.RoomRepository.GetRoomByCodeAsync("ROOM-VIP").Result;
            room.Photos = new List<Photo>();
            List<Photo> photos = new List<Photo>();
            foreach (var file in files)
            {
                var result = await _photoService.AddPhotoAsync(file);

                if (result.Error != null) return BadRequest(result.Error.Message);

                var photo = new Photo
                {
                    Url = result.SecureUrl.AbsoluteUri,
                    PublicId = result.PublicId
                };

                if (room.Photos == null)
                {
                    photo.IsMain = true;
                }
                photos.Add(photo);
            }
            room.Photos = photos;

            if (await _unitOfWork.Complete())
            {
                //remove cache entry
                _memoryCache.Remove("room_added");
                return Ok();
            }
            return BadRequest("Problem addding photo");
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(RoomUpdateDTO roomDTO)
        {

            var room = await _unitOfWork.RoomRepository.GetRoomByIdAsync(roomDTO.Id);

            _mapper.Map(roomDTO, room);

            _unitOfWork.RoomRepository.Update(room);

            if (await _unitOfWork.Complete()) return NoContent();

            return BadRequest("Failed to update room");
        }

        [HttpDelete("delete-photo")]
        public async Task<ActionResult> DeletePhoto(DeletePhotoDTO deletePhotoDTO)
        {
            var room = await _unitOfWork.RoomRepository.GetRoomByIdAsync(deletePhotoDTO.roomId);

            if (room.Photos == null)
            {
                return BadRequest("No photos to delete");
            }
            var photo = room.Photos.FirstOrDefault(x => x.Id == deletePhotoDTO.photoId);

            if (photo == null) return NotFound();

            if (photo.IsMain) return BadRequest("You cannot delete your main photo");

            if (photo.PublicId != null)
            {
                var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                if (result.Error != null) return BadRequest(result.Error.Message);
            }

            room.Photos.Remove(photo);

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to delete the photo");
        }

        //delete multiple photos
        [HttpDelete("delete-multi-photo")]
        public async Task<ActionResult> DeleteMultiPhoto(DeleteMultiPhotoDTO deleteMultiPhotoDTO)
        {
            var room = await _unitOfWork.RoomRepository.GetRoomByIdAsync(deleteMultiPhotoDTO.roomId);
            if (room.Photos == null)
            {
                return BadRequest("No photos to delete");
            }

            foreach (var p in deleteMultiPhotoDTO.photos)
            {
                var photo = room.Photos.FirstOrDefault(x => x.Id == p.Id);
                if (photo == null) return NotFound();
                if (photo.IsMain) return BadRequest("You cannot delete your main photo");
                if (photo.PublicId != null)
                {
                    var result = await _photoService.DeletePhotoAsync(photo.PublicId);
                    if (result.Error != null) return BadRequest(result.Error.Message);
                }
                room.Photos.Remove(photo);
            }

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Failed to delete the photo");
        }

        [HttpPost("add-multi-photo/{roomId}")]
        public async Task<ActionResult<PhotoDTO>> AddMultiPhotoWithRoomId(IList<IFormFile> files, int roomId)
        {
            var room = _unitOfWork.RoomRepository.GetRoomByIdAsync(roomId).Result;

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

                if (room.Photos == null)
                {
                    photo.IsMain = true;
                }
                // photos.Add(photo);
                room.Photos?.Add(photo);
            }
            // room.Photos = photos;

            if (await _unitOfWork.Complete()) return Ok();

            return BadRequest("Problem addding photo");
        }


    }
}