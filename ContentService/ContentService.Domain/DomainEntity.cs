using System.ComponentModel.DataAnnotations;

namespace ContentService.Domain
{
    public abstract class DomainEntity
    {
        public int Id { get; protected set; }
        [Timestamp]
        public uint RowVersion { get; protected set; }
    }
}
