using System;
using WarBall.Events.Game;
using WarBall.Grid.Base;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridToken : GridBase, IGridCollisionHandle , IGridDestroyHandle
    {
        public void OnCollisionHandle()
        {
            GameEvents.Instance.TriggerEvent<int, bool>(EGameEventType.UpdateToken, 1);
        }

        public void OnDestroyHandle() { }
    }
}
