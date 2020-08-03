using nDSSH;

namespace CapsBallServer
{
    public class JoinGameRequestHandler : IRequestHandler
    {
        public int ParamsRequiredCount { get; } = 0;

        public void Handle(RequestPackage package)
        {
            System.Console.WriteLine($"JEA {package.Alias}");
            IdResolver.AddUser(package.Alias, package.Id);
        }
    }
}
