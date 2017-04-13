using FootballAIGame.Client.SimulationEntities;

namespace FootballAIGame.Client.AIs.Fsm.Entities
{
    abstract class FieldPlayer : Player
    {
        protected FieldPlayer(FootballPlayer player, FsmAI footballAI) : 
            base(player, footballAI)
        {
        }
    }
}
