namespace assiment_csad4.Models
{
    public class Role
    {
        public Role()
        {

        }
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int Status { get; set; }
        public virtual ICollection<User>? Users { get; set; }
    }
}
