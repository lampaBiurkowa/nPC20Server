using nDSSH;
using System;

namespace CapsBallServer
{
    class ServerLoop
    {
        bool isRunning;

        public ServerLoop(string address, int port)
        {
            initialize(address, port);
            handle();
        }

        void initialize(string address, int port)
        {
            Receiver.Received += onReceived;
            isRunning = true;
            ServerManager.Initialize(address, port);
            TeamsHandler.Initialize();
        }

        void onReceived(Package package)
        {
            RequestPackage requestPackage = new RequestPackage(package);
            System.Console.WriteLine($"risiwd {package.MessageContent}");
            if (checkIfFakeClient(requestPackage))
                return;

            requestPackage.TryHandle();
        }

        bool checkIfFakeClient(RequestPackage package)
        {
            return false;
        }

        void handle()
        {
            while (isRunning)
            {
                ServerManager.Update();
                BonusHandler.Update();
            }
        }
    }
}