using System.Collections.Generic;
using BE.CQRS.Domain.DataProtection;
using Microsoft.AspNetCore.DataProtection;

namespace BE.CQRS.DataProtection.AspNetCore
{
    public sealed class AspNetCoreProtectorFactory : IEventDataProtectorFactory
    {
        private readonly IDataProtectionProvider provider;

        public AspNetCoreProtectorFactory(IDataProtectionProvider provider)
        {
            this.provider = provider;
        }

        public IEventDataProtector CreateProtector(string prefix, string[] subScopes)
        {
            var scopes = new List<string>(10)
            {
                prefix
            };
            scopes.AddRange(subScopes);

            var component = provider.CreateProtector("EventSource", scopes.ToArray());
            return new AspNetCoreProtector(component);
        }
    }
}