using System;
using System.Threading;
using System.Threading.Tasks;
using EscPosServer.Check.ExampleChecks;

namespace EscPosServer
{
    internal class CheckMenu
    {
        private const int AutomaticTimeSleepMs = 5000;

        private const string MenuString = "\n1) Send bar check\n" +
                                          "0) Close\n";

        private readonly EscPosServer _server;

        public CheckMenu(EscPosServer server)
        {
            _server = server;
        }

        public async void StartQuestionLoop(bool isAutomatic)
        {
            _server.StartServer();
            await QuestionLoopInternal(isAutomatic);
            _server.StopServer();
        }

        private async Task QuestionLoopInternal(bool isAutomatic)
        {
            while (true)
            {
                Console.WriteLine(MenuString);
                var answer = isAutomatic ? "1" : Console.ReadLine();

                if (isAutomatic || _server.IsHaveClients)
                {
                    Console.WriteLine("Server doesn't have any clients");
                    Thread.Sleep(AutomaticTimeSleepMs);
                }

                switch (answer)
                {
                    case "1":
                        await _server.WriteCheckAsync(new BarCheck());
                        break;

                    case "0":
                        return;
                }
            }
        }
    }
}