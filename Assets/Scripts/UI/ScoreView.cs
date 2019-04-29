using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreView : MonoBehaviour
{
    public ScoreSystem ScoreSystem;
    public TMPro.TextMeshProUGUI Text;
    // Start is called before the first frame update
    void Start()
    {
        ScoreSystem.OnScoreChanged += SetScore;
    }

    private void SetScore(int score)
    {
        Text.text = score.ToString();
    }
}
