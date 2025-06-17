namespace WarBall.Grid
{   
    /// <summary>
    /// ��ש���������
    /// </summary>
    public interface IGridDoScaleCompleteHandle
    {
        void OnScaleComplete();
    }

    /// <summary>
    /// ������ש��
    /// </summary>
    public interface IGridCollisionHandle
    {
        void OnCollisionHandle();
    }

    /// <summary>
    /// ��ש�鱻�ݻ�
    /// </summary>
    public interface IGridDestroyHandle
    {
        void OnDestroyHandle();
    }

    /// <summary>
    /// ��ש����Ա��ݻ�
    /// </summary>
    public interface IGridDestroyDelayHandle
    {
        bool OnDestroyDelayHandle();
    }

    /// <summary>
    /// ��ש���ͷ�Ԥ����
    /// </summary>
    public interface IGridReleasePrefabHandle
    {
        void OnReleasePrefab();
    }

    /// <summary>
    /// ��ש����ǹ����׼�����ӵ�
    /// </summary>
    public interface IGridGunReplaceHandle
    {
        void OnReplace();
    }

    /// <summary>
    /// ��ש���ƶ�
    /// </summary>
    public interface IGridSwitchPositionHandle
    {
        void OnSwitchPosition();
    }

    /// <summary>
    /// ������UIש��
    /// </summary>
    public interface IGridSetUIHandle
    {
        string OnSetUI(out int count);
    }

    /// <summary>
    /// ��ש��غϽ���
    /// </summary>
    public interface IGridTurnSettlement
    {
        void OnSettlement();
    }
}
