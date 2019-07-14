using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxCanvas : MonoBehaviour
{
    [SerializeField] private Text _score1, _score2;
    [SerializeField] private Text _winner;
    [SerializeField] private Image _boxHP;
    private string _scoreString = "Score ";
    private BoxBehaviour _boxScript;

    void Start()
    {
        _winner.GetComponent<Text>().enabled = false;
        _boxScript = FindObjectOfType<BoxBehaviour>();
    }

    void Update()
    {
        UpdateScore();
        UpdateBoxLife();
        CheckWinner();
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

    private void CheckWinner()
    {
        if (BoxSingleton.Instance.HaveWinner)
        {
            if (BoxSingleton.Instance.Player1Score > BoxSingleton.Instance.Player2Score)
            {
                _winner.text = "Player 1 foi o vencedor!";
            }
            else if (BoxSingleton.Instance.Player2Score > BoxSingleton.Instance.Player1Score)
            {
                _winner.text = "Player 2 foi o vencedor!";
            }
            else
            {
                _winner.text = "Foi um empate!";
            }
            _winner.GetComponent<Text>().enabled = true;
        }
    }
}
