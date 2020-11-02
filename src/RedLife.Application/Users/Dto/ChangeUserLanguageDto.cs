using System.ComponentModel.DataAnnotations;

namespace RedLife.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        [Required]
        public string LanguageName { get; set; }
    }
}