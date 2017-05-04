using System.Net;
using FootballAIGame.Client.AIs.Basic;
using FootballAIGame.Client.AIs.Fsm;

namespace FootballAIGame.Client
{
    /// <summary>
    /// The class that contains the entry point of the application.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// The entry point of the application. Creates and starts the <see cref="GameClient"/>.
        /// </summary>
        private static void Main()
        {
            // ------------------------ FSM AI ----
            var client = new GameClient(IPAddress.Parse("13.69.197.216"), 50030, new FsmAI());
            //var client = new GameClient(IPAddress.Loopback, 50030, new FsmAI());

            // ----------------------- BASIC (RANDOM) AI ----
            //var client = new GameClient(IPAddress.Parse("13.69.197.216"), 50030, new BasicAI());
            //var client = new GameClient(IPAddress.Loopback, 50030, new BasicAI());


            client.Start();
            //client.Start("UserName", null); // fixed user with his access key (suitable for connecting to local simulators)
        }

    }
}
