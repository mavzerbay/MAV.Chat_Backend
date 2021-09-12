namespace MAV.Chat.Common.Helpers
{
    public class MessageSpecParams : BaseSpecParams
    {
        /// <summary>
        /// CurrentUserName
        /// </summary>
        public string UserName { get; set; }
        public string ReceiverUserName { get; set; }
        public bool GetMessageThread { get; set; } = false;
        public string Container { get; set; } = "Unread";
    }
}