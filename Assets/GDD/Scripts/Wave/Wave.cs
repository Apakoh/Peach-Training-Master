using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(menuName = "GDD/Tower Defense/Wave")]
    public class Wave : ScriptableObject
    {
        public int nb_parasite;
        public int nb_infected;
        public int nb_coloss;

        public int reward;
    }
}
