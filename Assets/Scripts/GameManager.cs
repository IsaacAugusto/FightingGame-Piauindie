using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _kirbyDaniel;
    [SerializeField] private Transform _player1Spawn, _player2Spawn;
    private int _player1Character, _player2Character;

    private void Awake()
    {
        GetCharacters();
    }

    void Start()
    {
        SpawnPlayers();
    }

    void Update()
    {

    }

    private void SpawnPlayers()
    {
        FighterBehavior player1;
        FighterBehavior player2;
        switch (_player1Character)
        {
            case 1:
                player1 = Instantiate(_kirbyDaniel, _player1Spawn.position, Quaternion.identity).GetComponent<FighterBehavior>();
                player1.Player = PlayerType.Player1;
                break;

            default :
                break;
        }

        switch (_player2Character)
        {
            case 1:
                player2 = Instantiate(_kirbyDaniel, _player2Spawn.position, Quaternion.identity).GetComponent<FighterBehavior>();
                player2.Player = PlayerType.Player2;
                break;

            default:
                break;
        }
    }

    private void GetCharacters()
    {
        if (PlayerPrefs.GetInt("Player1") != 0 && PlayerPrefs.GetInt("Player2") != 0)
        {
        _player1Character = PlayerPrefs.GetInt("Player1");
        _player2Character = PlayerPrefs.GetInt("Player2");
        }
        else
        {
            _player1Character = 1;
            _player2Character = 1;
        }
    }
}
