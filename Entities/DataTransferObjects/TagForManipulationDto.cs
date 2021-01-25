using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class TagForManipulationDto
    {
        [Required(ErrorMessage = "Введите название тега")]
        [MaxLength(120, ErrorMessage = "Максимальная длина тега 120 символов")]
        public string Name { get; set; }
    }
}
