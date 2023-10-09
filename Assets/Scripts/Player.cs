using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{

    [Header("Move & Jumo")]
    [SerializeField] private float runSpeed = 5f;
    [SerializeField] private float jumpForce = 6f;
    [SerializeField] private int extraJump = 1;
    [SerializeField] private float distanceCheckGround = 0.7f;

    [HideInInspector] private LayerMask _layerMask;

    BulletPoolManager _bulletPoolManager;

    Rigidbody2D rb;
    [HideInInspector] public float _moveX;
    Animator animator;
    RaycastHit2D hit;
    CapsuleCollider2D bodyCollider;

    //bool _isAlive = true;
    [HideInInspector]  public bool _isJump = false;
    [HideInInspector]  public bool _isShoot = false;
    private bool _isGrounded = false;
    private bool _facingRight = true;
    private int _remainingJump;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _layerMask = LayerMask.GetMask("Ground");
        bodyCollider = GetComponent<CapsuleCollider2D>();
        _remainingJump = extraJump;
    }

    private void Update()
    {
        GroundCheck();
        Run();
        Jump();
        Shoot();
        Death();
    }

     private void Run()
    {
        Vector2 playerVelocity = new Vector2(_moveX * runSpeed, rb.velocity.y);
        rb.velocity = playerVelocity;

        bool playerHorizontalSpeed = Mathf.Abs(rb.velocity.x) > Mathf.Epsilon;

        if (_moveX > 0 && !_facingRight || _moveX < 0 && _facingRight)
        {
            Flip();
        }

        animator.SetBool("isRun", playerHorizontalSpeed);
    }

    private void Jump()
    {
        if(_isJump && _isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            _remainingJump--;

            if (_remainingJump == 0)
            {
                _remainingJump = extraJump;
            }
        }

        if (!_isGrounded)
        {
            _isJump = true;
        }
        else
        {
            _isJump = false;
        }

        animator.SetBool("isJump", _isJump);
    }

    private void GroundCheck()
    {
        hit = Physics2D.Raycast(transform.position, Vector2.down, distanceCheckGround, _layerMask);
        if (hit.collider)
        {
            _isGrounded = true;
            return;
        }

        _isGrounded = false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, Vector2.down * distanceCheckGround);
    }

    private void Flip()
    {
        _facingRight = !_facingRight;
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }

    private void Death()
    {
        if (bodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies")))
        {
            animator.SetTrigger("Death");
            //_isAlive = false;
        }
    }

    private void Shoot()
    {
        if (_isShoot)
        {
            GameObject bullet = _bulletPoolManager.GetBullet();
            if (bullet != null)
            {
                bullet.transform.position = transform.position;
                bullet.GetComponent<Rigidbody2D>().velocity = Vector2.right * bullet.GetComponent<BulletController>()._bulletSpeed;
            }
        }
    }
}
