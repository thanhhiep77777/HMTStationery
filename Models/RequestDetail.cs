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
    
    public partial class RequestDetail
    {
        public int ID { get; set; }
        public Nullable<int> RequestID { get; set; }
        public Nullable<int> StationeryID { get; set; }
        public Nullable<int> Quantity { get; set; }
        public Nullable<double> Price { get; set; }
    
        public virtual Request Request { get; set; }
        public virtual Stationery Stationery { get; set; }
    }
}
