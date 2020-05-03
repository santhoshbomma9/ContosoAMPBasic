using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection.Metadata.Ecma335;

namespace Dashboard.Models
{   
    public class DimensionViewModel
    {
        public Guid SubscriptionId { get; set; }
        
        public string SubscriptionName { get; set; }
        public string OfferId { get; set; }
        public string PlanId { get; set; }
        public IEnumerable<string> SubscriptionDimensions { get; set; }
        
        [Required]
        public string SelectedDimension { get; set; }
        
        [Required]
        public int Quantity { get; set; }
        
        [Required]
        [DisplayName("Event Time In UTC")]
        public DateTime EventTime { get; set; }
        //public int MyProperty { get; set; }
    }
}