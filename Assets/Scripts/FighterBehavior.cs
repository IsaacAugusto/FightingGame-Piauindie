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

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
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
