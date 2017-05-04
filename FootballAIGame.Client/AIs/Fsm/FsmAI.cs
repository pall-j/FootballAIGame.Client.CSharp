﻿using System;
using System.Threading;
using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.Utilities;
using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client.AIs.Fsm
{
    /// <summary>
    /// The football AI class where the AI behavior is defined.
    /// </summary>
    class FsmAI : IFootballAI
    {
        public static Random Random { get; set; }

        public Ball Ball { get; set; }

        public Team MyTeam { get; set; }

        public Team OpponentTeam { get; set; }

        public SupportPositionsManager SupportPositionsManager { get; set; }

        /// <summary>
        /// Called every time the new match simulation with this AI starts.<para />
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
        /// <param name="gameState">State of the game.</param>
        /// <returns>The <see cref="AIAction" /> for the specified <see cref="GameState" />.</returns>
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