using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using WarBall.Events.Game;

namespace WarBall.Enemy.Level
{
    [CreateAssetMenu(menuName = "关卡/设置", fileName = "Level_")]
    public class EnemyLevelSO : ScriptableObject
    {
        [Serializable]
        public class Turn
        {
            [LabelText("当前回合每次生成间隔")]
            public float interval;

            [HideInInspector] public int index;
            private int _wave = 0;

            [Space(10)]
            [LabelText("当前回合敌人")]public List<EnemySetting> normalId;

            public string GetTurn()
            {
                return $"第{index}回合";
            }

            public void Spawn()
            {   
                if (_wave >= normalId.Count)
                {
                    return;
                }

                for (int i = 0; i < normalId[_wave].count; i++)
                {
                    GameEvents.Instance.TriggerEvent(EGameEventType.EnQueue, normalId[_wave].id, "Enemy", "Enemy");
                }

                _wave++;
            }
        }

        [Serializable]
        public struct EnemySetting
        {
            [ValueDropdown("EnemyIds")][LabelText("敌人ID")] public string id;
            [LabelText("生成数量")] public int count;

            public static List<string> EnemyIds = new List<string>
            {
                "enemy_vilefluid",
                "enemy_devileye",
                "enemy_smallgoblin",
                "enemy_floatghost",
                "enemy_slime"
            };
        }

        [LabelText("设置")]
        [ListDrawerSettings(ShowIndexLabels = false, ListElementLabelName = "GetTurn", OnBeginListElementGUI = "BeginDrawLevel")]
        public List<Turn> turn = new List<Turn>();

        private void BeginDrawLevel(int index)
        {
            var level = turn[index];
            level.index = index + 1; 
            turn[index] = level;
        }

        public void SpawnEnemy(int index)
        {
            turn[index].Spawn();
        }
    }
}