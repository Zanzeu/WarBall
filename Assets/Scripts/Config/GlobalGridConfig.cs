using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WarBall.Config
{
    public struct GridSprite
    {
        public Sprite Icon;
        public Sprite Bcg;
    }

    public enum EPriority
    {   
        Low = 1,
        Base = 2, //enemy
        Middle = 4,
        High = 8, //grid
        Top = 16, //boss
    }

    public static class GlobalGridConfig
    {
        public static readonly float TIME_SCALE = 0.1f;
        public static readonly float VALUE_TWEEN_SCALE = 1.5f;

        public static readonly float MIN_SPEED = 5f;
        public static readonly float MAX_SPEED = 100f;
    }
}
