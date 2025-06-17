namespace WarBall.Grid
{   
    /// <summary>
    /// 当砖块缩放完毕
    /// </summary>
    public interface IGridDoScaleCompleteHandle
    {
        void OnScaleComplete();
    }

    /// <summary>
    /// 当碰到砖块
    /// </summary>
    public interface IGridCollisionHandle
    {
        void OnCollisionHandle();
    }

    /// <summary>
    /// 当砖块被摧毁
    /// </summary>
    public interface IGridDestroyHandle
    {
        void OnDestroyHandle();
    }

    /// <summary>
    /// 当砖块可以被摧毁
    /// </summary>
    public interface IGridDestroyDelayHandle
    {
        bool OnDestroyDelayHandle();
    }

    /// <summary>
    /// 当砖块释放预制体
    /// </summary>
    public interface IGridReleasePrefabHandle
    {
        void OnReleasePrefab();
    }

    /// <summary>
    /// 当砖块是枪并且准备换子弹
    /// </summary>
    public interface IGridGunReplaceHandle
    {
        void OnReplace();
    }

    /// <summary>
    /// 当砖块移动
    /// </summary>
    public interface IGridSwitchPositionHandle
    {
        void OnSwitchPosition();
    }

    /// <summary>
    /// 当碰到UI砖块
    /// </summary>
    public interface IGridSetUIHandle
    {
        string OnSetUI(out int count);
    }

    /// <summary>
    /// 当砖块回合结算
    /// </summary>
    public interface IGridTurnSettlement
    {
        void OnSettlement();
    }
}
