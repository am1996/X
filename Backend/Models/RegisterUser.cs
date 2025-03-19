using System.ComponentModel.DataAnnotations;
public class RegisterUser{
    [Required,MinLength(6)]
    required public string UserName { get; set; }
    [Required,EmailAddress(ErrorMessage ="Invalid Email Address")]
    required public string Email { get; set; }
    [Required]
    required public string FirstName{get;set;}
    [Required]
    required public string LastName{get;set;}
    [Required]
    required public string Address{get;set;}
    [Required,MinLength(8)]
    required public string Password { get; set; }
    [Required,MinLength(8),Compare("Password",ErrorMessage ="Passwords do not match")]
    required public string ConfirmPassword { get; set; }
}