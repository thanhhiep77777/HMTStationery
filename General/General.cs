using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace HMTStationery.General
{
    public static class General
    {
        
    }
    public enum RequestStatus
    {
        WAITING=1,
        APPROVED=2,
        REJECTED=3,
        CANCELED=4,
        WITHDRAWED=5
    }
    public enum UserStatus 
    {
          ENABLE=1,
          DISABLE=2,
    }
}