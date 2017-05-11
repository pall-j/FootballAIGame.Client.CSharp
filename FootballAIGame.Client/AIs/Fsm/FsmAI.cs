using System;
using System.Threading;
using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.Utilities;
using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client.AIs.Fsm
{
    /// <summary>
    /// Represents the FSM AI.
    /// </summary>
    class FsmAI : IFootballAI
    {
        /// <summary>
        /// Gets or sets the <see cref="System.Random" /> used for generating random numbers.
        /// </summary>
        /// <value>
        /// The <see cref="Random"/> instance.
        /// </value>
        public static Random Random { get; set; }

        /// <summary>
        /// Gets or sets the ball.
        /// </summary>
        /// <value>
        /// The ball.
        /// </value>
        public Ball Ball { get; set; }

        /// <summary>
        /// Gets or sets my (AI's) team.
        /// </summary>
        /// <value>
        /// My team.
        /// </value>
        public Team MyTeam { get; set; }

        /// <summary>
        /// Gets or sets the opponent's team.
        /// </summary>
        /// <value>
        /// The opponent's team.
        /// </value>
        public Team OpponentTeam { get; set; }

        /// <summary>
        /// Gets or sets the support positions manager.
        /// </summary>
        /// <value>
        /// The support positions manager.
        /// </value>
        public SupportPositionsManager SupportPositionsManager { get; set; }

        /// <summary>
        /// Called every time the new match simulation with the AI starts.<para />
        /// Called before <see cref="GetParameters" />.
        /// </summary>
        public void Initialize()
        {
            if (Random == null)
                Random = new Random();
            SupportPositionsManager = new SupportPositionsManager(this);
        }

        /// <summary>
        /// Gets the <see cref="AIAction" /> for the specified <see cref="GameState" />.
        /// </summary>
        /// <param name="gameState">The state of the game.</param>
        /// <returns>
        /// The <see cref="AIAction" /> for the specified <see cref="GameState" />.
        /// </returns>
        public AIAction GetAction(GameState gameState)
        {
            if (gameState.Step == 0 || MyTeam == null)
            {
                Ball = new Ball(gameState.Ball);
                MyTeam = new Team(GetParameters(), this);
                OpponentTeam = new Team(GetParameters(), this); // expect opponent to have the same parameters
            }

            // AI entities (wrappers of SimulationEntities) are set accordingly
            Ball.LoadState(gameState);
            OpponentTeam.LoadState(gameState, false); // must be loaded before my team!
            MyTeam.LoadState(gameState, true);

            SupportPositionsManager.Update();

            // new action
            var currentAction = new AIAction
            {
                Step = gameState.Step,
                PlayerActions = MyTeam.GetActions()
            };

            return currentAction;
        }

        /// <summary>
        /// Gets the players' parameters.
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
