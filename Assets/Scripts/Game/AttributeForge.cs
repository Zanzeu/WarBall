using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WarBall.Game
{
    public enum EAttributeOperation
    {
        SetBase,
        SetTemp,
        SetMax,
        SetCur,
    }

    public class HPForge : AttributeForge
    {   
        public HPForge(string name, float maxValue, float baseValue) : base(name, baseValue, maxValue) 
        { 
            BaseValue = baseValue;
            CurValue = BaseValue;
        }

        public override float SetMaxValue(float val, out float curVal)
        {
            BaseValue += val;
            
            curVal = SetCurValue(val);
            return BaseValue;
        }

        public override float SetCurValue(float val)
        {
            CurValue += val;
            CurValue = Mathf.Clamp(CurValue, 0f, BaseValue);
            return CurValue;
        }
    }

    public class AttributeForge
    {   
        public string Name { get; private set; }
        public float BaseValue { get; protected set; }
        public float TempValue { get; protected set; }
        public float MaxValue { get; set; }
        public float CurValue { get; set; }
        public bool Percentage { get; private set; }
        public float ApplyValue 
        {
            get => Mathf.Min(BaseValue + TempValue, MaxValue);
            private set => BaseValue = value;
        }

        public AttributeForge(string name,float baseValue,float maxValue ,bool percentage)
        {
            Name = name;
            BaseValue = baseValue;
            MaxValue = maxValue;
            TempValue = 0f;
            CurValue = BaseValue;
            Percentage = percentage;
        }

        public AttributeForge(string name, float maxValue,float baseValue)
        {
            Name = name;
            BaseValue = baseValue;
            MaxValue = maxValue;
            TempValue = 0f;
            CurValue = BaseValue;
            Percentage = false;
        }

        public virtual float SetMaxValue(float val , out float curVal) 
        {
            curVal = CurValue;
            return 0f;
        }

        public virtual float SetCurValue(float val) 
        {
            return CurValue;
        }

        public void SetBaseValue(float val)
        {   
            BaseValue += val;
            BaseValue = Mathf.Clamp(BaseValue, 0, MaxValue);
        }

        public void SetTempValue(float val)
        {
            TempValue += val;
        }

        public void ResetCurValue()
        {
            CurValue = BaseValue;
        }

        public void ResetTempValue()
        {
            TempValue = 0f;
        }
    }
}
