using App.API.Filters;

namespace App.API.Extensions
{
    public static class ControllerExtension
    {
        public static IServiceCollection AddControllersWithFiltersExt(this IServiceCollection services)
        {
            services.AddControllers(options => {
                options.Filters.Add<FluentValidationFilter>();
                options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
            });


            services.AddScoped(typeof(NotFoundFilter<,>));

            return services;
        }
    }
}
