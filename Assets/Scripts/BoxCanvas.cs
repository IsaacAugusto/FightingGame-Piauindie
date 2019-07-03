using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxCanvas : MonoBehaviour
{
    [SerializeField] private Text _score1, _score2;
    [SerializeField] private Image _boxHP;
    private string _scoreString = "Score ";
    private BoxBehaviour _boxScript;

    void Start()
    {
        _boxScript = FindObjectOfType<BoxBehaviour>();
    }

    void Update()
    {
        UpdateScore();
        UpdateBoxLife();
    }

    private void UpdateScore()
    {
        _score1.text = _scoreString + BoxSingleton.Instance.Player1Score;
        _score2.text = _scoreString + BoxSingleton.Instance.Player2Score;
    }

    private void UpdateBoxLife()
    {
        _boxHP.fillAmount = _boxScript.Hp / _boxScript.InitialHp;
    }
}
