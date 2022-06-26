using System.Reflection;

namespace AutoForm;

public interface IBuildableComponent
{
    int Priority { get; }

    bool CanBuild(PropertyInfo propertyInfo);
}