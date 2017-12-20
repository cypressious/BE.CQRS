using System;
using BE.CQRS.Domain.DataProtection;
using BE.FluentGuard;
using Microsoft.AspNetCore.DataProtection;

namespace BE.CQRS.DataProtection.AspNetCore
{
    public sealed class AspNetCoreProtector : IEventDataProtector
    {
        private readonly IDataProtector protector;

        public AspNetCoreProtector(IDataProtector protector)
        {
            Precondition.For(protector, nameof(protector)).NotNull(message:"Protector has to bet set");

            this.protector = protector;
        }
        public string Protect(string plainText)
        {
            return protector.Protect(plainText);
        }

        public byte[] Protect(byte[] plainText)
        {
            return protector.Protect(plainText);
        }

        public string Unprotect(string proctectedText)
        {
            return protector.Unprotect(proctectedText);
        }

        public byte[] Unprotect(byte[] proctectedText)
        {
            return protector.Unprotect(proctectedText);
        }
    }
}
