namespace AutoForm;

public class AutoFieldBuilders
{
    public static List<IBuildableComponent> BuildableComponents { get; private set; } = new();

    public static List<Type>? AllTypes { get; private set; }


    public static void Create(params Type[] extraAssemblies)
    {
        if (AllTypes == null)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            if (extraAssemblies.Any())
            {
                assemblies = assemblies.Concat(extraAssemblies.Select(a => a.Assembly)).ToArray();
            }

            AllTypes = assemblies.SelectMany(x => x.GetTypes()).ToList();
        }

        var baseComponentType = typeof(IBuildableComponent);
        Console.WriteLine($"Found {AllTypes.Count} types in assemblies");

        var buildableComponentsTypes = AllTypes
                                      .Where(x => baseComponentType.IsAssignableFrom(x) && !x.IsInterface &&
                                                  !x.IsAbstract).ToList();

        Console.WriteLine($"Found {buildableComponentsTypes.Count} components");

        foreach (var type in buildableComponentsTypes)
            try
            {
                var instance = (IBuildableComponent) Activator.CreateInstance(type)!;
                Console.WriteLine("Created instance of " + type.Name);
                BuildableComponents.Add(instance);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        //Order builders 
        BuildableComponents = BuildableComponents.OrderBy(x => x.Priority).ToList();

        Console.WriteLine($"Found {BuildableComponents.Count} buildable components");
    }
}