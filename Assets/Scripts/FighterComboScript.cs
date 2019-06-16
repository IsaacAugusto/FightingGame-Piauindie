using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterComboScript : MonoBehaviour
{
    private Animator _anim;
    public int _noOfClicks = 0;
    float _lastClickedTime = 0;
    [SerializeField] float _maxComboDelay = 1;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    void Update()
    {
        if (Time.time - _lastClickedTime > _maxComboDelay)
        {
            Debug.Log("resetou");
            _noOfClicks = 0;
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            OnClick();
        }
    }

    void OnClick()
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
