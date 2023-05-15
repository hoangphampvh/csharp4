using System.Text.Json.Serialization;
namespace assiment_csad4.ViewModel
{
    public class PagingModel
    {
        public int currentpage { get; set; }
        public int countpages { get; set; }
        [JsonIgnore]
        public Func<int?, string> generateUrl { get; set; }
    }
}
