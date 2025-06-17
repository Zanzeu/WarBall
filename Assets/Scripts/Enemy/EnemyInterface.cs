namespace WarBall.Enemy
{   
    /// <summary>
    /// �������������
    /// </summary>
    public interface IEnemyDoScaleCompleteHandle
    {
        void OnScaleComplete();
    }

    /// <summary>
    /// �������ƶ�
    /// </summary>
    public interface IEnemySwitchPositionHandle
    {
        void OnSwitchPosition();
    }

    /// <summary>
    /// ����������
    /// </summary>
    public interface IEnemyDeathHandle
    {
        void OnDeath();
    }
}
