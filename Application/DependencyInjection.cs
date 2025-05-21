using Application.Interfaces;
using Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            var assembly = typeof(DependencyInjection).Assembly;

            services.AddMediatR(configuration => configuration.RegisterServicesFromAssembly(assembly));
            services.AddAutoMapper(assembly);

            services.AddValidatorsFromAssembly(assembly);
            services.AddFluentValidationAutoValidation();
            services.AddValidatorsFromAssemblyContaining<UserRegisterDtoValidator>();

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(Application.Behaviors.ValidationBehavior<,>));

            return services;
        }
    }
}
