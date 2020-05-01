namespace CapsBallServer
{
    interface IRequestHandler
    {
        int ParamsRequiredCount { get; }
        void Handle(RequestPackage package);
    }
}
