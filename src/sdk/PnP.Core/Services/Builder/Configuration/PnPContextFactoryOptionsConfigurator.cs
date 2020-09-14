﻿using Microsoft.Extensions.Options;
using System;

namespace PnP.Core.Services.Builder.Configuration
{
    internal class PnPContextFactoryOptionsConfigurator: 
        IConfigureOptions<PnPContextFactoryOptions>, 
        IConfigureOptions<OAuthAuthenticationProviderOptions>
    {
        private readonly IOptions<PnPCoreOptions> pnpCoreOptions;

        public PnPContextFactoryOptionsConfigurator(IOptions<PnPCoreOptions> pnpCoreOptions)
        {
            this.pnpCoreOptions = pnpCoreOptions;
        }

        public void Configure(PnPContextFactoryOptions options)
        {
            foreach (var (optionKey, optionValue) in pnpCoreOptions.Value.Sites)
            {
                options.Configurations.Add(new PnPContextFactoryOptionsConfiguration { 
                    Name = optionKey,
                    SiteUrl = new Uri(optionValue.SiteUrl),
                    AuthenticationProviderName = optionValue.AuthenticationProviderName
                });
            }
        }

        public void Configure(OAuthAuthenticationProviderOptions options)
        {
            foreach (var (optionKey, optionValue) in pnpCoreOptions.Value.Credentials)
            {
                if (!String.IsNullOrEmpty(optionValue.CredentialManagerName))
                {
                    options.Configurations.Add(new OAuthCredentialManagerConfiguration {
                        Name = optionKey,
                        ClientId = optionValue.ClientId,
                        CredentialManagerName = optionValue.CredentialManagerName
                    });
                }
                else if (!String.IsNullOrEmpty(optionValue.CertificateThumbprint))
                {
                    options.Configurations.Add(new OAuthCertificateConfiguration
                    {
                        Name = optionKey,
                        ClientId = optionValue.ClientId,
                        Certificate = null, // TODO: Load certificate from thumbprint
                    });
                }
            }
        }
    }
}