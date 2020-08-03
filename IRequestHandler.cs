namespace CapsBallServer
{
    public interface IRequestHandler
    {
        int ParamsRequiredCount { get; }
        void Handle(RequestPackage package);
    }
}
