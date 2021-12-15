using HMTStationery.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMTStationery.General
{
    public static class UserStatic
    {
        public static int ID { get; set; }
        public static string Name { get; set; }
        public static string Email { get; set; }
        public static string Role { get; set; }
        public static bool IsAuthenticated { get; set; }
        public static void SetInfomation(User user)
        {
            ID = user.ID;
            Name = user.Name;
            Email = user.Email;
            IsAuthenticated = true;
        }
        public static void ResetInformation()
        {
            ID = 0;
            Name = "";
            Email = "";
            Role = "";
            IsAuthenticated = false;
        }
    }
}