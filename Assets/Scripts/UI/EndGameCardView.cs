using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndGameCardView : MonoBehaviour
{
    public Animator Animator;

    public TMPro.TextMeshProUGUI YearsCollectedText;
    public TMPro.TextMeshProUGUI TotalDeathsText;

    public GameObject PassedText;
    public GameObject FailedText;

    void Start()
    {
    }

    public void Setup(int yearsCollected, int totalDeaths, bool passed)
    {
        YearsCollectedText.text = $"Total Years Collected : {yearsCollected.ToString()}";
        TotalDeathsText.text = $"Total Deaths : {totalDeaths}";

        if (passed) {
            PassedText.SetActive(true);
            FailedText.SetActive(false);
        } else {
            PassedText.SetActive(false);
            FailedText.SetActive(true);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
        Animator.SetTrigger("Show");
    }
}
