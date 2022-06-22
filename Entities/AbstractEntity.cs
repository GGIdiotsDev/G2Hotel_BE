using System.ComponentModel.DataAnnotations;

namespace g2hotel_server.Entities
{
    public abstract class AbstractEntity
    {
        public DateTime? CreatedDate { set; get; }
        [MaxLength(256)]
        public string? CreatedBy { set; get; }
        public DateTime? UpdatedDate { set; get; }
        [MaxLength(256)]
        public string? UpdatedBy { set; get; }
        public bool Status { set; get; }
    }
}