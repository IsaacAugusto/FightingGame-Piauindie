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
    private Vector3 _rayPosition;
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
        GetAxis();
        Movement();
        Jump();
    }

    void Update()
    {
        KnockbackTimer();
        _rayPosition = transform.position - new Vector3(0, 1, 0);
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

    public void DealDamage()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(_atkPosition.position, _atkRange, _fighterLayer);
        for (int i = 0 ; i < enemiesToDamage.Length; i++)
        {
            if (!enemiesToDamage[i].gameObject.Equals(transform.gameObject))
            {
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
            _comboScript.GetAttacks();
        }
    }

    private void Push(GameObject fighter)
    {
        fighter.GetComponent<FighterBehavior>().IsStuned = true;
        fighter.GetComponent<FighterBehavior>().KnockbackTime = 1;
        fighter.GetComponent<Rigidbody2D>().velocity = ((-transform.right) + Vector3.up).normalized * 10;
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
            transform.rotation = new Quaternion(0, 180, 0, 0);
        }
        else if (_rb.velocity.x < 0)
        {
            transform.rotation = new Quaternion(0, 0, 0, 0);
        }

        if (_xInput == -1 || _xInput == 1)
        {
            _anim.SetBool("Walking", true);
        }
        else
        {
            _anim.SetBool("Walking", false);
        }
    }

    private void Jump()
    {
        _isGrounded = Physics2D.Raycast(_rayPosition, Vector2.down, 0.5f, _groudLayer);
        
        if (_isGrounded)
        {
            if (Input.GetKeyDown(_jumpKey))
            {
                _rb.AddForce(Vector2.up * _jumpForce);
            }
        }
    }
}
