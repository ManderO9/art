namespace Art.UI;

public class DI
{
    public static IServiceProvider ServiceProvider { get; private set; } = default!;

    public static void Init(IServiceProvider serviceProvider)
    {
        ServiceProvider = serviceProvider;
    }

}
