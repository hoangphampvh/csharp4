namespace assiment_csad4.Models
{
    public class Bill
    {

        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public string MaHd { get; set; }
        public Guid UserId { get; set; }
        public int Status { get; set; }
        public virtual ICollection<BillDetail>? BillDetails { get; set; }
        public virtual User? UserNavigation { get; set; }

    }
}
