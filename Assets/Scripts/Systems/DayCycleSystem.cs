using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DayCycleSystem : MonoBehaviour
{
    private const int c_WaitBeforeFadeOut = 2000;
    public DayCycleCardView CardView;
    public ClientSpawnSystem ClientSpawnSystem;
    public ProductDisplayController ProductDisplayController;

    private int m_CurrentDay = 1;
    private const int c_FinalDay = 4;

    private float m_CurrentDayTime = 0f;
    private const float c_DayTimeDuration = 10f;

    // Start is called before the first frame update
    void Start()
    {
        CardView.gameObject.SetActive(true);
        Task.Delay(c_WaitBeforeFadeOut).ContinueWith(t =>
        {
            CardView.FadeOut(null);
        }
        );
    }

    // Update is called once per frame
    void Update()
    {
        m_CurrentDayTime += Time.deltaTime;

        if (m_CurrentDayTime >= c_DayTimeDuration) {
            m_CurrentDay++;
            StartDay(m_CurrentDay);
            m_CurrentDayTime -= c_DayTimeDuration;
            Debug.LogWarning("Starting new day!");
        }
    }

    private void StartDay(int day)
    {
        string dayText = GetDayText(day);
        CardView.SetCardText(dayText);
        CardView.FadeIn(ResetShopState);
    }

    private void ResetShopState()
    {
        ClientSpawnSystem.RemoveAllClients();
        ProductDisplayController.ResetStoreProducts();
        Task.Delay(c_WaitBeforeFadeOut).ContinueWith(t => CardView.FadeOut(null));
    }

    private string GetDayText(int day)
    {
        if (day == c_FinalDay) {
            return "Final Day";
        } else {
            return $"Day {day.ToString()}";
        }
    }
}
