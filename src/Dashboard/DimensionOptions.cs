using System.Collections.Generic;

namespace Dashboard
{
    public class DimensionOptions
    {
        public List<Offer> Offers { get; set; }        
    }
    public class Offer
    {
        public string OfferId { get; set; }
        public List<Plan> Plans { get; set; }
    }
    public class Plan
    {
        public string PlanId { get; set; }
        public List<string> Dimensions { get; set; }
    }
}
