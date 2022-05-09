using ESCPOS_NET.Emitters;

namespace EscPosServer
{
    internal static class Program
    {
        public static readonly ICommandEmitter CommandEmitter = new EPSON();

        public static void Main(string[] args)
        {
            var server = new EscPosServer(12345);
            var menu = new CheckMenu(server);
            menu.StartQuestionLoop(true);
        }
    }
}