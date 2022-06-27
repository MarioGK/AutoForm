using System.Reflection;
using AutoForm.Extensions;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;

namespace AutoForm;

public class AutoFields<T> : ComponentBase
{
    [Parameter]
    [EditorRequired]
    public T? Model { get; set; }

    [Parameter]
    public AutoFieldOptions? Options { get; set; } = new();

    //public bool EditMode => Model?.Id != null;

    public string Name => Model?.GetType().Name.ToFriendlyCase() ?? "null";

    private List<PropertyInfo> GetProperties()
    {
        Options ??= new AutoFieldOptions();

        if (Model is null)
        {
            return new List<PropertyInfo>();
        }

        var props = Model.GetType()
                         .GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.FlattenHierarchy)
                         .Where(p => !p.GetIndexParameters().Any() && p.GetCustomAttribute<FieldOptions>() != null)
                         .OrderBy(x => x.GetCustomAttribute<FieldOptions>()?.Order ?? 5000)
                         .ToList();

        if (Options.OnlyFields != null && Options.OnlyFields.Any())
        {
            var fieldsToBuild = props
                               .Where(x => Options.OnlyFields.Contains(x.Name))
                               .OrderBy(x => Options.OnlyFields.IndexOf(x.Name))
                               .ToList();

            return fieldsToBuild;
        }

        if (Options.OnlySortFields != null && Options.OnlySortFields.Any())
        {
            return props
                  .OrderBy(x =>
                               Options.OnlySortFields.IndexOf(x.Name) == -1
                                   ? props.Count
                                   : Options.OnlySortFields.IndexOf(x.Name))
                  .ToList();
        }

        //Ignore default fields

        props = props.Where(x => x.GetCustomAttribute<FieldOptions>()!.Create).ToList();

        return props;
    }

    /// <inheritdoc />
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        foreach (var propertyInfo in GetProperties())
        {
            var componentBuilder = AutoFieldBuilders.BuildableComponents.FirstOrDefault(b => b.CanBuild(propertyInfo));

            try
            {
                if (componentBuilder != null && Model != null)
                {
                    var options = propertyInfo.GetCustomAttribute<FieldOptions>();
                    var labelName = string.IsNullOrEmpty(options?.DisplayName)
                                        ? propertyInfo.Name.ToFriendlyCase()
                                        : options.DisplayName;

                    var componentType = componentBuilder.GetType();
                    builder.OpenComponent(0, componentType);
                    try
                    {
                        var propertyValue = propertyInfo.GetValue(Model);
                        builder.AddAttribute(1, "LabelName",    labelName);
                        builder.AddAttribute(2, "Value",        propertyValue);
                        builder.AddAttribute(3, "ValueChanged", CreateGenericValueChanged(propertyInfo, Model));
                        builder.AddAttribute(4, "TypeName",     propertyInfo.PropertyType.Name);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                    }
                    finally
                    {
                        builder.CloseComponent();
                    }
                }
            }
            catch (Exception e)
            {
                // ReSharper disable once TemplateIsNotCompileTimeConstantProblem
                // ReSharper disable once ExceptionPassedAsTemplateArgumentProblem
                //Log.Error($"Error building component for property {Model?.GetType().Name}.{propertyInfo.Name} with component builder {componentBuilder}: {ex.Message}{Environment.NewLine}{ex.StackTrace}", ex);
                Console.WriteLine(e);
            }
        }
    }

    private object? CreateGenericValueChanged(PropertyInfo propertyInfo, object instance)
    {
        //TODO make this better
        var methods = EventCallback.Factory.GetType().GetMethods(BindingFlags.Public | BindingFlags.Instance)
                                   .Where(m => m.Name == nameof(EventCallback.Factory.Create)).ToList();
        var method = methods[8];

        var type = propertyInfo.PropertyType;

        //If the type has a base type use it instead, so components can be inherited from
        var baseType = type.BaseType;
        if (baseType == null)
        {
            type = baseType;
        }

        var genericMethod = method.MakeGenericMethod(type);

        var makeActionMethod = GetType().GetMethod("MakeAction", BindingFlags.NonPublic | BindingFlags.Instance)!;
        var makeActionGenericMethod = makeActionMethod.MakeGenericMethod(type);
        var action = makeActionGenericMethod.Invoke(this, new object[] {propertyInfo})!;

        var parameters = new[] {this, action};
        return genericMethod.Invoke(EventCallback.Factory, parameters);
    }

    private Action<TEntity> MakeAction<TEntity>(PropertyInfo propertyInfo)
    {
        // your custom action here.
        return value =>
        {
            try
            {
                if (propertyInfo.SetMethod is null)
                {
                    return;
                }

                propertyInfo.SetValue(Model, value);
                //Console.WriteLine(value);
            }
            catch (Exception e)
            {
                //Log.Error(e, "Failed to set value to the model");
                Console.WriteLine("Failed to set value to the model");
            }
        };
    }
}