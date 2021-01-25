using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.Models
{
    public class Group
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Введите название группы")]
        [MaxLength(60, ErrorMessage = "Максимальная длина имени группы 60 символов")]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
