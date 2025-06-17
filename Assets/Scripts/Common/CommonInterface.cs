using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using WarBall.Config;

namespace WarBall.Common
{   
    public interface IPririority
    {
        EPriority Priority { get; }
    }

    public interface ILifeDeath
    {
        void OnDeath();
    }
}
