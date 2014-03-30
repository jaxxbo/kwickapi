namespace Kwick.Data
{
    public class KUser : IVersionedModelObject
    {
        public virtual long KUserId { get; set; }
        public virtual string Mobile { get; set; }
        public virtual string Email { get; set; }
        public virtual int Approved { get; set; }
    }
}
