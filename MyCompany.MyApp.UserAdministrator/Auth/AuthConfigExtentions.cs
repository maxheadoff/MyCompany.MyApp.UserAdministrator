using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MyCompany.MyApp.UserAdministrator.Auth
{
    public static class AuthConfigExtentions
    {
        public static IServiceCollection AddCustomJwtBearer(this IServiceCollection services)
        {
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.RequireHttpsMetadata = true;
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            // will Issuer be validated
                            ValidateIssuer = true,
                            ValidIssuer = AuthOptions.ISSUER,
                            // will Audience be validated
                            ValidateAudience = true,
                            // the presentation string of audience
                            ValidAudience = AuthOptions.AUDIENCE,
                            // will lifetime be validated
                            ValidateLifetime = true,

                            // setting security key
                            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
                            // security key validation wil be
                            ValidateIssuerSigningKey = true,
                        };
                    });

            return services;
        }
    }
}
