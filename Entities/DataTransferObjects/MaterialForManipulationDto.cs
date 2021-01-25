using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class MaterialForManipulationDto
    {
        [Required(ErrorMessage = "Введите название материала")]
        [MaxLength(120, ErrorMessage = "Максимальная длина названия материала 120 символов")]
        public string Name { get; set; }
    }
}
