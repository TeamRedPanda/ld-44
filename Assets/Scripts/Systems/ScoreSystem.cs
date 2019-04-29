using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreSystem : MonoBehaviour
{
    public event Action<int> OnScoreChanged;

    private int SoulsCollected; // Shadow field :D

    public void Collect(int souls)
    {
        SoulsCollected += souls;
        OnScoreChanged?.Invoke(SoulsCollected);
        StatisticsSystem.Instance.TotalYearsCollected += souls;
    }
}
