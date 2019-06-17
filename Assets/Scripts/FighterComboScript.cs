using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterComboScript : MonoBehaviour
{
    public int _noOfClicks = 0;
    [SerializeField] float _maxComboDelay = 1;
    private Animator _anim;
    private float _lastClickedTime = 0;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Time.time - _lastClickedTime > _maxComboDelay)
        {
            _noOfClicks = 0;
        }
    }

    public void GetAttacks()
    {
        if (_noOfClicks < 3)
        {
            _lastClickedTime = Time.time;
            _noOfClicks++;
            if (_noOfClicks >= 1)
            {
                _anim.SetBool("Attack1", true);
            }
            _noOfClicks = Mathf.Clamp(_noOfClicks, 0, 3);
        }
    }
}
