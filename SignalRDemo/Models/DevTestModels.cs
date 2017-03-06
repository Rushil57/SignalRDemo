using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRDemo.Models
{
    public class DevTestModels
    {
        public int ID { get; set; }
        public string CampaignName { get; set; }
        public string Date { get; set; }
        public Nullable<int> Clicks { get; set; }
        public Nullable<int> Conversions { get; set; }
        public Nullable<int> Impressions { get; set; }
        public string AffiliateName { get; set; }

        
    }
}