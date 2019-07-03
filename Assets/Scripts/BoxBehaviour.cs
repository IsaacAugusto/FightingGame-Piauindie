using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxBehaviour : MonoBehaviour
{
    public float Hp;
    public float InitialHp;
    [SerializeField] private Sprite _boxComplete;
    [SerializeField] private Sprite _boxHalf;
    [SerializeField] private Sprite _boxBroken;
    private SpriteRenderer _spriteRenderer;
    public float _initTime;
    private bool _blinking;
    private Rigidbody2D _rb;
    private Animator _anim;


    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
        InitialHp = Hp;
    }

    void Update()
    {
        if (_blinking)
        {
            Blink();
        }
        SpriteUpdate();
    }

    private void ReloadTheScene()
    {
        BoxSingleton.Instance.ReloadScene();
    }

    private void SpriteUpdate()
    {
        if (Hp > (InitialHp / 3) * 2)
        {
            _spriteRenderer.sprite = _boxComplete;
        } else if (Hp > (InitialHp/3) && Hp < (InitialHp / 3 ) * 2)
        {
            _spriteRenderer.sprite = _boxHalf;
        }
        else if (Hp < InitialHp/3 && Hp > 0)
        {
            _spriteRenderer.sprite = _boxBroken;
        } else if (Hp <= 0)
        {
            Destroy(this.gameObject);
            ReloadTheScene();
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

    public void HitTheBox(GameObject fighter, int TypeOfHit)
    {
        if (!_blinking)
        {
            PushTheBox(fighter);
            ScoreIncrease(fighter, TypeOfHit);
        }
    }

    private void ScoreIncrease(GameObject fighter, int TypeOfHit)
    {
        if (fighter.GetComponent<FighterBehavior>().Player == PlayerType.Player1)
        {
            if (TypeOfHit == 0)
            {
                BoxSingleton.Instance.Player1Score++;
                Hp--;
            }
            else if (TypeOfHit == 1)
            {
                int damage = fighter.GetComponent<FighterBehavior>().SpecialDamage;
                BoxSingleton.Instance.Player1Score += damage;
                Hp -= damage;
            }
        }
        else if (fighter.GetComponent<FighterBehavior>().Player == PlayerType.Player2)
        {
            if (TypeOfHit == 0)
            {
                BoxSingleton.Instance.Player2Score++;
                Hp--;
            }
            else if (TypeOfHit == 1)
            {
                int damage = fighter.GetComponent<FighterBehavior>().SpecialDamage;
                BoxSingleton.Instance.Player2Score += damage;
                Hp -= damage;
            }
        }
    }

    private void PushTheBox(GameObject fighter)
    {
        _rb.velocity = ((fighter.transform.right) + Vector3.up).normalized * 4.5f;
        _rb.AddTorque(10f);
        _initTime = Time.time;
        _blinking = true;
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
        }
    }
}
