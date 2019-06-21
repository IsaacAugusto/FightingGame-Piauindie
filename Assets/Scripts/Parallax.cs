using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private GameObject _cam;
    [SerializeField] private float _parallax;
    private float _lenght, _startPos;

    void Start()
    {
        _startPos = transform.position.x;
        _lenght = GetComponentInChildren<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {
        float dist = (_cam.transform.position.x * _parallax);

        Vector3 pos = new Vector3(_startPos + dist, transform.position.y, transform.position.z);

        transform.position = Vector3.Lerp(transform.position, pos, 1);
    }
}
