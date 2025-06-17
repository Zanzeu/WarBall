using System.Collections;
using UnityEngine;
using DG.Tweening;
using System;
using Sirenix.OdinInspector;
using TMPro;
using WarBall.Common;
using WarBall.Events.Game;
using WarBall.UI.Base;
using WarBall.Persistent;
using WarBall.Agent;
using System.Collections.Generic;
using WarBall.Ball.Base;
using WarBall.Enemy.Level;
using WarBall.UI.Pool;
namespace WarBall.Game
{
    public class GameProcess : Singleton<GameProcess>
    {
        private WaitForSeconds _waitOneSecond;
        private BallAttributes _ballAttributes;
        private BallAgent _ballAgent;
        private HashSet<GameObject> _entityPrefabPool;

        [FoldoutGroup("关卡")][LabelText("关卡SO")][SerializeField] private EnemyLevelSO _level;
        [FoldoutGroup("关卡")][LabelText("当前回合")][SerializeField] private int _turn;

        [FoldoutGroup("时间")][LabelText("每回合时间")][SerializeField] private float everyTurnTime;
        [FoldoutGroup("时间")][LabelText("当前回合时间")][SerializeField] private float turnTime;
        [FoldoutGroup("时间")][LabelText("当前回合剩余时间")][SerializeField] private float turnLastTime;
        private TextMeshProUGUI _turnTimerTMP;

        [FoldoutGroup("代币")][LabelText("代币TMP")][SuffixLabel("TMP")][SerializeField] private TextMeshProUGUI _coinRestTMP;
        private int tokenCount = 0;
        private bool _spawnToken;
        private float _curTime;
        private TextMeshProUGUI _coinTMP;

        [FoldoutGroup("分数")][LabelText("TMP变化时间")][SerializeField] private float scoreDuration;
        private TextMeshProUGUI _scoreTMP;
        public int Score { get; private set; }

        [FoldoutGroup("敌人")][LabelText("生成间隔")][SerializeField] private float spawnEnemyInterval;
        private float _spawnTimer;

        [FoldoutGroup("其他")][LabelText("发射点")][SceneObjectsOnly][SuffixLabel("FirePoint")][ValidateInput("CheckFirePointName", "必须为FirePoint！", InfoMessageType.Error)]
        [SerializeField] private GameObject firePoint;

        [FoldoutGroup("测试")][LabelText("关卡测试")][SerializeField] private bool _test;

        public List<string> BackpackGrid { get; set; }

        protected override void Awake()
        {
            base.Awake();
            BackpackGrid = new List<string>();
            _entityPrefabPool = new HashSet<GameObject>();
        }

        private void Start()
        {

#if UNITY_EDITOR
            if (firePoint == null)
            {
                Debug.LogError("GameProcess发射点未设置！！！");
                UnityEditor.EditorApplication.isPlaying = false;
            }
#endif

            _turn = 0;
            turnTime = everyTurnTime;
            spawnEnemyInterval = _level.turn[_turn].interval;
            TimeInit();
            ScoreInit();

            _ballAttributes = FindAnyObjectByType<BallAttributes>();
            _spawnTimer = Time.time;

            if (!_test)
            {
                //_level = Resources.Load<EnemyLevelSO>("");
            }
            else
            {
                _level = Instantiate(_level);
            }
        }

        private void OnEnable()
        {
            GameEvents.Instance.RegisterEvent(EGameEventType.UpdateScore, (Action<int>)OnUpdateScore);
            GameEvents.Instance.RegisterEvent(EGameEventType.UpdateToken, (Func<int, bool>)OnUpdateToken);
            GameEvents.Instance.RegisterEvent(EGameEventType.GetGrid, (Func<string, bool>)OnGetGrid);
            GameEvents.Instance.RegisterEvent(EGameEventType.AbandonGrid, (Action<string>)OnAbandonGrid);
            GameEvents.Instance.RegisterEvent(EGameEventType.UpdateWave, (Func<int>)OnUpdateWave);
            GameEvents.Instance.RegisterEvent(EGameEventType.SetCurTurnTime,(Action<float>)OnSetCurTurnTime);
            GameEvents.Instance.RegisterEvent(EGameEventType.SetBallSpeed, (Action<float>)OnSetBallSpeed);
            GameEvents.Instance.RegisterEvent(EGameEventType.EntitySpawn, (Action<GameObject>)OnEntitySpawn);
            GameEvents.Instance.RegisterEvent(EGameEventType.EntityDeath, (Action<GameObject>)OnEntityDeath);

            GameManager.Instance.RegisterEvent(EGameStatus.Rest, (Action)OnRest);
            GameManager.Instance.RegisterEvent(EGameStatus.Prepare, (Action)OnPrepare);
            GameManager.Instance.RegisterEvent(EGameStatus.Active, (Action)OnActive);
        }

