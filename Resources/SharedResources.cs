using Microsoft.AspNetCore.Mvc.Localization;
using Microsoft.Extensions.Localization;
using System.Reflection;

namespace ShawkanyDb.Resources
{
    public class SharedResources
    {
        private readonly IHtmlLocalizer _localizer;

        public SharedResources(IHtmlLocalizerFactory factory)
        {
            var type = typeof(Shared);
            var assemblyName = new AssemblyName(type.GetTypeInfo().Assembly.FullName!);
            _localizer = factory.Create("Shared", assemblyName.Name);
        }

        public LocalizedHtmlString Get(string key)
        {
            return _localizer[key];
        }
    }
}
