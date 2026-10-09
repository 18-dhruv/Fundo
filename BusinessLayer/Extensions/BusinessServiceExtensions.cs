using BusinessLayer.Interface;
using BusinessLayer.Service;
using BusinessLayer.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RepositoryLayer.Entity;

namespace BusinessLayer.Extensions
{
    public static class BusinessServiceExtensions
    {
        /// <summary>
        /// Registers settings, business services, and the MessageService HttpClient. Call from Program.cs.
        /// </summary>
        public static IServiceCollection AddBusinessLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JwtSettings>(configuration.GetSection(JwtSettings.SectionName));

            var messageSettings = configuration.GetSection(MessageServiceSettings.SectionName).Get<MessageServiceSettings>();
            if (string.IsNullOrWhiteSpace(messageSettings?.BaseUrl))
            {
                throw new InvalidOperationException("MessageService:BaseUrl is missing from configuration.");
            }

            services.AddHttpClient<IMessageClient, MessageClient>(client =>
            {
                client.BaseAddress = new Uri(messageSettings.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(15);
            });

            services.AddScoped<IPasswordHasher<UserEntity>, PasswordHasher<UserEntity>>();
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserBL, UserBL>();

            return services;
        }
    }
}
