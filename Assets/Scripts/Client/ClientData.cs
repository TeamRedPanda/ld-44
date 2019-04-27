using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Client
{
    class ClientData : MonoBehaviour
    {
        public int Age = 20;
        public int Happiness = 100;

        public bool IsCloseToDying { get; internal set; }
        public bool IsHappy {
            get {
                return Happiness > 0;
            }
        }
        public bool WantToBuy { get; internal set; }
    }
}
