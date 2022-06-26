namespace AutoForm;

public class FieldOptions : Attribute
{
    public FieldOptions()
    {
        Create      = true;
        DisplayName = "";
        Order       = 500;
    }

    public FieldOptions(string name, int order = 0)
    {
        Create      = true;
        DisplayName = name;
        Order       = order;
    }
    
    public FieldOptions(string name, string customType, int order = 0)
    {
        Create      = true;
        DisplayName = name;
        Order       = order;
        CustomType  = customType;
    }
    
    public FieldOptions(bool show)
    {
        Create      = show;
        DisplayName = "";
        Order       = 500;
    }

    public FieldOptions(bool create, string displayName = "", int order = 500)
    {
        Create      = create;
        DisplayName = displayName;
        Order       = order;
    }   

    public string DisplayName { get; }
    public int Order { get; }
    public bool Create { get; }

    public string? CustomType { get; set; }
}