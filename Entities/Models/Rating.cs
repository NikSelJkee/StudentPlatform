using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    public class Rating
    {
        public int Id { get; set; }

        [Range(0, 5, ErrorMessage = "Оценка должна находиться в диапазоне по 5-и бальной шкале")]
        public int Result { get; set; }

        [ForeignKey(nameof(Material))]
        public int MaterialId { get; set; }
        public Material Material { get; set; }

        public User User { get; set; }
    }
}
