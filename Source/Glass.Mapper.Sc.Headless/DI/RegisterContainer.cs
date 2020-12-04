using Glass.Mapper.Sc.LayoutService.Serialization.ItemSerializers;
using Glass.Mapper.Sc.Services;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;

namespace Glass.Mapper.Sc.DI
{
    public class RegisterContainer : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IGlassFieldService, GlassFieldService>();
            
            //Glass Item Serializers
            serviceCollection.AddTransient<IGlassItemSerializer, DefaultGlassItemSerializer>();
            //serviceCollection.Decorate<IGlassItemSerializer, DefaultGlassItemSerializer>();
        }
    }
}