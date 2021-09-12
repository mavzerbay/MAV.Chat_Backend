namespace MAV.Chat.Common.Helpers
{
    public class BaseSpecParams
    {
        private const int MaxPageSize = 15;
        public int PageIndex { get; set; } = 1;
        private int _pageSize = 10;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxPageSize ? MaxPageSize : value);
        }
        public string Sort { get; set; }
        private string _search;
        public string Search
        {
            get => _search;
            set => _search = value.ToLower();
        }
    }
}