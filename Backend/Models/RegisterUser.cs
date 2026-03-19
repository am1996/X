using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

public class RegisterUser{
    [Required,MinLength(6,ErrorMessage ="Username must be at least 6 characters.")]
    [JsonPropertyName("username")]
    public string UserName { get; set; } = "";
    [Required,EmailAddress(ErrorMessage ="Invalid Email Address")]
    [JsonPropertyName("email")]
    public string Email { get; set; } = "";
    [Required]
    [JsonPropertyName("firstname")]
    public string FirstName { get; set; } = "";
    [Required]
    [JsonPropertyName("lastname")]
    public string LastName { get; set; } = "";
    [Required]
    [JsonPropertyName("address")]
    public string Address { get; set; } = "";
    [Required,MinLength(8,ErrorMessage ="Password must be at least 8 characters")]
    [JsonPropertyName("password")]
    public string Password { get; set; } = "";
    [Required,MinLength(8),Compare("Password",ErrorMessage ="Passwords do not match")]
    [JsonPropertyName("confirmPassword")]
    public string ConfirmPassword { get; set; } = "";
}