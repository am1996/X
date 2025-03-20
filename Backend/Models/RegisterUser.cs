using System.ComponentModel.DataAnnotations;
public class RegisterUser{
    [Required,MinLength(6,ErrorMessage ="Username must be at least 6 characters.")]
    required public string UserName { get; set; }
    [Required,EmailAddress(ErrorMessage ="Invalid Email Address")]
    required public string Email { get; set; }
    [Required]
    required public string FirstName{get;set;}
    [Required]
    required public string LastName{get;set;}
    [Required]
    required public string Address{get;set;}
    [Required,MinLength(8,ErrorMessage ="Passwrod must be at least 8 characters")]
    required public string Password { get; set; }
    [Required,MinLength(8),Compare("Password",ErrorMessage ="Passwords do not match")]
    required public string ConfirmPassword { get; set; }
}