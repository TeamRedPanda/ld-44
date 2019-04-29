using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class DayCycleSystem : MonoBehaviour
{
    private const float c_WaitBeforeFadeOut = 1f;
    public DayCycleCardView CardView;
    public ClientSpawnSystem ClientSpawnSystem;
    public ProductDisplayController ProductDisplayController;

    private int m_CurrentDay = 1;
    private const int c_FinalDay = 4;

    private float m_CurrentDayTime = 0f;
    private const float c_DayTimeDuration = 45f;

    // Start is called before the first frame update
    void Start()
    {
        CardView.gameObject.SetActive(true);
        CardView.FadeOut(c_WaitBeforeFadeOut, ResumeClientSpawn);
    }

    private void ResumeClientSpawn()
    {
        ClientSpawnSystem.ResumeSpawn();
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

    IEnumerator ExecuteAfterTime(float time, Action task)
    {
        yield return new WaitForSeconds(time);
        task();
    }

    private void StartDay(int day)
    {
        string dayText = GetDayText(day);
        CardView.SetCardText(dayText);
        CardView.FadeIn(callback:ResetShopState);
    }

    private void ResetShopState()
    {
        ClientSpawnSystem.RemoveAllClients();
        ClientSpawnSystem.PauseSpawn();
        ProductDisplayController.ResetStoreProducts();
        CardView.FadeOut(c_WaitBeforeFadeOut, ResumeClientSpawn);
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