        private void OnDisable()
        {
            GameEvents.Instance.UnregisterEvent(EGameEventType.UpdateScore, (Action<int>)OnUpdateScore);
            GameEvents.Instance.UnregisterEvent(EGameEventType.UpdateToken, (Func<int, bool>)OnUpdateToken);
            GameEvents.Instance.UnregisterEvent(EGameEventType.GetGrid, (Func<string, bool>)OnGetGrid);
            GameEvents.Instance.UnregisterEvent(EGameEventType.AbandonGrid, (Action<string>)OnAbandonGrid);
            GameEvents.Instance.UnregisterEvent(EGameEventType.UpdateWave, (Func<int>)OnUpdateWave);
            GameEvents.Instance.UnregisterEvent(EGameEventType.SetCurTurnTime, (Action<float>)OnSetCurTurnTime);
            GameEvents.Instance.UnregisterEvent(EGameEventType.SetBallSpeed, (Action<float>)OnSetBallSpeed);
            GameEvents.Instance.UnregisterEvent(EGameEventType.EntitySpawn, (Action<GameObject>)OnEntitySpawn);
            GameEvents.Instance.RegisterEvent(EGameEventType.EntityDeath, (Action<GameObject>)OnEntityDeath);

            GameManager.Instance.UnregisterEvent(EGameStatus.Rest, (Action)OnRest);
            GameManager.Instance.UnregisterEvent(EGameStatus.Prepare, (Action)OnPrepare);
            GameManager.Instance.UnregisterEvent(EGameStatus.Active, (Action)OnActive);
        }

        private void Update()
        {
            if (GameManager.Instance.CurrentGameStatus != EGameStatus.Active)
            {
                return;
            }

            if (Time.time - _spawnTimer >= (spawnEnemyInterval / _ballAttributes.Attributes["SPAWN"].ApplyValue))
            {   
                _level.SpawnEnemy(_turn);
                _spawnTimer = Time.time;
            }

            if (!_spawnToken && (Time.time - _curTime >= turnTime / 2f))
            {
                _spawnToken = true;
                GameEvents.Instance.TriggerEvent(EGameEventType.EnQueue, "grid_token", "Grid", "Grid");
            }
        }

        #region ==========回合==========

        private int OnUpdateWave()
        {
            _turn++;

            return _turn;
        }

        private void TimeInit()
        {
            turnLastTime = turnTime;
            _turnTimerTMP = UIManager.GetUI("time").GetComponentInChildren<TextMeshProUGUI>();
            _turnTimerTMP.text = turnLastTime.ToString();
            _waitOneSecond = new WaitForSeconds(1f);
        }

        private IEnumerator TurnTimerCoroutine()
        {   
            while (turnLastTime > 0f)
            {
                yield return _waitOneSecond;

                turnLastTime -= 1f;
                _turnTimerTMP.text = turnLastTime.ToString();
            }

            if (_turn >= _level.turn.Count - 1)
            {
                GameManager.Instance.SwitchGameStatus(EGameStatus.End);
            }
            else
            {
                GameManager.Instance.SwitchGameStatus(EGameStatus.Rest);
            }

        }

        private void OnRest()
        {
            (GameBlackboard.Instance.GetData("ball") as GameObject).GetComponent<BallAgent>().Death();
            foreach (GameObject obj in _entityPrefabPool)
            {
                obj.SetActive(false);
            }
            _entityPrefabPool.Clear();

            turnTime = everyTurnTime;
            spawnEnemyInterval = _level.turn[_turn].interval;
        }

        private void OnPrepare()
        {
            firePoint.SetActive(true);
            turnLastTime = turnTime;
            _turnTimerTMP.text = turnLastTime.ToString();
        }

        private void OnActive()
        {
            _spawnToken = false;
            _curTime = Time.time;
            _spawnTimer = Time.time;
            StopCoroutine(TurnTimerCoroutine());
            StartCoroutine(TurnTimerCoroutine());

            _ballAgent = GameBlackboard.Instance.GetData<GameObject>("ball").GetComponent<BallAgent>();
        }

