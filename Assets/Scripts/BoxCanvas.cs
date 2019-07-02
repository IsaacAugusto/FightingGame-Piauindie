using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxCanvas : MonoBehaviour
{
    [SerializeField] private Text _score1, _score2;
    private string _scoreString = "Score "; 

    void Start()
    {
    }

    void Update()
    {
        UpdateScore();
    }

    private void UpdateScore()
    {
        _score1.text = _scoreString + BoxSingleton.Instance.Player1Score;
        _score2.text = _scoreString + BoxSingleton.Instance.Player2Score;

    }
}
