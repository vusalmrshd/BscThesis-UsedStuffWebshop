using System;
namespace Models.Database.Entity
{
    public class EntityPersistent
    {
        public long createdBy { get; set; }
        public DateTimeOffset createdAt { get; set; }
        public long updatedBy { get; set; }
        public DateTimeOffset updatedAt { get; set; }
        public bool isActive { get; set; }
    }
}
