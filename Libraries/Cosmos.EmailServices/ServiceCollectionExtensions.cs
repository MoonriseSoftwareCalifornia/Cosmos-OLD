using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Cosmos.EmailServices
{
    /// <summary>
    /// extension methods for <see cref="IServiceCollection"/>.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the SendGrid Email Provider to the services collection.
        /// </summary>
        /// <param name="services"></param>
        /// <param name="sendGridOptions"></param>
        public static void AddSendGridEmailProvider(this IServiceCollection services, SendGridEmailProviderOptions options)
        {
            services.AddSingleton(Options.Create(options));
            services.AddTransient<IEmailSender, SendGridEmailSender>();
        }

        /// <summary>
        /// Adds the default Azure Email Communication Services
        /// </summary>
        /// <param name="services"></param>
        /// <param name="options"></param>
        public static void AddAzureCommunicationEmailSenderProvider(this IServiceCollection services, AzureCommunicationEmailProviderOptions options)
        {
            services.AddSingleton(Options.Create(options));
            services.AddTransient<IEmailSender, AzureCommunicationEmailSender>();
        }

        /// <summary>
        /// Add SMTP EMail Provider
        /// </summary>
        /// <param name="services"></param>
        /// <param name="sendGridOptions"></param>
        public static void AddSmtpEmailProvider(this IServiceCollection services, SmtpEmailProviderOptions options)
        {
            services.AddSingleton(Options.Create(options));
            services.AddTransient<IEmailSender, SmtpEmailSender>();
        }
    }
}