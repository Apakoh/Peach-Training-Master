using UnityEngine;

namespace GDD
{    
    public class Turret : ScriptableObject
    {
        public string type;
        public int hp;
        public int cost;
        public int range;
        public Direction direction;
    }
}

