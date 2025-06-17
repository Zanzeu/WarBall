using System;
using WarBall.Events.Game;
using WarBall.Grid.Base;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridSuppressor : GridBase, IGridCollisionHandle
    {
        public void OnCollisionHandle()
        {
            GameEvents.Instance.TriggerEvent(EGameEventType.SetBallSpeed, 20f);
        }
    }
}
