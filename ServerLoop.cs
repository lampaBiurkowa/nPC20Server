﻿using nDSSH;

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
        }

        void onReceived(Package package)
        {
            RequestPackage requestPackage = new RequestPackage(package);
            System.Console.WriteLine("HAAASD");
            if (checkIfFakeClient(requestPackage))
                return;

            requestPackage.TryHandle();
        }

        static bool checkIfFakeClient(RequestPackage package)
        {
            return false;
        }

        void handle()
        {
            while (isRunning)
                ServerManager.Update();
        }
    }
}