using AutoForm;

namespace MudBlazorServer.Data;

public class MyUserModel
{
    [FieldOptions("Nome", 0)]
    public string Name { get; set; } = null!;
    
    [FieldOptions("Login", 1)]
    public string Login { get; set; } = null!;
    
    [FieldOptions("Senha", 2)]
    public string Password { get; set; } = null!;
    
    [FieldOptions("Email", 3)]
    public string? Email { get; set; }

    [FieldOptions(false)]
    public string WeDontWantThisOne { get; set; } = null!;
    
    public bool Admin { get; set; } = false;
}