using nDSSH;

namespace CapsBallServer
{
    class JoinGameRequestHandler : IRequestHandler
    {
        public int ParamsRequiredCount { get; } = 0;

        public void Handle(RequestPackage package)
        {
            IdResolver.AddUser(package.Alias, package.Id);
        }
    }
}
