using Application.Authorize.Queries;
using Application.Validators;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

namespace Application
    {
        public static class DependencyInjection
        {
            public static IServiceCollection AddApplicationServices(this IServiceCollection services)
            {
                var appAssembly = typeof(DependencyInjection).Assembly;
                var queryAssembly = typeof(GetTodayVoteTallyQuery).Assembly;

                // One AddMediatR call that registers both assemblies:
                services.AddMediatR(cfg =>
                {
                    cfg.RegisterServicesFromAssembly(appAssembly);
                    if (queryAssembly != appAssembly)
                        cfg.RegisterServicesFromAssembly(queryAssembly);
                });

                services.AddAutoMapper(appAssembly);
                services.AddValidatorsFromAssembly(appAssembly);
                services.AddFluentValidationAutoValidation();
                services.AddValidatorsFromAssemblyContaining<UserRegisterDtoValidator>();

                return services;
            }
        }

    }
