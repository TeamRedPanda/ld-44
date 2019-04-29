using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatisticsSystem : MonoBehaviour
{
    public static StatisticsSystem Instance {
        get {
            if (m_Instance == null) {
                m_Instance = FindObjectOfType<StatisticsSystem>();
            }

            return m_Instance;
        }
    }

    private static StatisticsSystem m_Instance = null;

    // Start is called before the first frame update
    void Awake()
    {
        if (m_Instance != null)
            Debug.LogError("Cannot have more than 1 SoundEffectControllers in scene.");

        m_Instance = this;
    }

    public int TotalYearsCollected = 0;
    public int TotalDeaths = 0;
    private const int c_RequiredYearsCollected = 1000;

    public bool Passed {
        get {
            return TotalYearsCollected >= c_RequiredYearsCollected;
        }
    }
}
