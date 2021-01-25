using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class TestForManipulationDto
    {
        [Required(ErrorMessage = "Введите вопрос")]
        [MaxLength(120, ErrorMessage = "Максимальная длина вопрос 120 символов")]
        public string Question { get; set; }

        [Required(ErrorMessage = "Введите ответ")]
        public string Answer1 { get; set; }

        [Required(ErrorMessage = "Введите ответ")]
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string Answer5 { get; set; }
        public string Answer6 { get; set; }

        [Required(ErrorMessage = "Введите правильный ответ")]
        public string CorrectAnswer { get; set; }
    }
}
