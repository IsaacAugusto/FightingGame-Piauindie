using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FighterBehavior : MonoBehaviour
{
    [SerializeField]private float _mSpeed;
    [SerializeField]private float _jumpForce;
    [SerializeField]private LayerMask _groudLayer;
    private Vector3 _rayPosition;
    private Rigidbody2D _rb;
    private float _xInput;
    private bool _isGrounded;
    private SpriteRenderer _spriteR;
    private Animator _anim;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _anim = GetComponent<Animator>();
        _spriteR = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        _rayPosition = transform.position - new Vector3(0, 1, 0);
        GetAxis();
        Movement();
        Jump();
    }

    private void GetAxis()
    {
        _xInput = Input.GetAxisRaw("HPlayer1");
    }

    private void Movement()
    {
        _rb.velocity = new Vector2(_xInput * _mSpeed, _rb.velocity.y);

        if (_rb.velocity.x > 0)
        {
            _spriteR.flipX = true;
        }
        else if (_rb.velocity.x < 0)
        {
            _spriteR.flipX = false;
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
            if (Input.GetKeyDown(KeyCode.W))
            {
                _rb.AddForce(Vector2.up * _jumpForce);
            }
        }
    }
}
