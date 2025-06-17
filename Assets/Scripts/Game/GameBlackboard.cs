using System.Collections;
using System.Collections.Generic;
using WarBall.UI.Pool;
using UnityEngine;
using WarBall.UI.Game;

namespace WarBall.Game
{
    public class GameBlackboard
    {   
        public class Node
        {
            public object data;
            public float life;
            public float timer;

            public Node(object data, float life)
            {
                this.data = data;
                this.life = life;
                timer = Time.unscaledTime;
            }
        }

        public class GridStateNode
        {
            public UIGridState state;
            public UIPool pool;
            public int count;
            public bool settlement;

            public GridStateNode(UIPool pool,UIGridState state)
            {
                this.pool = pool;
                this.state = state;
                count = 0;
                settlement = false;
            }
        }

        public static GameBlackboard Instance = new GameBlackboard();
        private Dictionary<string, Node> _blackboard;
        private Dictionary<string, GridStateNode> _uiGridState;

        public void Init()
        {
            _blackboard = new Dictionary<string, Node>();
            _uiGridState = new Dictionary<string, GridStateNode>();
        }

        #region »ù´¡ºÚ°å

        public object AddData(string name, object data,float life)
        {
            if (!_blackboard.ContainsKey(name))
            {
                _blackboard.Add(name, new Node(data, life));

                return data;
            }

            return null;
        }

        public bool RemoveData(string name)
        {
            if (_blackboard.TryGetValue(name , out Node data))
            {
                _blackboard.Remove(name);
                return true;
            }

            return false;
        }

        public object SetData(string name, object data,float life)
        {
            if (_blackboard.ContainsKey(name))
            {
                _blackboard[name] = new Node(data, life);

                return data;
            }

            return AddData(name, data, life);
        }

        public object GetData(string name)
        {
            if (_blackboard.TryGetValue(name, out Node data))
            {   
                if (Time.time - data.timer <= data.life)
                {
                    return data.data;
                }
                else
                {
                    RemoveData(name);
                    return null;
                }
            }

            return null;
        }

        public T GetData<T>(string name) where T : class
        {
            if (_blackboard.TryGetValue(name, out Node data))
            {
                if (Time.time - data.timer <= data.life)
                {
                    return data.data as T;
                }
                else
                {
                    RemoveData(name);
                    return null;
                }
            }

            return null;
        }

        #endregion

        #region ×©¿é×´Ì¬

        public bool CheckSettlement(string name)
        {
            if (_uiGridState.TryGetValue(name, out GridStateNode val))
            {   
                return val.settlement;
            }

            return true;
        }

        public void Settlement(string name,bool state)
        {
            if (_uiGridState.TryGetValue(name, out GridStateNode val))
            {
                val.settlement = state;
            }
        }

        public bool AddUIState(string name, UIGridState uI)
        {
            if (!_uiGridState.ContainsKey(name))
            {   
                _uiGridState.Add(name, new GridStateNode(uI.GetComponent<UIPool>(), uI));
                _uiGridState[name].count = 1;
                return true;
            }

            return false;
        }

        public UIGridState GetUIState(string name)
        {
            if (_uiGridState.TryGetValue(name, out GridStateNode res))
            {
                return res.state;
            }

            return null;
        }

        public int SetCount(string name,int val)
        {
            if (_uiGridState.TryGetValue(name, out GridStateNode ui))
            {
                ui.count += val;
                return ui.count;
            }

            if (val == 0)
            {
                return 0;
            }

            return 1;
        }

        public void CloseUI(string name)
        {
            if (_uiGridState.TryGetValue(name, out GridStateNode ui))
            {
                ui.pool.Enterpool();
                _uiGridState.Remove(name);
            }
        }

        #endregion
    }
}
