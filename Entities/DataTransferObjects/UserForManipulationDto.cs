using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Entities.DataTransferObjects
{
    public class UserForManipulationDto
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

        [Required(ErrorMessage = "Введите логин")]
        [MaxLength(30, ErrorMessage = "Максимальная длина логина 30 символов")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "Введите электронную почту")]
        [EmailAddress(ErrorMessage = "Неправильный формат электронной почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите пароль")]
        [StringLength(18, MinimumLength = 6, ErrorMessage = "Пароль должен содержать от 6 до 18 символов")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Повторите пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
