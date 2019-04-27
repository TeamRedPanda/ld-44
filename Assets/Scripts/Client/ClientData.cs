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

        public bool IsDead {
            get {
                return Age >= c_DeathAge;
            }
        }

        public bool IsHappy {
            get {
                return Happiness > 0;
            }
        }
        public bool WantToBuy {
            get {
                // 50% chance to want to buy.
                return UnityEngine.Random.Range(0, 2) == 0;
            }
        }

        // How long does it take in seconds to age 1 year.
        private const float c_SecondsPerYear = 5f;
        private const int c_DeathAge = 100;

        private float m_CurrentYear = 0f;

        void Update()
        {
            m_CurrentYear += Time.deltaTime;

            if (m_CurrentYear >= c_SecondsPerYear) {
                Age++;
                m_CurrentYear -= c_SecondsPerYear;
            }
        }

        internal void IncreaseHappiness(int value)
        {
            // Older people get angry faster.
            float factor = 1 + (float)Age / (float)c_DeathAge;
            Happiness += Mathf.RoundToInt(value * factor);
        }
    }
}
