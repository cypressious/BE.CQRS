namespace BE.CQRS.Domain.DataProtection
{
    public interface IEventDataProtector
    {
        string Protect(string plainText);

        byte[] Protect(byte[] plainText);

        string Unprotect(string proctectedText);

        byte[] Unprotect(byte[] proctectedContent);
    }
}