using Entities.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    public class Material
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название материала")]
        [MaxLength(120, ErrorMessage = "Максимальная длина названия материала 120 символов")]
        public string Name { get; set; }

        public ICollection<Test> Tests { get; set; }

        [ForeignKey(nameof(Tag))]
        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
