using UnityEngine;

namespace WarBall.Util
{
    public static class GetRandomAttributes
    {
        private static string[] _attributes = { "ATK", "DEF", "CRTR", "DMG", "SCORE", "COOL", "SPAWN" };

        public static string GetName()
        {
            return _attributes[Random.Range(0, _attributes.Length)];
        }

        public static float GetValue(string attribute,float MinVal,float MaxVal)
        {
            if (attribute == "ATK" || attribute == "DEF")
            {
                return (int)Random.Range(MinVal, MaxVal);
            }
            else if (attribute == "COOL")
            {
                return -Random.Range(MinVal, MaxVal) * 0.1f;
            }

            return Random.Range(MinVal, MaxVal) * 0.1f;
        }

        public static float GetValue(string attribute, float val)
        {
            if (attribute == "ATK" || attribute == "DEF")
            {
                return (int)val;
            }
            else if (attribute == "COOL")
            {
                return -val * 0.1f;
            }

            return val * 0.1f;
        }
    }
}
