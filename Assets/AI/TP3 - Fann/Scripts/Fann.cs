using UnityEngine;
using FANNCSharp.Double;
using System.Collections.Generic;

namespace AI_FANN
{
    public class Fann : MonoBehaviour
    {
        public void FANN()
        {
            List<uint> layers = new List<uint>();
            NeuralNet network = new NeuralNet(FANNCSharp.NetworkType.LAYER, layers);
        }        
    }
}