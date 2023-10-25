using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float enemyMoveSpeed = 2f;
    [SerializeField] private float distanceCheckGround = 2f;
    [SerializeField] private float rayDistance = 3f;

    [SerializeField] private List<float> _patrolEnemy;
    [SerializeField] Transform player;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] public bool _enemyStatic;

    [HideInInspector] private LayerMask _layerMask;
    [HideInInspector] private LayerMask _playerMask;

    Rigidbody2D rb;
    Animator animator;
    RaycastHit2D hit;

    private bool _isGrounded = true;
    private bool _visiblePlayer;
    private int index;
    private bool _facingLeft = true;
    //private Vector3 _lastPlayerPosition;

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

    private void FixedUpdate()
    {
        ChoisePatrolPoint();
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

            Debug.Log(direction.x);
        }
    }

    private void Flip()
    {
        _facingLeft = !_facingLeft;
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }

    private void ChoisePatrolPoint()
    {
        if (!_visiblePlayer && !_enemyStatic)
        { 
            for (int i = 0; i < _patrolEnemy.Count; i++)
            {
                if (transform.position.x == _patrolEnemy[i])
                {
                    index = i > 0 ? 0 : 1;

                    Flip();
                }
            }

            Patrol(index);
        }
    }

    private void Patrol(int currentPointIndex)
    {
        var position = new Vector2(_patrolEnemy[currentPointIndex], transform.position.y);
        var currentPosition = rb.position;
        Vector2 newPosition = Vector2.MoveTowards(currentPosition, position, enemyMoveSpeed * Time.deltaTime);
        rb.MovePosition(newPosition);
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

    private void OnCollisionEnter2D(Collision2D coll)
    {
        Vector2 collPosition = coll.transform.position;

        if (collPosition.y > transform.position.y && !_enemyStatic)
        {
            if (coll.otherCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
            {
                var coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);

                if (coin != null)
                {
                    Destroy(gameObject);
                }
            }
        }
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
