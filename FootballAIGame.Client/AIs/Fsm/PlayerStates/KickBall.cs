using FootballAIGame.Client.AIs.Fsm.Entities;
using FootballAIGame.Client.AIs.Fsm.Messaging;
using FootballAIGame.Client.AIs.Fsm.Messaging.Messages;
using FootballAIGame.Client.CustomDataTypes;

namespace FootballAIGame.Client.AIs.Fsm.PlayerStates
{
    /// <summary>
    /// Represents the player's kick ball state. Player in this state tries to find the
    /// shot on goal. If there is not a safe shot and player is in danger, then he
    /// tries to find a safe pass to other team's player. Otherwise
    /// the player will go to <see cref="Dribble"/> state.
    /// </summary>
    /// <seealso cref="FootballAIGame.Client.AIs.Fsm.PlayerStates.PlayerState" />
    class KickBall : PlayerState
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="KickBall"/> class.
        /// </summary>
        /// <param name="player">The player.</param>
        /// <param name="footballAI">The <see cref="FsmAI" /> instance to which this instance belongs.</param>
        public KickBall(Player player, FsmAI footballAI) : base(player, footballAI)
        {
        }

        /// <summary>
        /// Occurs when the entity enters to this state.
        /// </summary>
        public override void Enter()
        {
            AI.MyTeam.ControllingPlayer = Player;
            Run(); // run immediately
        }

        /// <summary>
        /// Occurs every simulation step while the entity is in this state.
        /// </summary>
        public override void Run()
        {
            var team = AI.MyTeam;

            if (team.PassReceiver != null)
            {
                team.PassReceiver = null;
            }

            Vector target;
            if (team.TryGetShotOnGoal(Player, out target))
            {
                Player.KickBall(AI.Ball, target);
                Player.StateMachine.ChangeState(new Default(Player, AI));
                return;
            }
           
            Player passPlayerTarget;
            if (Player.IsInDanger && team.TryGetSafePass(Player, out passPlayerTarget))
            {
                var passTarget = Player.PassBall(AI.Ball, passPlayerTarget);
                MessageDispatcher.Instance.SendMessage(new ReceivePassMessage(passTarget), passPlayerTarget);
                Player.StateMachine.ChangeState(new Default(Player, AI));
                return;
            }
            
            Player.StateMachine.ChangeState(new Dribble(Player, AI));
        }

        /// <summary>
        /// Occurs when the entity leaves this state.
        /// </summary>
        public override void Exit()
        {
            if (AI.MyTeam.ControllingPlayer == Player)
                AI.MyTeam.ControllingPlayer = null;
        }
    }
}
