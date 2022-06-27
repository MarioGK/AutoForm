using System.Reflection;
using Microsoft.AspNetCore.Components;

namespace AutoForm;

public abstract class AutoComponentBase<TValue> : ComponentBase
{
    [Parameter]
    public string? LabelName { get; set; }

    [Parameter]
    public string? TypeName { get; set; }

    [Parameter]
    [EditorRequired]
    public TValue? Value { get; set; }

    [Parameter]
    public EventCallback<TValue> ValueChanged { get; set; }

    public string ValueAsString => Value?.ToString() ?? "Value is null.";

    public virtual bool CanBuild(PropertyInfo propertyInfo)
    {
        var result = propertyInfo.PropertyType == typeof(TValue);

        var baseType = propertyInfo.PropertyType.BaseType;

        if (baseType != null && baseType == typeof(TValue))
        {
            Console.WriteLine($"{propertyInfo.Name} is a base type of {typeof(TValue).Name}");
            result = true;
        }

        return result;
    }

    public async Task NotifyValueChanged()
    {
        await ValueChanged.InvokeAsync(Value);
        StateHasChanged();
    }

    public async Task NotifyValueChanged(TValue value)
    {
        Value = value;
        await ValueChanged.InvokeAsync(Value);
        StateHasChanged();
    }
}