using System.ComponentModel.DataAnnotations;

namespace Common.BaseEntity
{
    /// <summary>
    /// کلاس پایه موجودیت
    /// </summary>
    /// <typeparam name="TKey">شناسه ی اصلی</typeparam>
    public abstract class EntityBase<TKey>
    {
        #region Properties
        /// <summary>
        /// کلید اصلی
        /// </summary>
        [Key]
        public virtual TKey Id { get; set; }
        #endregion
    }
}
