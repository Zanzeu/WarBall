using DG.Tweening;
using WarBall.UI.Pool;
using UnityEngine;
using WarBall.Common;
using WarBall.Config;
using WarBall.Events.Game;
using WarBall.Game;
using WarBall.Grid;
using WarBall.Grid.Base;
using WarBall.Map;
using WarBall.UI.Base;
using WarBall.UI.Game;
using WarBall.XML;
using WarBall.Persistent;

namespace WarBall.Agent
{
    public class GridAgent : MonoBehaviour, IAgent, ILifeDeath, IPririority
    {
        public GridBase Data { get; private set; }
        public SpriteRenderer _spriteRenderer { get; set; }

        public EPriority Priority => EPriority.High;

        private SpriteRenderer _spriteBcgRenderer;
        private Sequence _scaleSequence;
        private Vector2Int _index;

        private bool _canCollision;

        public void Set(Vector2Int index, GridBase data) => OnSet(index, data);
        public void Collision() => OnCollision();

        private void OnSet(Vector2Int index, GridBase data)
        {   
            if (_spriteBcgRenderer == null)
            {
                _spriteBcgRenderer = GetComponentInChildren<SpriteRenderer>();
                _spriteRenderer = _spriteBcgRenderer.transform.Find("sprite").GetComponent<SpriteRenderer>();
            }

            _canCollision = true;

            _index = index;
            Data = data;

            _spriteBcgRenderer.sprite = GridList.Instance.GetSprite(Data.ID).Bcg;
            _spriteRenderer.sprite = GridList.Instance.GetSprite(Data.ID).Icon;
            Data.OnInit(this);
        }

        public void OnDeath()
        {
            KillTween();
            GameMap.Instance.ReturnMap(_index);
            _index = -Vector2Int.one;
            _spriteRenderer.sprite = null;
            Data = null;

            GameEvents.Instance.TriggerEvent(EGameEventType.GridDeath);
            gameObject.SetActive(false);
        }

        private void OnCollision()
        {   
            if (!_canCollision)
            {
                return;
            }

            _canCollision = false;
            AudioManager.Instance.PlaySound(Sounds.GridCollision);
            TriggerHandle();
            DoScale();
            IncreaseScore();
            DestroyHandle();
        }

        private void KillTween()
        {
            _scaleSequence?.Kill();
        }

        private void TriggerHandle()
        {
            if (Data is IGridCollisionHandle grid1)
            {
                grid1.OnCollisionHandle();
            }

            if (Data is IGridReleasePrefabHandle grid2)
            {
                grid2.OnReleasePrefab();
            }

            if (Data is IGridSetUIHandle grid3)
            {
                string name = grid3.OnSetUI(out int count);

                UIGridState u = GameBlackboard.Instance.GetUIState(name);

                if (u == null)
                {
                    UIGridState ui = UIPoolManager.Release(UIPoolPrefab.EffectItem,UIManager.GetUI("effect")).GetComponent<UIGridState>();
                    ui.Init(GridList.Instance.GetSprite(Data.ID).Icon);
                    ui.SetText(count);
                    GameBlackboard.Instance.AddUIState(name, ui);
                }
                else
                {
                    u.SetText(count);
                }
            }
        }

        private void DestroyHandle()
        {
            if (Data is IGridDestroyHandle grid1)
            {
                grid1.OnDestroyHandle();
                OnDeath();
                return;
            }

            if (Data is IGridDestroyDelayHandle grid2)
            {   
                if (grid2.OnDestroyDelayHandle())
                {
                    OnDeath();
                    return;
                }
            }
        }

        private void DoScale()
        {
            if (_scaleSequence != null && _scaleSequence.IsActive())
            {
                return;
            }

            _scaleSequence = DOTween.Sequence();
            _scaleSequence.Append(_spriteBcgRenderer.transform.DOScale(new Vector2(GlobalGridConfig.VALUE_TWEEN_SCALE, GlobalGridConfig.VALUE_TWEEN_SCALE), GlobalGridConfig.TIME_SCALE).SetEase(Ease.OutQuad));
            _scaleSequence.Append(_spriteBcgRenderer.transform.DOScale(Vector2.one, GlobalGridConfig.TIME_SCALE).SetEase(Ease.InQuad));
            _scaleSequence.OnComplete(() =>
            {
                if (Data is IGridDoScaleCompleteHandle grid1)
                {
                    grid1.OnScaleComplete();
                }

                if (Data is IGridSwitchPositionHandle grid2)
                {
                    grid2.OnSwitchPosition();
                    OnSwitchPosition();
                }

                _canCollision = true;
            });

            _scaleSequence.Play();
        }

        private void IncreaseScore()
        {   
            if(Data.Score > 0)
            {
                PoolManager.Release(PrefabList.Instance.GetPrefab("addScoreTextPrefab"), transform.position).GetComponent<ShowScoreValue>().Set(transform.position.y + 1.5f, Data.Score);
            }
            GameEvents.Instance.TriggerEvent(EGameEventType.UpdateScore, Data.Score);
        }

        private void OnSwitchPosition()
        {
            Vector2Int? pos = GameMap.Instance.GetTile(out Vector2Int index);
            if (pos == null || index.x < 0)
            {
                return;
            }
            GameMap.Instance.ReturnMap(_index);
            _index = index;
            transform.position = (Vector2)pos; 
        }
    }
}
