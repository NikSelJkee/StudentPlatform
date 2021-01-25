using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class RatingForManipulationDto
    {
        [Range(0, 5, ErrorMessage = "Оценка должна находиться в диапазоне по 5-и бальной шкале")]
        public int Result { get; set; }
    }
}
