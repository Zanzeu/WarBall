namespace WarBall.Enemy
{   
    /// <summary>
    /// 当敌人缩放完毕
    /// </summary>
    public interface IEnemyDoScaleCompleteHandle
    {
        void OnScaleComplete();
    }

    /// <summary>
    /// 当敌人移动
    /// </summary>
    public interface IEnemySwitchPositionHandle
    {
        void OnSwitchPosition();
    }

    /// <summary>
    /// 当敌人死亡
    /// </summary>
    public interface IEnemyDeathHandle
    {
        void OnDeath();
    }
}
