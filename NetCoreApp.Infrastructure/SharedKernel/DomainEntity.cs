using System.ComponentModel.DataAnnotations.Schema;

namespace NetCoreApp.Infrastructure.SharedKernel
{
    public abstract class DomainEntity<T>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public T Id { get; set; }

        /// <summary>
        /// True if domain entity has an identity
        /// </summary>
        /// <returns></returns>
        public bool IsTransient()
        {
            return Id.Equals(default(T));
        }
    }
}
