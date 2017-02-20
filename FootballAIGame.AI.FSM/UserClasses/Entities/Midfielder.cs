using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGameClient.SimulationEntities;

namespace FootballAIGame.AI.FSM.UserClasses.Entities
{
    class Midfielder : Player
    {
        public Midfielder(FootballPlayer player) : base(player)
        {
        }

        public override void Update()
        {
            throw new NotImplementedException();
        }

        public override void OnMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
