using System;
using System.IO;
using System.Net;
using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client
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
        public const int StepInterval = 200;

        /// <summary>
        /// The football field height.
        /// </summary>
        public const double FieldHeight = 75;

        /// <summary>
        /// The football field width.
        /// </summary>
        public const double FieldWidth = 110;

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
        private IFootballAI AI { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="GameClient" /> class.
        /// </summary>
        /// <param name="serverAddress">The game server IP address.</param>
        /// <param name="port">The game server port.</param>
        /// <param name="footballAI">The AI.</param>
        public GameClient(IPAddress serverAddress, int port, IFootballAI footballAI)
        {
            ServerAddress = serverAddress;
            ServerPort = port;
            AI = footballAI;
        }

        /// <summary>
        /// Starts the logging in process.
        /// </summary>
        public void Start()
        {
            while (true)
            {
                Console.WriteLine("Enter user name, AI name and access key separated by whitespace.");
                var line = Console.ReadLine();

                if (line == null)
                    break;

                var tokens = line.Trim().Split();
                if (tokens.Length != 3)
                {
                    Console.WriteLine("Invalid format!");
                    continue;
                }

                TryStart(tokens[0], tokens[1], tokens[2]);
            }
        }

        /// <summary>
        /// Starts this instance. Starts logging in process. Uses the specified parameters for logging in.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="accessKey">The access key.</param>
        public void Start(string userName, string accessKey)
        {
            while (true)
            {
                Console.WriteLine("Enter AI name.");
                var line = Console.ReadLine();

                if (line == null)
                    break;

                var tokens = line.Trim().Split();
                if (tokens.Length != 1 || tokens[0].Length == 0)
                {
                    Console.WriteLine("Invalid format!");
                    continue;
                }

                TryStart(userName, tokens[0], accessKey);
            }
        }

        /// <summary>
        /// Tries to start this instance by logging in to server with the specified parameters.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="aiName">The AI name.</param>
        /// <param name="accessKey">The access key.</param>
        public void TryStart(string userName, string aiName, string accessKey)
        {
            string errorMessage;
            ServerConnection connection;

            if ((errorMessage = ServerConnection.TryConnect(ServerAddress, ServerPort,
                    userName, aiName, accessKey, out connection)) == null)
            {
                Console.WriteLine("Connected.");
                Connection = connection;
                StartProcessing();
            }
            else
            {
                Console.WriteLine(errorMessage);
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
        /// Processes the specified command. Calls appropriate <see cref="IFootballAI"/> methods.
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
                    Connection.Send(AI.GetAction(state));
                    //Console.WriteLine($"Step {state.Step} action sent.");
                    //Console.WriteLine("response sent");
                    break;
                case CommandType.GetParameters:
                    AI.Initialize();
                    Connection.SendParameters(AI.GetParameters());
                    break;
            }
        }
    }
}
