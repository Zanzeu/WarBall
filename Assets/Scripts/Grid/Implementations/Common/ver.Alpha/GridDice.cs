using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using WarBall.Agent;
using WarBall.Grid.Base;
using WarBall.Persistent;

namespace WarBall.Grid.Implementations
{
    [Serializable]
    public class GridDice : GridBase, IGridDoScaleCompleteHandle
    {
        [XmlArray("PointSprites")]
        [XmlArrayItem("SpritePath")]
        public List<string> SpritePaths { get; set; } = new List<string>();

        [NonSerialized] private List<Sprite> _pointSprites;
        [NonSerialized] private SpriteRenderer _spriteRenderer;

        private int _point;
        private int _curPoint = -1;

        public override void OnInit(IAgent agent)
        {   
            base.OnInit(agent);
            _spriteRenderer = agent.GetComponent<GridAgent>()._spriteRenderer;
            LoadSpritesFromPaths();
            SwitchDice();
        }

        private void LoadSpritesFromPaths()
        {
            _pointSprites = new List<Sprite>();

            foreach (var path in SpritePaths)
            {
                _pointSprites.Add(ABLoader.Instance.LoadResources<Sprite>("sprite", path));
            }
        }

        private void SwitchDice()
        {
            do
            {
                _point = UnityEngine.Random.Range(1, 7);
            } while (_point == _curPoint);

            _curPoint = _point;
            _spriteRenderer.sprite = _pointSprites[_point - 1];
            Score = 10 * _point;
        }

        public void OnScaleComplete()
        {
            SwitchDice();
        }
    }
}
