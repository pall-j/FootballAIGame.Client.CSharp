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
    }
}
