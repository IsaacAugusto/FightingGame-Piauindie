using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehaviour : MonoBehaviour
{
    [SerializeField] private Sprite _boxComplete;
    [SerializeField] private Sprite _boxHalf;
    [SerializeField] private Sprite _boxBroken;
    [SerializeField] private float _hp;
    private SpriteRenderer _spriteRenderer;
    private float _initialHp;
    private float _initTime;
    private bool _blinking;
    private Rigidbody2D _rb;
    private Animator _anim;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialHp = _hp;
    }

    void Update()
    {
        if (_blinking)
        {
            Blink();
        }
        SpriteUpdate();
    }

    private void SpriteUpdate()
    {
        if (_hp > (_initialHp / 3) * 2)
        {
            _spriteRenderer.sprite = _boxComplete;
        } else if (_hp > (_initialHp/3) && _hp < (_initialHp / 3 ) * 2)
        {
            _spriteRenderer.sprite = _boxHalf;
        }
        else if (_hp < _initialHp/3 && _hp > 0)
        {
            _spriteRenderer.sprite = _boxBroken;
        } else if (_hp <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    private void Blink()
    {
        _anim.SetBool("Blinking", true);
        if(Time.time - _initTime > 1)
        {
            _blinking = false;
            _anim.SetBool("Blinking", false);
        }
    }

    public void HitTheBox(GameObject fighter)
    {
        if (!_blinking)
        {
            _hp -= 1;
            _rb.velocity = ((fighter.transform.right) + Vector3.up).normalized * 4.5f;
            _rb.AddTorque(-10f);
            _initTime = Time.time;
            _blinking = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Walls")
        {
            transform.position = new Vector3(-4, 0, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "OutOfLevel")
        {
            transform.position = new Vector3(-4, 0, 0);
            _hp = _initialHp;
        }
    }
}
