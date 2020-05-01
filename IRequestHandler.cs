namespace CapsBallServer
{
    interface IRequestHandler
    {
        void Handle(RequestPackage package);
    }
}
