using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoxSingleton : MonoBehaviour
{
    public static BoxSingleton Instance = null;

    public int Player1Score = 0, Player2Score = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        ClampScores();
    }

    private void ClampScores()
    {
        Player1Score = Mathf.Clamp(Player1Score, 0, 100);
        Player2Score = Mathf.Clamp(Player2Score, 0, 100);
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
