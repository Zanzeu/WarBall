using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WarBall.Agent
{
    public interface IAgent
    {   
        Transform transform { get;}
        T GetComponent<T>();
    }
}
