using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGameClient.SimulationEntities;

namespace FootballAIGame.AI.FSM.UserClasses.Entities
{
    class Defender : Player
    {
        public Defender(FootballPlayer player) : base(player)
        {
        }

        public override void Update()
        {
        }

        public override void ProcessMessage(Message message)
        {
        }
    }
}
