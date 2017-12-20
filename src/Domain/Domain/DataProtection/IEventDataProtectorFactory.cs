namespace BE.CQRS.Domain.DataProtection
{
    public interface IEventDataProtectorFactory
    {
        IEventDataProtector CreateProtector(string prefix,params string[] subScopes);
    }
}