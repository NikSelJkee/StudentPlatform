using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Entities.Models
{
    public class User : IdentityUser
    {
        [Required(ErrorMessage = "Введите имя студента")]
        [MaxLength(60, ErrorMessage = "Максимальная длина имени студента 60 символов")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Введите фамилию студента")]
        [MaxLength(60, ErrorMessage = "Максимальная длина фамилия студента 60 символов")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Введите отчество студента")]
        [MaxLength(60, ErrorMessage = "Максимальная длина отчества студента 60 символов")]
        public string Patronymic { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(18, MinimumLength = 6, ErrorMessage = "Пароль должен содержать от 6 до 18 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [ForeignKey(nameof(Group))]
        public int? GroupId { get; set; }
        public Group Group { get; set; }
    }
}
