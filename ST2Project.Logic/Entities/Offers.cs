//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ST2Project.Logic.Entities
{
    using System;
    using System.Collections.Generic;
    
    public partial class Offers
    {
        public int OfferID { get; set; }
        public int OwnerID { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public Nullable<decimal> Price { get; set; }
        public string Location { get; set; }
        public string Condition { get; set; }
    
        public virtual Users Users { get; set; }
    }
}