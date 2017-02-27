using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.SimulationEntities;

namespace FootballAIGame.AI.FSM.UserClasses.Entities
{
    class Ball : FootballBall
    {
        public Ball(FootballBall ball)
        {
            Position = ball.Position;
            Movement = ball.Movement;
        }

        public void LoadState(GameState gameState)
        {
            Position.X = gameState.Ball.Position.X;
            Position.Y = gameState.Ball.Position.Y;
            Movement.X = gameState.Ball.Movement.X;
            Movement.Y = gameState.Ball.Movement.Y;
        }
    }
}
