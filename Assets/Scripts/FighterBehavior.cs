using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerType {Player1, Player2}

public class FighterBehavior : MonoBehaviour
{
    public PlayerType Player;

    [SerializeField] private GameObject _hitParticle;
    [SerializeField] private LayerMask _groudLayer;
    [SerializeField] private LayerMask _fighterLayer;
    [SerializeField] private Transform _atkPosition;
    [SerializeField] private float _blockTime;
    [SerializeField] private float _mSpeed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _atkRange;
    [SerializeField] private float _atkSpeed;
    [SerializeField] private float _atkTime;
    [SerializeField] private float _timeBlinking;
    [SerializeField] private AudioClip _hurt1Sound, _hurt2Sound, _jumpSound, _hitSound;

    private KeyCode _jumpKey;
    private KeyCode _attackKey;
    private KeyCode _blockKey;

    private Quaternion _rotateLeft = new Quaternion(0, 180, 0, 0);
    private GameManager _gameManager;
    public float Hp;
    public float KnockbackTime;
    public int SpecialDamage;
    public bool IsStuned;
    public bool IsBlocking = false;
    public bool IsBlinking = false;


    private FighterBehavior _otherPlayer;

    private float _xInput;
    private float _blockTimer;
    private float _timeLastBlock;
    private float _timeLastWalk;
    private float _blinkTime;

    private Vector2 _screenBoundsPositive;
    private Vector2 _screenBoundsNegative;
    private bool _readyToBlock = true;
    private Rigidbody2D _rb;
    private Animator _anim;
    private bool _isGrounded;
    private AudioSource _source;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
        _source = GetComponent<AudioSource>();
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

        SetOtherPlayer();
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
        BlinkTimeDecrease();
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
                if (!_source.isPlaying)
                    _source.PlayOneShot(_hitSound);
                if (!enemy.GetComponent<FighterBehavior>().IsBlocking)
                {
                    if (!enemy.GetComponent<FighterBehavior>().IsBlinking)
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
                }
                else
                {
                    _anim.Play("Damage");
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

    public void ReciveDamage()
    {
        Instantiate(_hitParticle, transform.position, Quaternion.identity);
        PlayHurtSound();
        _anim.Play("Damage");
        _blinkTime = _timeBlinking;
    }

    private void BlinkTimeDecrease()
    {
        if (_blinkTime >= 0)
        {
            IsBlinking = true;
            _blinkTime -= Time.deltaTime;
        }
        else
        {
            IsBlinking = false;
        }
    }

    private void PlayerAttack()
    {
        _atkTime -= Time.deltaTime;
        if (Input.GetKeyDown(_attackKey) && _atkTime <= 0 && !IsBlinking && !IsStuned)
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
            if (_otherPlayer.transform.position.x > transform.position.x)
            {
                transform.rotation = Quaternion.identity;
            }
            else
            {
                transform.rotation = transform.rotation = _rotateLeft;
            }

            IsBlocking = true;
            _anim.Play("Block");
            _timeLastBlock = Time.time;
            _readyToBlock = false;
        }
        if (IsBlocking)
        {
            _blockTimer -= Time.deltaTime;
            if (_isGrounded)
                _rb.velocity = Vector2.up * _rb.velocity.y;
            IsStuned = true;
        }
        if (_blockTimer <= 0)
        {
            IsStuned = false;
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
        fighter.GetComponent<FighterBehavior>().ReciveDamage();
        fighter.GetComponent<FighterBehavior>().IsStuned = true;
        fighter.GetComponent<FighterBehavior>().KnockbackTime = 0.5f;
        fighter.GetComponent<Rigidbody2D>().velocity = ((transform.right * direction) + Vector3.up) * 2;
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
            transform.rotation = Quaternion.identity;
        }
        else if (_rb.velocity.x < 0)
        {
            transform.rotation = _rotateLeft;
        }

        if (_xInput == -1 || _xInput == 1)
        {
            _anim.SetBool("IsWalking", true);
            _anim.SetBool("Idle", false);
            CheckRun();
        }
        else
        {
            _anim.SetBool("IsWalking", false);
            _anim.SetBool("Idle", true);
        }

    }

    private void CheckRun()
    {
        if (_xInput == 0)
            _timeLastWalk = Time.time;

        if (Time.time - _timeLastWalk <= 0.5f && _xInput == 1)
        {
            Debug.Log("DobleTap");
        }
    }

    private void Jump()
    {
        if (_isGrounded)
        {
            if (Input.GetKeyDown(_jumpKey))
            {
                _rb.velocity = Vector2.up * _jumpForce;
                _source.PlayOneShot(_jumpSound);
            }
        }
    }

    private void Respawn()
    {
        _rb.velocity = Vector2.zero;
        transform.position = Vector3.zero;
    }

    private void SetOtherPlayer()
    {
        if (Player == PlayerType.Player1)
        {
            _otherPlayer = _gameManager.GetPlayerRef(2);
        }
        else
        {
            _otherPlayer = _gameManager.GetPlayerRef(1);
        }
    }

    private void PlayHurtSound()
    {
        int x = Random.Range(1, 3);
        if (x == 1)
        {
            _source.PlayOneShot(_hurt1Sound);
            return;
        }
        else if (x == 2)
        {
            _source.PlayOneShot(_hurt2Sound);
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "OutOfLevel")
        {
            if (Player == PlayerType.Player1)
            {
                BoxSingleton.Instance.Player1Score-= 3;
            }
            else
            {
                BoxSingleton.Instance.Player2Score-= 3;
            }
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
            if (!IsStuned)
            {
                _isGrounded = false;
                _anim.SetBool("IsJumping", true);
            }
        }
    }
}
