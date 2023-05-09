using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatManager.Models
{
    public class Friendships
    {
        public int IdUser { get; set; }
        public int targetUser { get; set; }
        public int friendshipStatus { get; set; }
        [JsonIgnore]
        public string Avatar { get; set; }
        [JsonIgnore]
        public string fullName { get; set; }
        [JsonIgnore]
        public bool Blocked { get; set; }
        [JsonIgnore]
        public bool Online { get; set; }
    }
}