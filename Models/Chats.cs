using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ChatManager.Models
{
    public class Chats
    {
        public int Id { get; set; }
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        public List<(DateTime date, string message)> UserId1Messages;
        public List<(DateTime date, string message)> UserId2Messages;

        
        public Chats()
        {
            if (UserId1Messages == null)
                UserId1Messages = new List<(DateTime date, string message)>();
            if (UserId2Messages == null)
                UserId2Messages = new List<(DateTime date, string message)>();
        }
    }
}