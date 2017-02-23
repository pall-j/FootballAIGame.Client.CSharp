using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.SimulationEntities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.PlayerStates;
using FootballAIGame.AI.FSM.UserClasses.PlayerStates.GlobalStates;

namespace FootballAIGame.AI.FSM.UserClasses.Entities
{
    class GoalKeeper : Player
    {
        public PlayerAction Action { get; set; }

        public GoalKeeper(FootballPlayer player) : base(player)
        {
            Action = new PlayerAction();
            StateMachine = new FiniteStateMachine<Player>(this, new Wait(this), new GoalKeeperGlobalState(this));
        }

        public override PlayerAction GetAction()
        {
            Action.Kick = new Vector(0, 0); // state machine might change it

            StateMachine.Update();

            Action.Movement = Vector.Sum(SteeringBehaviorsManager.CalculateAccelerationVector(), Movement);

            return Action;
        }

        public override void ProcessMessage(Message message)
        {
            StateMachine.ProcessMessage(this, message);
        }

    }
}
