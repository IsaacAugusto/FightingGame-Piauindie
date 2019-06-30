using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private Transform _player1Pos;
    [SerializeField] private Transform _player2Pos;
    private Vector3 _relativePos;
    private Vector3 speed;

    void Start()
    {
        speed = Vector3.zero;
        GetPlayersPos();
    }

    void FixedUpdate()
    {
        if (_player2Pos == null)
        {
            transform.position = Vector3.SmoothDamp(transform.position, _player1Pos.position, ref speed, 0.3f);
            transform.position = new Vector3(transform.position.x, 0, -10);
            return;
        }
        _relativePos = Vector3.Lerp(_player1Pos.position, _player2Pos.position, 0.5f);
        transform.position = Vector3.SmoothDamp(transform.position, _relativePos, ref speed, 0.3f);
        transform.position = new Vector3(transform.position.x, 0, -10);
    }

    private void GetPlayersPos()
    {
        FighterBehavior[] players;
        players = FindObjectsOfType<FighterBehavior>();
        foreach (FighterBehavior _player in players)
        {
            if (_player1Pos == null && _player.Player == PlayerType.Player1)
            {
                _player1Pos = _player.transform;
            }

            if (_player2Pos == null && _player.Player == PlayerType.Player2)
            {
                _player2Pos = _player.transform;
            }
        }
    }
}
