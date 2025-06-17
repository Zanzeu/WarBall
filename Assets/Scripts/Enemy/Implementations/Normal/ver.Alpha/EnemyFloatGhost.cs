using System;
using WarBall.Enemy.Base;

namespace WarBall.Enemy.Implementations
{
    [Serializable]
    public class EnemyFloatGhost : EnemyBase, IEnemySwitchPositionHandle
    {
        public void OnSwitchPosition() { }
    }
}
