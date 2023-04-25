using Microsoft.AspNetCore.Mvc.Rendering;

namespace ST2Project.UI.Models
{
    public class OfferModel
    {
        public OfferModel()
        {
            var tmp = new List<string>() { "New", "Used"};
            var tmp2 = new List<string>() { "Warsaw", "Cracow", "Rzeszow", "Lublin", "Wrocław", "Global" };
            Conditions = new SelectList(tmp);
            Locations = new SelectList(tmp2);
        }
        public int OfferId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal? Price { get; set; }
        public string Condition { get; set; }
        public string Location { get; set; }
        public SelectList Conditions { get; set; }
        public SelectList Locations { get; set; }
    }
}
