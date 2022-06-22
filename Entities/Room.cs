using System.ComponentModel.DataAnnotations.Schema;

namespace g2hotel_server.Entities
{
    [Table("Rooms")]
    public class Room : AbstractEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;

        [Column(TypeName = "varchar(10)")]
        public string Code { get; set; } = null!;
        public string? ShortDescription { get; set; }
        public string? Description { get; set; }
        public decimal DefaultPrice { get; set; }
        public decimal PromotionPrice { get; set; }
        public int NumBeds { get; set; }
        public int NumAdults { get; set; }
        public int NumChilds { get; set; }
        public int? RoomTypeId { get; set; }
        public RoomType RoomType { get; set; } = null!;
        public ICollection<Photo>? Photos { get; set; }
        public IList<DetailRoomPayment>? DetailRoomPayments { get; set; }

    }
}