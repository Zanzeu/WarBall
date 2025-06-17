using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using WarBall.Agent;
using WarBall.Ball.Base;
using WarBall.Common;
using WarBall.Persistent;
using WarBall.XML;

namespace WarBall.Game
{
    public class FirePoint : MonoBehaviour
    {
        private bool _input;
        private int _layerMask;
        private Vector2 _direction;
        private Vector2 _mousePosition;
        private LineRenderer _line;
        private Vector2 _boundary;

        [LabelText("球预制体")] [SuffixLabel("Prefab")][SerializeField] private GameObject ball;
        [LabelText("错误预制体")][SuffixLabel("Prefab")][SerializeField] private GameObject error;

        private bool _init = false;

        private void Start()
        {
            _line = GetComponentInChildren<LineRenderer>();
            _layerMask = LayerMask.GetMask("Grid", "Obstacle");
            _input = false;
            _line.enabled = false;
            _init = true;
        }

        private void OnEnable()
        {   
            if (_init)
            {
                _input = false;
                _line.SetPosition(0, Vector2.zero);
                _line.SetPosition(1, Vector2.zero);
                _line.enabled = false;
            }
            else
            {
                _init = true;
            }
        }

        private void Update()
        {
            _mousePosition = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
            _boundary = _mousePosition;
            _boundary.x = Mathf.Clamp(_boundary.x, -19.3f, 9f);
            _boundary.y = Mathf.Clamp(_boundary.y, -3.5f, 14f);
            if (!_input)
            {
                transform.position = _boundary;

                if (Input.GetMouseButtonDown(0))
                {
                    if (IsBoundary(_mousePosition))
                    {
                        RaycastHit2D hit = Physics2D.CircleCast(transform.position, 0.25f, Vector2.down, 0.1f, _layerMask);
                        if (!hit)
                        {
                            _input = true;
                            _line.enabled = true;
                            _line.SetPosition(0, transform.position);
                        }
                        else
                        {
                            PoolManager.Release(error, new Vector2(transform.position.x + Random.Range(-0.2f, 0.2f), transform.position.y + Random.Range(-0.2f, 0.2f)));
                        }
                    }
                }
            }
            else
            {   
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject obj = PoolManager.Release(this.ball);
                    BallAgent ball = obj.GetComponent<BallAgent>();
                    ball.transform.position = transform.position;
                    ball.Set(BallList.Instance.GetData(GameManager.Instance.BallID), _direction);
                    GameBlackboard.Instance.SetData("ball", obj, 9999f);
                    GameManager.Instance.SwitchGameStatus(EGameStatus.Active);
                    gameObject.SetActive(false);
                }

                if (Input.GetMouseButtonDown(1))
                {
                    _input = false;
                    _line.SetPosition(0, Vector2.zero);
                    _line.SetPosition(1, Vector2.zero);
                    _line.enabled = false;
                }

                _direction = (_mousePosition - (Vector2)transform.position).normalized;

                RaycastHit2D hit = Physics2D.Raycast(transform.position,_direction,Mathf.Infinity,_layerMask);

                if (hit)
                {
                    _line.SetPosition(1, hit.point);
                }
            }
        }

        private bool IsBoundary(Vector2 pos)
        {
            return pos.x >= -19.3f && pos.x <= 9f&& pos.y >= -3.5f && pos.y <= 14f;
        }
    }
}