using System;
namespace Core.Dto
{
    public class DevTestDto
    {
        public int ID { get; set; }
        public string CampaignName { get; set; }
        public Nullable<DateTime> Date { get; set; }
        public Nullable<int> Clicks { get; set; }
        public Nullable<int> Conversions { get; set; }
        public Nullable<int> Impressions { get; set; }
        public string AffiliateName { get; set; }
    }
}
