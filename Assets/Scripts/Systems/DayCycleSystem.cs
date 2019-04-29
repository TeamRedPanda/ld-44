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
    public EndGameCardView m_EndGameCardView;

    private int m_CurrentDay = 1;
    private const int c_FinalDay = 4;

    private float m_CurrentDayTime = 0f;
    private const float c_DayTimeDuration = 5f;

    private bool IsGameOver = false;

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
        // Just wait the player restart the game.
        if (IsGameOver)
            return;

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
        if (day > c_FinalDay) {
            IsGameOver = true;
            ShowEndGameScreen();
            return;
        }
        string dayText = GetDayText(day);
        CardView.SetCardText(dayText);
        CardView.FadeIn(callback:ResetShopState);
    }

    private void ShowEndGameScreen()
    {
        ClientSpawnSystem.RemoveAllClients();
        ClientSpawnSystem.PauseSpawn();

        m_EndGameCardView.Setup(156, 15, true);
        m_EndGameCardView.Show();
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
