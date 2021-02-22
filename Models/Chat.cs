using System;

namespace GraphiAPI.Models
{
    public class Chat
    {
        public string Id { get; set; }
        public string Topic { get; set; }
        public string CreatedDateTime { get; set; }
        public string LastUpdatedDateTime { get; set; }
        public string ChatType { get; set; }
    }
}
