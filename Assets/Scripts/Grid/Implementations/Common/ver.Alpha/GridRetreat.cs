using System;
using WarBall.Events.Game;
using WarBall.Grid.Base;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridRetreat : GridBase, IGridCollisionHandle
    {
        public void OnCollisionHandle()
        {
            GameEvents.Instance.TriggerEvent(EGameEventType.SetCurTurnTime, 0f);
        }
    }
}
