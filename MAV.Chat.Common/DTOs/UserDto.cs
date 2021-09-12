namespace API.DTOs
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Token { get; set; }
        public byte[] ProfilePhoto { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string NameSurname
        {
            get
            {
                return $"{Name} {Surname}";
            }
        }
        public string PhoneNumber { get; set; }
    }
}