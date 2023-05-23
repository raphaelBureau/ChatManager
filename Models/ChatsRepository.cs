using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatManager.Models
{
    public class ChatsRepository : Repository<Chats>
    {
        public Chats GetChatByUsers(int user1, int user2)
        {
            return ToList().FirstOrDefault((c) => (c.UserId1 == user1 && c.UserId2 == user2) || (c.UserId1 == user2 && c.UserId2 == user1));
        }
    }
}