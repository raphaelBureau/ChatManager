using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatManager.Models
{
    public class FriendshipsRepository : Repository<Friendship>
    {
        public List<Friendship> ToList(int id)
        {
            var Users = DB.Users.ToList().Where(x => x.Id != id).ToList();
            var friendships = DB.Friendships.ToList().Where(x => x.Id == id).ToList();
            var friend = new List<Friendship>();
            foreach (User user in Users)
            {
                var temp = friendships.Find(x => x.Id == user.Id);
                var f = new Friendship();
                f.Id = id;
                f.targetUserId = user.Id;
                f.friendshipStatus = temp != null ? temp.friendshipStatus : 0;
                f.Avatar = user.GetAvatarURL();
                f.fullName = user.GetFullName();
                f.Blocked = user.Blocked;
                f.Online = OnlineUsers.IsOnLine(user.Id);
                friend.Add(f);
            }
            return friend;
        }
    }
}