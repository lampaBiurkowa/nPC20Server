using nDSSH;

namespace CapsBallServer
{
    public class JoinGameRequestHandler : IRequestHandler
    {
        public int ParamsRequiredCount => 0;

        public void Handle(RequestPackage package)
            => IdResolver.AddUser(package.Alias, package.Id);
    }
}
