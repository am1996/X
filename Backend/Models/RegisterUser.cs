using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class RegisterUser{
    [Required,MinLength(6,ErrorMessage ="Username must be at least 6 characters.")]
    [JsonPropertyName("username")]
    required public string UserName { get; set; }
    [Required,EmailAddress(ErrorMessage ="Invalid Email Address")]
    [JsonPropertyName("email")]
    required public string Email { get; set; }
    [Required]
    [JsonPropertyName("firstname")]
    required public string FirstName{get;set;}
    [Required]
    [JsonPropertyName("lastname")]
    required public string LastName{get;set;}
    [Required]
    [JsonPropertyName("address")]
    required public string Address{get;set;}
    [Required,MinLength(8,ErrorMessage ="Password must be at least 8 characters")]
    [JsonPropertyName("password")]
    required public string Password { get; set; }
    [Required,MinLength(8),Compare("Password",ErrorMessage ="Passwords do not match")]
    [JsonPropertyName("confirmPassword")]
    required public string ConfirmPassword { get; set; }
}