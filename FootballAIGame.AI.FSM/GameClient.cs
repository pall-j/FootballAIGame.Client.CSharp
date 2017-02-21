using System;
using System.IO;
using System.Net;
using System.Runtime.Remoting;
using FootballAIGame.AI.FSM.SimulationEntities;

namespace FootballAIGame.AI.FSM
{
    /// <summary>
    /// The main AI game client class.
    /// Provides methods to start logging in to the game server and listening 
    /// for the game server commands.
    /// </summary>
    class GameClient
    {
        /// <summary>
        /// The time in milliseconds of one simulation step.
        /// </summary>
        public const int StepInterval = 200; // [ms]

        /// <summary>
        /// The football field height in meters.
        /// </summary>
        public const double FieldHeight = 75; // [m]

        /// <summary>
        /// The football field width in meters.
        /// </summary>
        public const double FieldWidth = 110; // [m]

        /// <summary>
        /// Gets or sets the connection to the server.
        /// </summary>
        private ServerConnection Connection { get; set; }

        /// <summary>
        /// Gets or sets the game server address.
        /// </summary>
        private IPAddress ServerAddress { get; set; }

        /// <summary>
        /// Gets or sets the game server port.
        /// </summary>
        private int ServerPort { get; set; }

        /// <summary>
        /// Gets or sets AI instance that will process the game server commands.
        /// </summary>
        private IFootballAi Ai { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameClient" /> class.
        /// </summary>
        /// <param name="serverAddress">The game server IP address.</param>
        /// <param name="port">The game server port.</param>
        /// <param name="ai">The AI.</param>
        public GameClient(IPAddress serverAddress, int port, IFootballAi ai)
        {
            this.ServerAddress = serverAddress;
            this.ServerPort = port;
            this.Ai = ai;
        }

        /// <summary>
        /// Starts this instance. Starts logging in process.
        /// </summary>
        public void Start()
        {
            while (true)
            {
                Console.WriteLine("Enter user name and AI name separated by whitespace.");
                var line = Console.ReadLine();

                var tokens = line.Split();
                if (tokens.Length != 2)
                {
                    Console.WriteLine("Invalid format!");
                    continue;
                }

                try
                {
                    Connection = ServerConnection.ConnectAsync(ServerAddress, ServerPort,
                        tokens[0], tokens[1]);
                    Console.WriteLine("Connected.");
                    StartProcessing();
                    break;

                }
                catch (ServerException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        /// <summary>
        /// Starts listening for game commands and processing them.
        /// </summary>
        private void StartProcessing()
        {
            try
            {
                while (true)
                {
                    var command = Connection.ReceiveCommand();
                    Process(command);
                }
            }
            catch (IOException)
            {
                Console.WriteLine("Game Server has stopped responding.");
                Start();
            }
        }

        /// <summary>
        /// Processes the specified command. Calls appropriate <see cref="IFootballAi"/> methods.
        /// </summary>
        /// <param name="command">The game server command.</param>
        private void Process(Command command)
        {
            switch (command.Type)
            {
                case CommandType.GetAction:
                    //Console.WriteLine("Action received");
                    var state = GameState.Parse(command.Data);
                    //Console.WriteLine($"Step {state.Step} received.");
                    Connection.Send(Ai.GetAction(state));
                    //Console.WriteLine($"Step {state.Step} action sent.");
                    //Console.WriteLine("response sent");
                    break;
                case CommandType.GetParameters:
                    Ai.Initialize();
                    Connection.SendParameters(Ai.GetParameters());
                    break;
            }
        }

    }
}
