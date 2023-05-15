namespace assiment_csad4.Models
{
    public class Cart
    {

        public Guid IdUser { get; set; }
        public string? Description { get; set; }

        public virtual User? User { get; set; }
        public virtual ICollection<CartDetail>? CartDetails { get; set; }
    }
}
