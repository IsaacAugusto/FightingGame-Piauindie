using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType {Player1, Player2}

public class FighterBehavior : MonoBehaviour
{
    public PlayerType Player;

    [SerializeField] private LayerMask _groudLayer;
    [SerializeField] private LayerMask _fighterLayer;

    [SerializeField] private Transform _atkPosition;

    private KeyCode _jumpKey;
    private KeyCode _attackKey;
    private KeyCode _blockKey;

    public float Hp;
    public float KnockbackTime;
    public int SpecialDamage;
    public bool IsStuned;
    public bool IsBlocking = false;

    [SerializeField] private float _blockTime;
    [SerializeField] private float _mSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _atkRange;
    [SerializeField] private float _atkSpeed;
    [SerializeField] private float _atkTime;
    private float _xInput;
    private float _blockTimer;
    private float _timeLastBlock;

    private Vector2 _screenBoundsPositive;
    private Vector2 _screenBoundsNegative;
    private bool _readyToBlock = true;
    private Rigidbody2D _rb;
    private Animator _anim;
    private bool _isGrounded;

    void Start()
    {
        if (Player == PlayerType.Player1)
        {
            _jumpKey = KeyCode.W;
            _attackKey = KeyCode.F;
            _blockKey = KeyCode.G;
        } else
        {
            _jumpKey = KeyCode.UpArrow;
            _attackKey = KeyCode.P;
            _blockKey = KeyCode.O;
        }

        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();

        _blockTimer = _blockTime;
    }

    private void LateUpdate()
    {
        _screenBoundsPositive = Camera.main.ScreenToWorldPoint(Vector3.one * Screen.width);
        _screenBoundsNegative = Camera.main.ScreenToWorldPoint(Vector3.zero);
        if (transform.position.x > _screenBoundsPositive.x)
        {
            Vector3 viewPos = transform.position;
            viewPos.x = _screenBoundsPositive.x;
            transform.position = viewPos;
        } else if (transform.position.x < _screenBoundsNegative.x)
        {
            Vector3 viewPos = transform.position;
            viewPos.x = _screenBoundsNegative.x;
            transform.position = viewPos;
        }
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
        PlayerBlock();
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

    public void DealDamage(int TypeOfDamage)
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(_atkPosition.position, _atkRange, _fighterLayer);
        for (int i = 0 ; i < enemiesToDamage.Length; i++)
        {
            Collider2D enemy = enemiesToDamage[i];
            if (!enemy.gameObject.Equals(transform.gameObject))
            {
                if (enemy.tag == "Box")
                {
                    enemy.GetComponent<BoxBehaviour>().HitTheBox(this.gameObject, TypeOfDamage);
                    return;
                }

                if (!enemy.GetComponent<FighterBehavior>().IsBlocking)
                {
                    if (TypeOfDamage == 0)
                    {
                        Push(enemy.gameObject, 1);
                        if (enemy.GetComponent<FighterBehavior>().Player == PlayerType.Player1)
                        {
                            BoxSingleton.Instance.Player1Score--;
                            return;
                        }
                        else
                        {
                            BoxSingleton.Instance.Player2Score--;
                            return;
                        }

                    }
                    else if (TypeOfDamage == 1)
                    {
                        Push(enemy.gameObject, 1);
                        return;
                    }
                }
                else
                {
                    Push(this.gameObject, -1);
                    if (Player == PlayerType.Player1)
                    {
                        BoxSingleton.Instance.Player1Score--;
                        return;
                    }
                    else
                    {
                        BoxSingleton.Instance.Player2Score--;
                        return;
                    }
                }
            }
        }
    }

    // não utilizado ainda ---------------------
    public void ReciveDamage(float damageTaken)
    {
        Debug.Log("I Take damage");
        Hp -= damageTaken;
    }
    //------------------------------------------
    private void PlayerAttack()
    {
        _atkTime -= Time.deltaTime;
        if (Input.GetKeyDown(_attackKey) && _atkTime <= 0)
        {
            DealDamage(0);
            _anim.Play("Punch");
            _atkTime = _atkSpeed;
        }
    }

    private void PlayerBlock()
    {
        if (Input.GetKeyDown(_blockKey) && !IsBlocking && _readyToBlock)
        {
            IsBlocking = true;
            _anim.Play("Block");
            _timeLastBlock = Time.time;
            _readyToBlock = false;
        }
        if (IsBlocking)
        {
            _blockTimer -= Time.deltaTime;
        }
        if (_blockTimer <= 0)
        {
            IsBlocking = false;
            _blockTimer = _blockTime;
        }
        if (Time.time - _timeLastBlock >= 1.5f)
        {
            _readyToBlock = true;
        }
    }

    private void Push(GameObject fighter, int direction)
    {
        fighter.GetComponent<FighterBehavior>().IsStuned = true;
        fighter.GetComponent<FighterBehavior>().KnockbackTime = 1;
        fighter.GetComponent<Rigidbody2D>().velocity = ((transform.right * direction) + Vector3.up * 1.5f) * 3;
    }

    private void GetAxis()
    {
        if (Player == PlayerType.Player1)
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

    private void Respawn()
    {
        _rb.velocity = Vector2.zero;
        transform.position = Vector3.zero;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "OutOfLevel")
        {
            Invoke("Respawn", 2);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Ground" || collision.tag == "Player" || collision.tag == "Box")
        {
            _isGrounded = true;
            _anim.SetBool("IsJumping", false);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Ground" || collision.tag == "Player" || collision.tag == "Box")
        {
            _isGrounded = false;
            _anim.SetBool("IsJumping", true);
        }
    }
}
