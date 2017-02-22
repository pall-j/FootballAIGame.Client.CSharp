using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates
{
    class PlayerGlobalState<TPlayer> : State<TPlayer> where TPlayer : Player
    {
        private PlayerGlobalState() { }

        private static PlayerGlobalState<TPlayer> _instance;

        public static PlayerGlobalState<TPlayer> Instance
        {
            get { return _instance ?? (_instance = new PlayerGlobalState<TPlayer>()); }
        }

        public override void Run(TPlayer entity)
        {
        }

        public bool ProcessMessage(TPlayer entity, ReturnToHome message)
        {
            if (entity is Forward)
                (entity as Forward).ChangeState(MoveToHomeRegion<Forward>.Instance);

            if (entity is Midfielder)
                (entity as Midfielder).ChangeState(MoveToHomeRegion<Midfielder>.Instance);

            if (entity is Defender)
                (entity as Defender).ChangeState(MoveToHomeRegion<Defender>.Instance);

            if (entity is GoalKeeper)
                (entity as GoalKeeper).ChangeState(MoveToHomeRegion<GoalKeeper>.Instance);

            return true;
        }
    }
}
