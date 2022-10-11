using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace ShawkanyDb.Resources
{
    public class SharedControllersResources
    {
        private readonly IStringLocalizer _localizer;

        public SharedControllersResources(IStringLocalizerFactory factory)
        {
            var type = typeof(ControllersResources);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName!);
            _localizer = factory.Create("ControllersResources", assemblyName.Name);
        }

        public LocalizedString Get(string key)
        {
            return _localizer[key];
        }
    }
}
