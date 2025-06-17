using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WarBall.Common;
using WarBall.Enemy.Base;
using WarBall.Grid.Base;
using WarBall.Map;

namespace WarBall.Test
{
    public class GameMapTest : MonoBehaviour
    {
        [SerializeField] private Vector2Int startPos = new Vector2Int(-17, 13);
        [SerializeField] private Vector2Int endPos = new Vector2Int(15, -3);
        [SerializeField] private Vector2Int stepSize = new Vector2Int(3, 3);

        private MapGrid[,] _map;
        private List<Vector2Int> _freeMap;

        private void Start()
        {
            InitMap();
        }

        private void InitMap()
        {
            _freeMap = new List<Vector2Int>();
            int mapWidth = Mathf.FloorToInt((endPos.x - startPos.x) / (float)stepSize.x) + 1;
            int mapHeight = Mathf.FloorToInt((startPos.y - endPos.y) / (float)stepSize.y) + 1;
            _map = new MapGrid[mapWidth, mapHeight];

            for (int yIndex = 0; yIndex < mapHeight; yIndex++)
            {
                int yCoord = startPos.y - yIndex * stepSize.y;
                for (int xIndex = 0; xIndex < mapWidth; xIndex++)
                {
                    int xCoord = startPos.x + xIndex * stepSize.x;
                    var grid = new MapGrid(new Vector2Int(xIndex, yIndex), new Vector2Int(xCoord, yCoord));
                    _map[xIndex, yIndex] = grid;
                    _freeMap.Add(grid.index);
                }
            }
        }

        public Vector2Int GetTile()
        {
            int random = Random.Range(0, _freeMap.Count);
            Vector2Int index = _freeMap[random];
            _freeMap.Remove(index);

            return _map[index.x, index.y].pos;
        }

        public void ReturnMap(Vector2Int index)
        {
            if (!_freeMap.Contains(index))
            {
                _freeMap.Add(index);
            }
        }
    }
}