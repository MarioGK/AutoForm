namespace AutoForm;

public class AutoFieldBuilders
{
    public static List<IBuildableComponent> BuildableComponents { get; private set; } = new();

    public static List<Type> AllTypes { get; }
    
    static AutoFieldBuilders()
    {
        AllTypes = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes()).ToList();
    }

    public static void Create()
    {
        var baseComponentType = typeof(IBuildableComponent);
        Console.WriteLine($"Found {AllTypes.Count} types in assemblies");

        var buildableComponentsTypes = AllTypes
                                      .Where(x => baseComponentType.IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();

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