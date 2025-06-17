using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WarBall.Agent;
using WarBall.Game;

namespace WarBall.Enemy.Base
{
    public class EnemyAttributes
    {
        public Dictionary<string, AttributeForge> Attributes { get; private set; }

        public EnemyAttributes(int hP, int aTK, int dEF , float sCORE, float cOOL, float lIFE)
        {
            Attributes = new Dictionary<string, AttributeForge>();

            Attributes.Add("HP", new HPForge("HP",9999f,hP));
            Attributes.Add("ATK", new AttributeForge("ATK", 9999f, aTK));
            Attributes.Add("DEF", new AttributeForge("DEF", 9999f, dEF));
            Attributes.Add("SCORE", new AttributeForge("SCORE", 9999f, sCORE));
            Attributes.Add("COOL", new AttributeForge("COOL", 9999f, cOOL));
            Attributes.Add("LIFE", new AttributeForge("LIFE", 9999f, lIFE));
        }

        public AttributeForge SetAttributes(string attributes, EAttributeOperation target, float val)
        {
            if (target == EAttributeOperation.SetBase)
            {
                Attributes[attributes].SetBaseValue(val);
            }
            else if (target == EAttributeOperation.SetTemp)
            {
                Attributes[attributes].SetTempValue(val);
            }
            else if (target == EAttributeOperation.SetMax)
            {
                Attributes[attributes].SetMaxValue(val, out float curVal);
            }
            else if (target == EAttributeOperation.SetCur)
            {
                Attributes[attributes].SetCurValue(val);
            }

            return Attributes[attributes];
        }
    }
}
