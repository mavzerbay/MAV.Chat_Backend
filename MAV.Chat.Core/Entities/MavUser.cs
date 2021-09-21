using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace MAV.Chat.Core.Entities
{
    public class MavUser : IdentityUser<int>
    {
        #region BaseEntity
        public DateTime? CreatedDate { get; set; }
        public int? UpdatedById { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public int? DeletedById { get; set; }
        public DateTime? DeletedDate { get; set; }
        #endregion
        public string Name { get; set; }
        public string Surname { get; set; }
        [NotMapped]
        public string NameSurname
        {
            get
            {
                return $"{Name} {Surname}";
            }
        }
        public byte[] ProfilePhoto { get; set; }
        public string About { get; set; }
        public DateTime LastActive { get; set; } = DateTime.Now;
        public ICollection<Message> SendedMessages { get; set; }
        public ICollection<Message> ReceivedMessages { get; set; }
        public ICollection<MavUserRole> UserRoles { get; set; }
        [NotMapped]
        public string PhotoUrl { get; set; }
    }
}
