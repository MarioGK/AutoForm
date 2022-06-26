namespace AutoForm;

public class AutoFieldOptions
{
    public string Title { get; set; } = null!;
    public List<string>? OnlyFields { get; set; }
    public List<string>? OnlySortFields { get; set; }
    public bool EditMode { get; set; }
}