using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum PlayerType {Player1, Player2}

public class FighterBehavior : MonoBehaviour
{
    [SerializeField] private PlayerType _player;

    [SerializeField] private LayerMask _groudLayer;
    [SerializeField] private LayerMask _fighterLayer;

    [SerializeField] private Transform _atkPosition;

    private KeyCode _jumpKey;
    private KeyCode _attackKey;

    public float KnockbackTime;
    [SerializeField] private float _mSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _atkRange;
    [SerializeField] private float _hp;
    [SerializeField] private float _damage;
    private float _xInput;

    public bool IsStuned;
    private FighterComboScript _comboScript;
    private Rigidbody2D _rb;
    private Animator _anim;
    private bool _isGrounded;

    void Start()
    {
        _comboScript = GetComponent<FighterComboScript>();
        if (_player == PlayerType.Player1)
        {
            _jumpKey = KeyCode.W;
            _attackKey = KeyCode.F;
        } else
        {
            _jumpKey = KeyCode.UpArrow;
            _attackKey = KeyCode.P;
        }

        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Movement();
    }

    void Update()
    {
        GetAxis();
        Jump();
        KnockbackTimer();
        PlayerAttack();
    }

    private void KnockbackTimer()
    {
        if (IsStuned)
            KnockbackTime -= Time.deltaTime;
        if (KnockbackTime < 0)
        {
            IsStuned = false;
            KnockbackTime = 1;
        }
    }

   /* private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_atkPosition.transform.position, _atkRange);
    }*/

    public void DealDamage()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(_atkPosition.position, _atkRange, _fighterLayer);
        for (int i = 0 ; i < enemiesToDamage.Length; i++)
        {
            if (!enemiesToDamage[i].gameObject.Equals(transform.gameObject))
            {
                if (enemiesToDamage[i].tag == "Box")
                {
                    enemiesToDamage[i].GetComponent<BoxBehaviour>().HitTheBox(this.gameObject);
                    return;
                }
                Push(enemiesToDamage[i].gameObject);
            }
        }
    }

    public void ReciveDamage(float damageTaken)
    {
        Debug.Log("I Take damage");
        _hp -= damageTaken;
    }

    private void PlayerAttack()
    {
        if (Input.GetKeyDown(_attackKey))
        {
            DealDamage();
        }
    }

    private void Push(GameObject fighter)
    {
        fighter.GetComponent<FighterBehavior>().IsStuned = true;
        fighter.GetComponent<FighterBehavior>().KnockbackTime = 1;
        fighter.GetComponent<Rigidbody2D>().velocity = ((transform.right) + Vector3.up).normalized * 10;
    }

    private void GetAxis()
    {
        if (_player == PlayerType.Player1)
        {
            _xInput = Input.GetAxisRaw("HPlayer1");
        }
        else
        {
            _xInput = Input.GetAxisRaw("HPlayer2");
        }
    }

    private void Movement()
    {
        if (!IsStuned)
            _rb.velocity = new Vector2((_xInput * _mSpeed), _rb.velocity.y);

        if (_rb.velocity.x > 0)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }
        else if (_rb.velocity.x < 0)
        {
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }

        if (_xInput == -1 || _xInput == 1)
        {
            _anim.SetBool("IsWalking", true);
            _anim.SetBool("Idle", false);
        }
        else
        {
            _anim.SetBool("IsWalking", false);
            _anim.SetBool("Idle", true);
        }
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            if (Input.GetKeyDown(_jumpKey))
            {
                _rb.velocity = Vector2.up * _jumpForce;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Ground")
        {
            _isGrounded = true;
            _anim.SetBool("IsJumping", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground"){
            _isGrounded = false;
            _anim.SetBool("IsJumping", true);
        }
    }
}
