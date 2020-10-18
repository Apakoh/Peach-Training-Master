using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AI_TP2
{
    public abstract class AgentBehavior : ScriptableObject
    {
        public abstract Vector3 NextMove(Agent agent, GameMasterTP2 gm);
    }
}