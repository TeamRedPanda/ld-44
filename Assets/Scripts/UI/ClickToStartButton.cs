using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickToStartButton : MonoBehaviour
{
    public void StartGame()
    {
        Debug.Log("Starting game.");
        SceneManager.LoadScene("Game");
    }
}
