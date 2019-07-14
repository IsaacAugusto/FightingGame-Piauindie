using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public FighterBehavior Player1, Player2;
    [SerializeField] private GameObject _kirbyDaniel, _subDaniel;
    [SerializeField] private Transform _player1Spawn, _player2Spawn;
    private int _player1Character, _player2Character;

    private void Awake()
    {
        GetCharacters();
        SpawnPlayers();
    }

    void Start()
    {
    }

    void Update()
    {

    }

    public FighterBehavior GetPlayerRef(int player)
    {
        if (player == 1)
        {
            return Player1;
        }
        else if (player == 2)
        {
            return Player2;
        }
        else
        {
            return null;
        }
    }

    private void SpawnPlayers()
    {
        switch (_player1Character)
        {
            case 1:
                Player1 = Instantiate(_kirbyDaniel, _player1Spawn.position, Quaternion.identity).GetComponent<FighterBehavior>();
                Player1.Player = PlayerType.Player1;
                break;

            case 2:
                Player1 = Instantiate(_subDaniel, _player1Spawn.position, Quaternion.identity).GetComponent<FighterBehavior>();
                Player1.Player = PlayerType.Player1;
                break;
            
            default:
                break;
        }

        switch (_player2Character)
        {
            case 1:
                Player2 = Instantiate(_kirbyDaniel, _player2Spawn.position, Quaternion.identity).GetComponent<FighterBehavior>();
                Player2.Player = PlayerType.Player2;
                break;

            case 2:
                Player2 = Instantiate(_subDaniel, _player2Spawn.position, Quaternion.identity).GetComponent<FighterBehavior>();
                Player2.Player = PlayerType.Player2;
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
            _player2Character = 2;
        }
    }
}
