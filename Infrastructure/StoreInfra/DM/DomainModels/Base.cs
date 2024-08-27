

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.DomainModels
{
    public abstract class Base<T>
    {
        [Key]
        public virtual T Id { get; set; }


        [Required(ErrorMessage = $"{nameof(CreatedBy)} is required")]
        public virtual string CreatedBy { get; set; }


        [Required(ErrorMessage = $"{nameof(UpdatedBy)} is required")]
        public virtual string UpdatedBy { get; set; }


        [Required(ErrorMessage = $"{nameof(CreatedDate)} is required")]
        public virtual DateTime CreatedDate { get; set; }


        [Required(ErrorMessage = $"{nameof(UpdatedDate)} is required")]
        public virtual DateTime UpdatedDate { get; set; }
        [Required(ErrorMessage = $"{nameof(IsActive)} is required")]
        public virtual bool IsActive { get; set; }
        [Required(ErrorMessage = $"{nameof(IsArchived)} is required")]
        public virtual bool IsArchived { get; set; }

    }
}