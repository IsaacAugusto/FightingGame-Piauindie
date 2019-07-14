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
    [SerializeField] private Sprite _boxAttack;
    [SerializeField] private GameObject _boxParticle;
    [SerializeField] private AudioClip _hitBoxSound;
    [SerializeField] private float _speed;
    private AudioSource _source;
    private SpriteRenderer _spriteRenderer;
    public float _initTime;
    private bool _blinking;
    private bool _attacking = false;
    private Rigidbody2D _rb;
    private Animator _anim;
    private GameManager _gameManager;
    private float _timeAttack = 5;

    private FighterBehavior _player1, _player2;
    private Transform _wininngPlayerTransform;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _player1 = _gameManager.GetPlayerRef(1);
        _player2 = _gameManager.GetPlayerRef(2);
        _source = GetComponent<AudioSource>();
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
        //Attack();
    }

    private void SpriteUpdate()
    {
        if (!_attacking)
        {
            if (Hp > (InitialHp / 3) * 2)
            {
                _spriteRenderer.sprite = _boxComplete;
            }
            else if (Hp > (InitialHp / 3) && Hp < (InitialHp / 3) * 2)
            {
                _spriteRenderer.sprite = _boxHalf;
            }
            else if (Hp < InitialHp / 3 && Hp > 0)
            {
                _spriteRenderer.sprite = _boxBroken;
            }
            else if (Hp <= 0)
            {
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<BoxCollider2D>().enabled = false;
                BoxSingleton.Instance.FinishStage();
            }
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
        if (!_blinking && !_attacking)
        {
            Instantiate(_boxParticle, transform.position, Quaternion.identity);
            _source.PlayOneShot(_hitBoxSound);
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
        _rb.velocity = ((fighter.transform.right) + Vector3.up).normalized * 3.5f;
        _rb.AddTorque(10f);
        _initTime = Time.time;
        _blinking = true;
    }


    private void Attack()
    {
        if (Hp == 10)
        {
            _timeAttack -= Time.deltaTime;
            if (_timeAttack <= 0)
            {
                _timeAttack = 5f;
                _attacking = false;
                Hp--;
            }
            _attacking = true;
            AttackState(1);
        }
        if (Hp == 5)
        {
            _timeAttack -= Time.deltaTime;
            if (_timeAttack <= 0)
            {
                _timeAttack = 5f;
                _attacking = false;
                Hp--;
            }
            _attacking = true;
            AttackState(2);
        }
    }

    private void AttackState(int state)
    {
        GetWinningPlayer();
        _spriteRenderer.sprite = _boxAttack;
        transform.position = Vector2.MoveTowards(transform.position, _wininngPlayerTransform.position, (_speed * state) * Time.deltaTime);
        if (_attacking == false)
        {
            return;
        }
    }

    private void ResetAttacking()
    {
        _attacking = false;
        Hp--;
    }

    private void GetWinningPlayer()
    {
        if (BoxSingleton.Instance.Player1Score > BoxSingleton.Instance.Player2Score)
        {
            _wininngPlayerTransform = _player1.transform;
        }
        else // Sempre vai atrás do player 2 caso de empate por enquanto
        {
            _wininngPlayerTransform = _player2.transform;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.tag == "Walls")
        {
            transform.position = new Vector3(-4, 0, 0);
        }

        if (_attacking)
        {
            if (collision.transform.tag == "Player")
            {
                collision.gameObject.GetComponent<FighterBehavior>().ReciveDamage();
                _attacking = false;
                Hp--;
                if (collision.gameObject.GetComponent<FighterBehavior>().Player == PlayerType.Player1)
                {
                    BoxSingleton.Instance.Player1Score -= 2;
                }
                else
                {
                    BoxSingleton.Instance.Player2Score -= 2;
                }
            }
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