        private void OnSetCurTurnTime(float val)
        {
            turnLastTime = val;
        }

        #endregion

        #region ==========分数==========

        private void ScoreInit()
        {
            _scoreTMP = UIManager.GetUI<TextMeshProUGUI>("score");
            Score = 0;
            _scoreTMP.text = Score.ToString("D9");

            _coinTMP = UIManager.GetUI("coin").GetComponentInChildren<TextMeshProUGUI>();
        }

        private void OnUpdateScore(int val)
        {
            int curScore = Score;
            Score += (int)(val * _ballAttributes.Attributes["SCORE"].ApplyValue);
            Score = Mathf.Max(0, Score);
            int targetScore = Score;

            _scoreTMP.DOKill();
            DOTween.To(() => curScore, x => _scoreTMP.text = x.ToString("D9"), targetScore, scoreDuration)
                .SetEase(Ease.Linear);
        }

        private bool OnUpdateToken(int val)
        {
            if (tokenCount + val >= 0)
            {
                tokenCount += val;

                if(val > 0)
                {
                    ShowText.Set(new Vector2(763f, 415f), $"+{val}");
                }
                _coinTMP.text = $"×{tokenCount}";
                _coinRestTMP.text = $"×{tokenCount}";

                return true;
            }

            return false;
        }

        #endregion

        #region ==========背包==========
        private bool OnGetGrid(string id)
        {
            if (BackpackGrid.Count >= 25)
            {
                return false;
            }


            BackpackGrid.Add(id);
            BackpackGrid.Sort((a, b) => string.Compare(a, b, StringComparison.Ordinal));
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateBackpack);

            return true;
        }

        private void OnAbandonGrid(string id)
        {
            if (BackpackGrid.Contains(id))
            {
                BackpackGrid.Remove(id);

                            BackpackGrid.Sort((a, b) => string.Compare(a, b, StringComparison.Ordinal));
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateBackpack);
            }
        }

        #endregion

        #region ==========检测===========

        private bool CheckFirePointName(GameObject obj)
        {
            return obj != null && obj.GetComponent<FirePoint>();
        }

        #endregion

        #region ==========其他==========

        public void OnSetBallSpeed(float val)
        {
            _ballAgent.SetSpeed(val);
        }

        private void OnEntitySpawn(GameObject entity)
        {
            _entityPrefabPool.Add(entity);
        }

        private void OnEntityDeath(GameObject entity)
        {
            _entityPrefabPool.Remove(entity);
            entity.SetActive(false);
        }

        #endregion

        #region ==========测试==========

        [FoldoutGroup("测试")]
        [Button("快速测试")]
        public void QuickTest([LabelText("剩余时间")] float lastTime = -1f, [LabelText("生成间隔")] float spawnInterval = 0f)
        {   
            if (lastTime >= 0)
            {
                turnLastTime = lastTime;
            }

            if (spawnInterval > 0)
            {
                spawnEnemyInterval = spawnInterval;
            }
        }

        [FoldoutGroup("测试")][Button("增加分数")]
        public void AddScore([LabelText("目标值")] int val)
        {
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateScore, val);
        }

        [FoldoutGroup("测试")][Button("增加代币")]
        public void AddToken([LabelText("目标值")] int val)
        {
            GameEvents.Instance.TriggerEvent<int, bool>(EGameEventType.UpdateToken, val);
        }

        [FoldoutGroup("测试")][Button("属性操作")]
        public void AddAttributes([LabelText("属性名")][ValueDropdown("GetAttributeName")] string name, [LabelText("操作")] EAttributeOperation temp, [LabelText("目标值")] float val)
        {   
            if (string.IsNullOrEmpty(name))
            {
                return;
            }

            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateAttributes, name, temp, val);
        }

        private List<string> GetAttributeName()
        {
            return new List<string> { "HP", "ATK", "DEF", "CRTR", "DMG", "SCORE", "COOL", "SPAWN" };
        }

        [FoldoutGroup("测试")][Button("切换游戏状态")]
        public void SwitchGameStatus([LabelText("目标状态")] EGameStatus type)
        {
            GameManager.Instance.SwitchGameStatus(type);
        }

        [FoldoutGroup("测试")][Button("获得一个砖块")]
        public void GetGrid([LabelText("砖块ID")] string id)
        {
            BackpackGrid.Add(id);
        }

        #endregion
    }
}
