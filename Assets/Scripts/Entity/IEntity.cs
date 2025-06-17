using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WarBall.Entity
{
    public interface IEntity
    {
        Transform transform { get; }
        T GetComponent<T>();
    }
}
