using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using WarBall.Events.Game;

namespace WarBall.Enemy.Level
{
    [CreateAssetMenu(menuName = "�ؿ�/����", fileName = "Level_")]
    public class EnemyLevelSO : ScriptableObject
    {
        [Serializable]
        public class Turn
        {
            [LabelText("��ǰ�غ�ÿ�����ɼ��")]
            public float interval;

            [HideInInspector] public int index;
            private int _wave = 0;

            [Space(10)]
            [LabelText("��ǰ�غϵ���")]public List<EnemySetting> normalId;

            public string GetTurn()
            {
                return $"��{index}�غ�";
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
            [ValueDropdown("EnemyIds")][LabelText("����ID")] public string id;
            [LabelText("��������")] public int count;

            public static List<string> EnemyIds = new List<string>
            {
                "enemy_vilefluid",
                "enemy_devileye",
                "enemy_smallgoblin",
                "enemy_floatghost",
                "enemy_slime"
            };
        }

        [LabelText("����")]
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