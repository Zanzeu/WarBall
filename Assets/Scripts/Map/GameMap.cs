using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using WarBall.Common;
using WarBall.Enemy.Base;
using WarBall.Grid.Base;

namespace WarBall.Map
{
    public class MapGrid
    {
        public Vector2Int index;
        public Vector2Int pos;

        public MapGrid(Vector2Int index, Vector2Int pos)
        {
            this.index = index;
            this.pos = pos;
        }
    }

    public class GameMap : Singleton<GameMap>
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

        public Vector2Int? GetTile()
        {   
            if (_freeMap.Count <= 0)
            {
                return null;
            }

            int random = Random.Range(0, _freeMap.Count);
            Vector2Int index = _freeMap[random];
            _freeMap.RemoveAt(random);
            return _map[index.x, index.y].pos;
        }

        public Vector2Int? GetTile(out Vector2Int index)
        {
            if (_freeMap.Count <= 0)
            {
                index = -Vector2Int.one;
                return null;
            }

            int random = Random.Range(0, _freeMap.Count);
            index = _freeMap[random];
            _freeMap.RemoveAt(random);
            return _map[index.x, index.y].pos;
        }

        public void ReturnMap(Vector2Int index)
        {   
            if (index.x < 0 || index.y < 0)
            {
                return;
            }

            if (!_freeMap.Contains(index)) 
            {
                _freeMap.Add(index);
            }
        }
    }
}