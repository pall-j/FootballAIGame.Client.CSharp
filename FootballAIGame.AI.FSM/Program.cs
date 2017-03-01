using System.Net;
using FootballAIGame.AI.FSM.UserClasses;

namespace FootballAIGame.AI.FSM
{
    /// <summary>
    /// The class that contains the entry point of the application.
    /// </summary>
    class Program
    {
        /// <summary>
        /// The entry point of the application. Creates and starts the <see cref="GameClient"/>.
        /// </summary>
        static void Main()
        {
            var client = new GameClient(IPAddress.Parse("13.69.197.216"), 50030, Ai.Instance);
            //var client = new GameClient(IPAddress.Parse("127.0.0.1"), 50030, Ai.Instance);
            client.Start();
        }

    }
}
