using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.Messaging.Messages;
using FootballAIGame.AI.FSM.UserClasses.SteeringBehaviors;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class SupportControlling : PlayerState
    {
        private Arrive Arrive { get; set; }

        public SupportControlling(Player player, FootballAI footballAI) : base(player, footballAI)
        {
        }

        public override void Enter()
        {
            Arrive = new Arrive(Player, 1, 1.0, AI.SupportPositionsManager.BestSupportPosition);
            Player.SteeringBehaviorsManager.AddBehavior(Arrive);
            AI.MyTeam.SupportingPlayers.Add(Player);
        }

        public override void Run()
        {
            Arrive.Target = AI.SupportPositionsManager.BestSupportPosition;
            var team = AI.MyTeam;

            // nearest except goalkeeper and controlling
            var nearest = AI.MyTeam.GetNearestPlayerToPosition(Arrive.Target, team.GoalKeeper, team.ControllingPlayer);

            // goalkeeper shouldn't go too far from his home region
            if (Player is GoalKeeper &&
                Vector.DistanceBetween(Arrive.Target, Player.HomeRegion.Center) > Parameters.MaxGoalkeeperSupportingDistance)
            {
                MessageDispatcher.Instance.SendMessage(new SupportControllingMessage(), nearest);
                Player.StateMachine.ChangeState(new Default(Player, AI));
                return;

            }

            // if shot on goal is possible request pass from controlling
            Vector shotVector;
            if (AI.MyTeam.TryGetShotOnGoal(Player, out shotVector) && team.ControllingPlayer != null)
                MessageDispatcher.Instance.SendMessage(new PassToPlayerMessage(Player));

            // someone else is nearer the best position (not goalkeeper)
            if (!(Player is GoalKeeper) && nearest != Player && nearest != team.ControllingPlayer)
            {
                MessageDispatcher.Instance.SendMessage(new SupportControllingMessage(), nearest);
                Player.StateMachine.ChangeState(new Default(Player, AI));
            }

        }

        public override void Exit()
        {
            Player.SteeringBehaviorsManager.RemoveBehavior(Arrive);
            AI.MyTeam.SupportingPlayers.Remove(Player);
        }
    }
}
