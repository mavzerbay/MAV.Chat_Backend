using MAV.Chat.Common.Helpers;
using MAV.Chat.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MAV.Chat.Core.Specifications
{
    public class UserSpecification : BaseSpecification<MavUser>
    {
        public UserSpecification(UserSpecParams userSpecParams)
               : base(x =>
                   (string.IsNullOrEmpty(userSpecParams.Search) || x.Name.ToLower().Contains(userSpecParams.Search.ToLower())) || x.Surname.ToLower().Contains(userSpecParams.Search.ToLower()) &&
                   (string.IsNullOrEmpty(userSpecParams.PhoneNumber) || x.PhoneNumber == userSpecParams.PhoneNumber) &&
                   (string.IsNullOrEmpty(userSpecParams.UserName) || x.UserName.ToLower().Contains(userSpecParams.UserName.ToLower())))
        {
            AddOrderBy(x => x.Name);
            ApplyPaging(userSpecParams.PageSize * (userSpecParams.PageIndex - 1), userSpecParams.PageSize);
            if (!string.IsNullOrEmpty(userSpecParams.Sort))
            {
                switch (userSpecParams.Sort)
                {
                    case "createdAsc":
                        AddOrderBy(x => x.CreatedDate);
                        break;
                    case "createdDesc":
                        AddOrderByDescending(x => x.CreatedDate);
                        break;
                    case "nameAsc":
                        AddOrderBy(x => x.Name);
                        break;
                    case "nameDesc":
                        AddOrderByDescending(x => x.Name);
                        break;
                    case "surnameAsc":
                        AddOrderBy(x => x.Surname);
                        break;
                    case "surnameDesc":
                        AddOrderByDescending(x => x.Surname);
                        break;
                    case "phoneNumberAsc":
                        AddOrderBy(x => x.PhoneNumber);
                        break;
                    case "phoneNumberDesc":
                        AddOrderByDescending(x => x.PhoneNumber);
                        break;
                }
            }
        }

        public UserSpecification(int id) : base(x => x.Id == id)
        {

        }
        public UserSpecification(string userName) : base(x => x.UserName == userName)
        {

        }
    }
}
