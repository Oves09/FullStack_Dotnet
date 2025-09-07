using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace TagHelpersDemo.Pages
{
    public class RegisterModel : PageModel
    {
        [BindProperty]
        public UserInput Input { get; set; } = new UserInput();

        public string Message { get; set; } = "";

        public void OnPost()
        {
            if (ModelState.IsValid)
            {
                Message = $"Registration Successful! Welcome, {Input.Name}";
            }
        }
    }

    public class UserInput
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
