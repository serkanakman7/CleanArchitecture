using App.API.ExceptionHandler;

namespace App.API.Extensions
{
    public static class ExceptionHandlerExtension
    {
        public static IServiceCollection AddExceptionHandlerExt(this IServiceCollection services)
        {
            services.AddExceptionHandler<CriticalExceptionHandler>();
            services.AddExceptionHandler<GlobalExceptionHandler>();

            return services;
        }
    }
}
