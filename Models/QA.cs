//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace HMTStationery.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class QA
    {
        public int ID { get; set; }
        public Nullable<int> UserID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string Question { get; set; }
        public string Answer { get; set; }
        public Nullable<int> Status { get; set; }
    
        public virtual User User { get; set; }
    }
}
