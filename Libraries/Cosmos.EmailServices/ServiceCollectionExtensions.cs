﻿using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.DependencyInjection;

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
        public static void AddSendGridEmailProvider(this IServiceCollection services, SendGridEmailProviderOptions sendGridOptions)
        {
            services.AddTransient<IEmailSender, SendGridEmailSender>();

            services.Configure<SendGridEmailProviderOptions>(
                configureOptions: options =>
                {
                    options.ApiKey = sendGridOptions.ApiKey;
                    options.Auth = sendGridOptions.Auth;
                    options.UrlPath = sendGridOptions.UrlPath;
                    options.DefaultFromEmailAddress = sendGridOptions.DefaultFromEmailAddress;
                    options.HttpErrorAsException = sendGridOptions.HttpErrorAsException;
                    options.ReliabilitySettings = sendGridOptions.ReliabilitySettings;
                    options.RequestHeaders = sendGridOptions.RequestHeaders;
                    options.SandboxMode = sendGridOptions.SandboxMode;
                    options.Version = sendGridOptions.Version;
                });
        }

    }
}