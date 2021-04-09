using Lamar;
using Microsoft.Extensions.Configuration;

namespace UsersApi
{
    public class ApplicationRegistry : ServiceRegistry
    {
        public ApplicationRegistry(IConfiguration configuration)
        {
            Scan(scanner =>
            {
                scanner.TheCallingAssembly();
                scanner.WithDefaultConventions();
                scanner.AssembliesAndExecutablesFromApplicationBaseDirectory(assembly =>
                    assembly.GetName().Name.StartsWith("UsersApi."));
            });
        }
    }
}
