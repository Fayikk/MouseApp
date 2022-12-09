using API.Helpers;

namespace API.Helper
{
    public class MessageParams : PaginationParams
    {
        public string Username { get; set; }

        public string Container { get; set; } = "Unread";
    }
}