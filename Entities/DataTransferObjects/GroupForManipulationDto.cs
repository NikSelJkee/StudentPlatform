using Entities.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class GroupForManipulationDto
    {
        [Required(ErrorMessage = "Введите название группы")]
        [MaxLength(60, ErrorMessage = "Максимальная длина имени группы 60 символов")]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
