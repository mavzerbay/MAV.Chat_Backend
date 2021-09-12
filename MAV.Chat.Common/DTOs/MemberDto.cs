using System;
using System.Collections.Generic;

namespace MAV.Chat.Common.DTOs
{
    public class MemberDto
    {
         public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string NameSurname
        {
            get
            {
                return $"{Name} {Surname}";
            }
        }
        public string UserName { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public string About { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime LastActive { get; set; }
    }
}