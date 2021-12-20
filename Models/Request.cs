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
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public partial class Request
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Request()
        {
            this.RequestDetails = new HashSet<RequestDetail>();
        }
    
        public int ID { get; set; }
        [Display(Name ="Title (Optional - Only you can see this infomation)")]
        public string Title { get; set; }
        public Nullable<int> SenderID { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public Nullable<int> Status { get; set; }
        [EmailAddress(ErrorMessage ="Email is invalid")]
        [Required(ErrorMessage ="Please enter email")]
        [Remote("IsEmail")]
        public string ReceiverEmail { get; set; }
        public string RequestMessage { get; set; }
        public string ResponseMessage { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RequestDetail> RequestDetails { get; set; }
        public virtual User User { get; set; }
    }
}
