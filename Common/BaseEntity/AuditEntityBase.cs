using System;

namespace Common.BaseEntity
{
    /// <summary>
    /// کلاس پایه موجودیت
    /// </summary>
    /// <typeparam name="TKey">شناسه ی اصلی</typeparam>
    public abstract class AuditEntityBase<TKey> : EntityBase<TKey>
    {
        #region Properties
        /// <summary>
        /// تاریخ ایجاد
        /// </summary>
        public virtual DateTime CreateDate { get; set; }

        /// <summary>
        /// ایجاد کننده
        /// </summary>
        public virtual long CreateBy { get; set; }

        /// <summary>
        /// تاریخ ویرایش
        /// </summary>
        public virtual DateTime? UpdateDate { get; set; }

        /// <summary>
        /// ویرایش کننده
        /// </summary>
        public virtual long? UpdateBy { get; set; }
        #endregion
    }
}
