using System;
using System.Collections.Generic;
using UnityEngine;
using WarBall.Agent;
using WarBall.Common;
using WarBall.Config;
using WarBall.Events.Game;
using WarBall.Map;
using WarBall.Persistent;
using WarBall.XML;
using WarBall.Grid;

namespace WarBall.Game
{
    public class GridSpawnController : MonoBehaviour
    {
        [SerializeField] private GameObject gridPrefab;
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private int batchSize = 5;

        public struct WaitGrid
        {
            public string tag;
            public string id;
            public string sort;
            public EPriority priority;
        }

        public List<WaitGrid> WaitSpawn { get; set; }
        private List<GridAgent> _tempGrid;
        private List<EnemyAgent> _tempEnemy;

        private void Awake()
        {
            WaitSpawn = new List<WaitGrid>();
            _tempGrid = new List<GridAgent>();
            _tempEnemy = new List<EnemyAgent>();
        }

        private void OnEnable()
        {

            GameEvents.Instance.RegisterEvent(EGameEventType.EnQueue, (Action<string, string,string>)AddWaitSpawn);
            GameManager.Instance.RegisterEvent(EGameStatus.Rest, (Action)OnClear);
            GameManager.Instance.RegisterEvent(EGameStatus.Prepare, (Action)OnPrepare);
        }

        private void OnDisable()
        {
            GameManager.Instance.UnregisterEvent(EGameStatus.Rest, (Action)OnClear);
            GameEvents.Instance.UnregisterEvent(EGameEventType.EnQueue, (Action<string, string,string>)AddWaitSpawn);
            GameManager.Instance.UnregisterEvent(EGameStatus.Prepare, (Action)OnPrepare);
        }

        private void Update()
        {
            if ((GameManager.Instance.CurrentGameStatus == EGameStatus.Prepare || GameManager.Instance.CurrentGameStatus == EGameStatus.Active) && WaitSpawn.Count > 0)
            {
                OnSpawn();
            }
        }

        private void OnSpawn()
        {
            int processedCount = 0;
            while (WaitSpawn.Count > 0 && processedCount < batchSize)
            {
                Vector2Int? pos = GameMap.Instance.GetTile(out Vector2Int index);
                if (pos == null || index.x < 0)
                {
                    OnFailedSpawn();
                    processedCount++;
                    continue;
                }

                ReleasePool((Vector2)pos, index);
                WaitSpawn.RemoveAt(0);
                processedCount++;
                GameEvents.Instance.TriggerEvent(EGameEventType.Spawn);
            }
        }

        private void ReleasePool(Vector2 position, Vector2Int index)
        {
            if (WaitSpawn[0].sort.Equals("Enemy"))
            {
                EnemyAgent agent = PoolManager.Release(enemyPrefab, position,
                    Quaternion.Euler(0, 0, UnityEngine.Random.Range(-360f, 360f)))
                    .GetComponent<EnemyAgent>();
                agent.Set(index, EnemyList.Instance.GetData(WaitSpawn[0].id));
                _tempEnemy.Add(agent);
            }
            else
            {
                GridAgent agent = PoolManager.Release(gridPrefab, position,
                    Quaternion.Euler(0, 0, UnityEngine.Random.Range(-360f, 360f)))
                    .GetComponent<GridAgent>();
                agent.Set(index, GridList.Instance.GetData(WaitSpawn[0].id));
                _tempGrid.Add(agent);
            }
        }

        private void OnFailedSpawn()
        {
            WaitGrid failed = WaitSpawn[0];
            WaitSpawn.RemoveAt(0);
            WaitSpawn.Add(failed);
        }

        private void AddWaitSpawn(string id, string tag,string sort)
        {
            EPriority priority;

            if (tag.Equals("Enemy"))
            {
                priority = EPriority.Base;
            }
            else if(tag.Equals("Grid"))
            {
                priority = EPriority.Middle;
            }
            else if(tag.Equals("High"))
            {
                priority = EPriority.High;
            }
            else if(tag.Equals("Boss") || tag.Equals("Elite"))
            {
                priority = EPriority.Top;
            }
            else
            {
                priority = EPriority.Low;
            }
            WaitSpawn.Add(new WaitGrid { tag = tag , id = id, priority = priority ,sort = sort});
            WaitSpawn.Sort((a, b) => b.priority.CompareTo(a.priority));
        }

        private void OnClear()
        {
            WaitSpawn.Clear();

            foreach (GridAgent grid in _tempGrid)
            {   
                if (grid.Data is IGridTurnSettlement g)
                {
                    g.OnSettlement();
                }

                if (grid.gameObject.activeSelf)
                {
                    grid.OnDeath();
                }
            }

            foreach (EnemyAgent enemy in _tempEnemy)
            {   
                if (enemy.gameObject.activeSelf)
                {
                    enemy.OnDeath();
                }
            }

            _tempGrid.Clear();
            _tempEnemy.Clear();
        }
        private void OnPrepare()
        {
            foreach (string id in GameProcess.Instance.BackpackGrid)
            {
                AddWaitSpawn(id, "Grid","Grid");
            }
        }
    }
}
