using System;

namespace JacksonVeroneze.Shopping.Common
{
    public abstract class BaseEntity : IBaseEntity
    {
        public DateTime CreatedAt { get; private set; }

        public DateTime? UpdatedAt { get; set; } = null;

        public DateTime? DeletedAt { get; set; } = null;

        public DateTime? LastSync { get; set; } = null;

        public int Version { get; private set; } = 1;

        //
        // Summary:
        //     /// Method responsible for initializing the entity. ///
        //
        public BaseEntity() => CreatedAt = DateTime.Now;

        //
        // Summary:
        //     /// Method responsible for updating the entity. ///
        //
        public void UpdateLastSync() => LastSync = DateTime.Now;

        //
        // Summary:
        //     /// Method responsible for updating the entity. ///
        //
        public void IncrementVersion() => Version++;

        //
        // Summary:
        //     /// Method responsible for returning a string 
        //     representation of the object. ///
        //
        public override string ToString()
            => $"CreatedAt: {CreatedAt}, UpdatedAt: {UpdatedAt}, DeletedAt: {DeletedAt}, LastSync: {LastSync}, Version: {Version}";
    }
}