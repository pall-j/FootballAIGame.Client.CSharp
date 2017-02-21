using System;
using FootballAIGame.AI.FSM.SimulationEntities;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses
{
    /// <summary>
    /// The main AI class where the AI behavior is defined. Singleton class.
    /// </summary>
    class Ai : IFootballAi
    {
        /// <summary>
        /// Gets or sets the football players with their parameters set.
        /// Set after GetParameters is called. Used to know players parameters at every <see cref="GetAction"/> call.
        /// </summary>
        private FootballPlayer[] Players { get; set; }

        private static Random Random { get; set; }

        public GameAction CurrentAction { get; set; }

        public Team MyTeam { get; set; }

        public Ball Ball { get; set; }

        private Ai()
        {
            Ball = new Ball(new FootballBall());
            MyTeam = new Team(GetParameters());
        }

        private static Ai _instance;

        public static Ai Instance
        {
            get { return _instance ?? (_instance = new Ai()); }
        }

        /// <summary>
        /// Called every time the new match simulation with this AI starts.<para />
        /// Called before <see cref="GetParameters" />.
        /// </summary>
        public void Initialize()
        {
            if (Random == null)
                Random = new Random();
        }

        /// <summary>
        /// Gets the <see cref="GameAction" /> for the specified <see cref="GameState" />.
        /// </summary>
        /// <param name="gameState">State of the game.</param>
        /// <returns>The <see cref="GameAction" /> for the specified <see cref="GameState" />.</returns>
        public GameAction GetAction(GameState gameState)
        {
            if (gameState.Step == 0)
            {
                MyTeam.IsOnLeft = gameState.FootballPlayers[0].Position.X < 55;
                MyTeam.UpdateHomeRegions();
            }

            // switch
            if (gameState.Step == 750)
            {
                MyTeam.IsOnLeft = !MyTeam.IsOnLeft;
                MyTeam.UpdateHomeRegions();
            }

            // AI entities (wrappers of SimulationEntities) are set accordingly
            LoadState(gameState);

            // new action
            CurrentAction = new GameAction
            {
                Step = gameState.Step,
                PlayerActions = MyTeam.GetActions()
            };

            return CurrentAction;
        }

        /// <summary>
        /// Gets the player parameters. Position and moving vector properties are ignored.
        /// </summary>
        /// <returns>
        /// The array of football players with their parameters set.
        /// </returns>
        public FootballPlayer[] GetParameters()
        {
            if (Players != null) return Players;

            Players = new FootballPlayer[11];
            for (var i = 0; i < 11; i++)
            {
                Players[i] = new FootballPlayer
                {
                    Speed = 0.4f,
                    KickPower = 0.2f,
                    Possession = 0.2f,
                    Precision = 0.2f
                };
            }

            return Players;
        }

        public FootballPlayer GetPlayer(int playerNumber)
        {
            if (playerNumber > 11 || playerNumber < 0)
                throw new IndexOutOfRangeException();
            return Players[playerNumber];
        }

        private void LoadState(GameState state)
        {
            for (int i = 0; i < Players.Length; i++)
            {
                MyTeam.Players[i].Movement = state.FootballPlayers[i].Movement;
                MyTeam.Players[i].Position = state.FootballPlayers[i].Position;
            }

            Ball.Position = state.Ball.Position;
            Ball.Movement = state.Ball.Movement;
        }

    }
}
