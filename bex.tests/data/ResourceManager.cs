namespace bex.tests.data;

public class ResourceManager
{
    public static Stream? GetEmbeddedResourceAsStream(string name)
    {
        var asm = typeof(ResourceManager).Assembly;
        
        var resRef = asm.GetManifestResourceNames()
            .FirstOrDefault(x => x.EndsWith(name, StringComparison.InvariantCultureIgnoreCase));

        if (resRef == null) return null;
        
        return asm.GetManifestResourceStream(resRef);
    }
}