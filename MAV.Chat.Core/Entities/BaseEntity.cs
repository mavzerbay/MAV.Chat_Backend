using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace MAV.Chat.Core.Entities
{
    public class BaseEntity
    {
        public int Id { get; set; }
        public int CreatedById { get; set; }
        [NotMapped]
        public MavUser CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public int? UpdatedById { get; set; }
        [NotMapped]
        public MavUser UpdatedBy { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeletedById { get; set; }
        [NotMapped]
        public MavUser DeletedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
    }
}
