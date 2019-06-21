using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _playerPos;
    private Vector3 speed;

    void Start()
    {
        speed = Vector3.zero;
    }

    void FixedUpdate()
    {
        transform.position = Vector3.SmoothDamp(transform.position, _playerPos.transform.position, ref speed, 0.3f);
        transform.position = new Vector3(transform.position.x, 0 , -10);
    }
}
