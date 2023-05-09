using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatManager.Models
{
    public class FriendshipsRepository : Repository<Friendships>
    {
        
        public List<Friendships> ToList(int id)
        {
            List<User> Users = DB.Users.ToList();
            List<Friendships> friendships = DB.Friendships.ToList().Where(x => x.IdUser == id).ToList();
            List<Friendships> friend = new List<Friendships>();
            foreach (User user in Users)
            {
                var temp = friendships.Find(x => x.IdUser == user.Id);
                var f = new Friendships();
                f.IdUser = id;
                f.targetUser = user.Id;
                f.friendshipStatus = temp != null ? temp.friendshipStatus : 0;
                f.Avatar = user.Avatar;
                f.fullName = user.GetFullName();
                f.Blocked = user.Blocked;
                f.Online = OnlineUsers.IsOnLine(user.Id);
                friend.Add(f);
            }
            return friend;
        }
    }
}