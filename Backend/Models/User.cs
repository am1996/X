using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

public class User : IdentityUser{
    required public string FirstName{get;set;}
    required public string LastName{get;set;}
    required public string Address{get;set;}
}