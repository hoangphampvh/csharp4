namespace assiment_csad4.Models
{
    public class CartDetail
    {
        public CartDetail()
        {

        }
        public Guid Id { get; set; }
        public Guid IdUser { get; set; }
        public Guid IdProduct { get; set; }
        public int Quantilty { get; set; }
        public int? Status { get; set; }
        public virtual Cart? CartNavigation { set; get; }
        public virtual Product? ProductNavigation { get; set; }

    }
}
