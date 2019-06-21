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
    private Rigidbody2D _rb;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        _initialHp = _hp;
    }

    void Update()
    {
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

    public void HitTheBox(GameObject fighter)
    {
        _hp -= 1;
        _rb.velocity = ((fighter.transform.right) + Vector3.up).normalized * 3;
        _rb.AddTorque(-10f);
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
