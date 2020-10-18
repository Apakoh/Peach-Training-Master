using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GDD
{
    [CreateAssetMenu(menuName = "GDD/Tower Defense/Level")]
    public class Level : ScriptableObject
    {
        public List<Wave> waves;
    }
}