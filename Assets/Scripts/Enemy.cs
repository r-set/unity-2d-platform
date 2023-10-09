using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemyMoveSpeed = 2f;
    [SerializeField] private float distanceCheckGround = 2f;
    [SerializeField] private float rayDistance = 3f;

    [SerializeField] Transform player;
    [SerializeField] private bool _enemyStatic;

    [HideInInspector] private LayerMask _layerMask;
    [HideInInspector] private LayerMask _playerMask;

    Rigidbody2D rb;
    Animator animator;
    RaycastHit2D hit;

    private bool _isGrounded = true;
    private bool _visiblePlayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _layerMask = LayerMask.GetMask("Ground");
        _playerMask = LayerMask.GetMask("Player");
    }

    private void Update()
    {
        GroundCheck();
        Attack();
        Death();
    }

     private void Attack()
    {
        if (_enemyStatic)
        {
            _visiblePlayer = Physics2D.Raycast(transform.position, Vector2.left, rayDistance, _playerMask);
            animator.SetBool("isAttack", _visiblePlayer);
        }
        else
        {
            _visiblePlayer = Physics2D.Raycast(transform.position, Vector2.left, rayDistance, _playerMask);
            Move();
        }
    }

    private void Move()
    {
        if (_visiblePlayer && !_enemyStatic)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            rb.velocity = new Vector2(direction.x * enemyMoveSpeed, rb.velocity.y);
        }
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
        Gizmos.DrawRay(transform.position, Vector2.left * rayDistance);
        Gizmos.DrawRay(transform.position, Vector2.down * distanceCheckGround);
    }

    private void Death()
    {
        if (!_enemyStatic)
        {
            if (!_isGrounded)
            {
                Destroy(gameObject);
            }
        }
    }
}
