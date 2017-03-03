using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using FootballAIGame.AI.FSM.CustomDataTypes;
using FootballAIGame.AI.FSM.SimulationEntities;
using FootballAIGame.AI.FSM.UserClasses.Entities;
using FootballAIGame.AI.FSM.UserClasses.Messaging;
using FootballAIGame.AI.FSM.UserClasses.Messaging.Messages;

namespace FootballAIGame.AI.FSM.UserClasses.PlayerStates
{
    class KickBall : PlayerState
    {
        public KickBall(Player player) : base(player)
        {
        }

        public override void Enter()
        {
            Ai.Instance.MyTeam.ControllingPlayer = Player;
            Run(); // run immediately
        }

        public override void Run()
        {
            var team = Ai.Instance.MyTeam;

            if (team.PassReceiver != null)
            {
                if (Player is GoalKeeper)
                    Console.WriteLine("State change: Kickball -> PursueBall");
                //Player.StateMachine.ChangeState(new PursueBall(Player));
                team.PassReceiver = null;
                //return;
            }

            Vector target;
            if (team.TryGetShotOnGoal(Player, out target))
            {
                Player.KickBall(Ai.Instance.Ball, target);
               // if (Player is GoalKeeper)
               //     Console.WriteLine("State change: KickBall -> Default (1)");
                Player.StateMachine.ChangeState(new Default(Player));

                return;
            }
           
            Player passTarget;
            if (Player.IsInDanger && team.TryGetSafePass(Player, out passTarget))
            {
                var ball = Ai.Instance.Ball;
                var time = ball.TimeToCoverDistance(Vector.DistanceBetween(ball.Position, passTarget.Position), Player.MaxKickSpeed);

                var nextPos = passTarget.PredictedPositionInTime(time);


                Player.KickBall(Ai.Instance.Ball, nextPos);
                MessageDispatcher.Instance.SendMessage(new ReceivePassMessage(nextPos), passTarget);
               // if (Player is GoalKeeper)
                //    Console.WriteLine("State change: KickBall -> Default (2)");
                Player.StateMachine.ChangeState(new Default(Player));

                return;
            }
            
            //if (Player is GoalKeeper)
           //     Console.WriteLine("State change: KickBall -> Dribble");
            Player.StateMachine.ChangeState(new Dribble(Player));
        }

        public override void Exit()
        {
            if (Ai.Instance.MyTeam.ControllingPlayer == Player)
                Ai.Instance.MyTeam.ControllingPlayer = null;
        }
    }
}
