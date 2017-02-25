using System;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.SimulationEntities;
using FootballAIGame.AI.FSM.UserClasses.Entities;

namespace FootballAIGame.AI.FSM.UserClasses
{
    /// <summary>
    /// The main AI class where the AI behavior is defined. Singleton class.
    /// </summary>
    class Ai : IFootballAi
    {
        public static Random Random { get; set; }

        public GameState CurrentState { get; set; }

        public Team MyTeam { get; set; }

        public Team OpponentTeam { get; set; }

        private static Ai _instance;

        public static Ai Instance
        {
            get { return _instance ?? (_instance = new Ai()); }
        }

        private Ai()
        {
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
            if (gameState.Step == 0 || MyTeam == null)
            {
                MyTeam = new Team(GetParameters());
                OpponentTeam = new Team(GetParameters()); // ~ expect him to have same parameters
            }

            // AI entities (wrappers of SimulationEntities) are set accordingly
            CurrentState = gameState;
            MyTeam.LoadState(gameState, true);
            OpponentTeam.LoadState(gameState, false);

            // new action
            var currentAction = new GameAction
            {
                Step = gameState.Step,
                PlayerActions = MyTeam.GetActions()
            };

            return currentAction;
        }

        /// <summary>
        /// Gets the player parameters. Position and moving vector properties are ignored.
        /// </summary>
        /// <returns>
        /// The array of football players with their parameters set.
        /// </returns>
        public FootballPlayer[] GetParameters()
        {
            var players = new FootballPlayer[11];

            for (var i = 0; i < 11; i++)
            {
                players[i] = new FootballPlayer(i)
                {
                    Speed = 0.4f,
                    KickPower = 0.2f,
                    Possession = 0.2f,
                    Precision = 0.2f
                };
            }

            return players;
        }
    }
}
