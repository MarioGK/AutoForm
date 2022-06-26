namespace AutoForm;

public class AutoOptions : Attribute
{
    public AutoOptions()
    {
        Fields = true;
        Table = true;
        DisplayName = "";
        Order = 500;
    }

    public AutoOptions(string name, int order = 0)
    {
        Fields = true;
        Table = true;
        DisplayName = name;
        Order = order;
    }
    
    public AutoOptions(string name, string customType, int order = 0)
    {
        Fields = true;
        Table = true;
        DisplayName = name;
        Order = order;
        CustomType = customType;
    }
    
    public AutoOptions(bool show)
    {
        Fields = show;
        Table = show;
        DisplayName = "";
        Order = 500;
    }

    public AutoOptions(bool fields, bool table, string displayName = "", int order = 500)
    {
        Fields = fields;
        Table = table;
        DisplayName = displayName;
        Order = order;
    }   

    public string DisplayName { get; }
    public int Order { get; }
    public bool Fields { get; }
    public bool Table { get; }

    public string? CustomType { get; set; }
}