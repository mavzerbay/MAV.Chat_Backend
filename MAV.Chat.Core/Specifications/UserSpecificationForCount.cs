using MAV.Chat.Common.Helpers;
using MAV.Chat.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAV.Chat.Core.Specifications
{
    public class UserSpecificationForCount : BaseSpecification<MavUser>
    {
        public UserSpecificationForCount(UserSpecParams userSpecParams)
               : base(x =>
                   (string.IsNullOrEmpty(userSpecParams.Search) || x.Name.ToLower().Contains(userSpecParams.Search.ToLower())) || x.Surname.ToLower().Contains(userSpecParams.Search.ToLower()) &&
                   (!userSpecParams.WithOutCurrent.HasValue || (userSpecParams.WithOutCurrent.HasValue && x.UserName!=userSpecParams.UserName)) &&
                   (string.IsNullOrEmpty(userSpecParams.PhoneNumber) || x.PhoneNumber == userSpecParams.PhoneNumber) &&
                   (string.IsNullOrEmpty(userSpecParams.UserName) || x.UserName.ToLower().Contains(userSpecParams.UserName.ToLower())))
        {
        }
    }
}
