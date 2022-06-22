using System.ComponentModel.DataAnnotations.Schema;

namespace g2hotel_server.Entities
{
    [Table("Photos")]
    public class Photo : AbstractEntity
    {
        public int Id { get; set; }
        public string? Url { get; set; }
        public bool IsMain { get; set; }
        public string? PublicId { get; set; }
        public Room? Room { get; set; }
        public int RoomId { get; set; }
    }
}