using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(menuName = "GDD/Tower Defense/Enemy")]
    public class Enemy : ScriptableObject
    {
        public string type;
        public int hp;
        public int dps;
        public float speed;
        public int range;
        public int coins_given;
    }
}
