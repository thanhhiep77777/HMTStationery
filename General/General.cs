using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace HMTStationery.General
{
    public static class General
    {
        
    }

    public enum RequestStatus
    {
        [Display(Name = "WAITING")]
        WAITING =1,
        [Display(Name = "APPROVED")]
        APPROVED = 2,
        [Display(Name = "REJECT")]
        REJECTED = 3,
        [Display(Name = "CANCELED")]
        CANCELED = 4,
        [Display(Name = "WITHDRAWED")]
        WITHDRAWN = 5,
        [Display(Name = "WAITING FOR CANCELING")]
        WAITINGCANCEL = 6
    }
    public enum UserStatus 
    {
        [Display(Name = "ENABLE")]
        ENABLE = 1,
        [Display(Name = "DISABLE")]
        DISABLE =2,
    }
    public enum StationeryStatus
    {
        [Display(Name = "ENABLE")]
        ENABLE = 1,
        [Display(Name = "DISABLE")]
        DISABLE = 2,
    }
}