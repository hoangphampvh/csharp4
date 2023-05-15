namespace assiment_csad4.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        public Guid IdRole { get; set; }
        public int Status { get; set; }

        public virtual Cart? Cart { get; set; }
        public virtual Role? RoleNavigation { get; set; }
        public virtual ICollection<Bill>? Bills { get; set; }

    }
}
