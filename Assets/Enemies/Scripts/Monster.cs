using System;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour
{
    [SerializeField] private MonstersData _monstersData;
    [SerializeField] private float enemyMoveSpeed = 2f;
    //[SerializeField] private float distanceCheckGround = 2f;
    [SerializeField] private float _rayDistance = 3f;

    [SerializeField] private List<float> _pathPoint;
    [SerializeField] Transform player;
    [SerializeField] private GameObject coinPrefab;
    [SerializeField] public bool _enemyStatic;

    [HideInInspector] private LayerMask _layerMask;
    [HideInInspector] private LayerMask _playerMask;

    Rigidbody2D rb;
    Animator animator;
    RaycastHit2D hit;

    //private bool _isGrounded = true;
    private bool _visiblePlayer;
    private int index;
    private bool _facingLeft = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        _layerMask = LayerMask.GetMask("Ground");
        _playerMask = LayerMask.GetMask("Player");
    }

    private void Update()
    {
        _visiblePlayer = Physics2D.Raycast(transform.position, Vector3.left, _rayDistance, _playerMask);
    }

    private void FixedUpdate()
    {
        ChoisePatrolPoint();
    }

    private void ChoisePatrolPoint()
    {
        if(transform.position.x == _pathPoint[0])
        {
            _monstersData.isValid = true;
            index = 1;
            Flip();
        }

        if (transform.position.x == _pathPoint[1])
        {
            _monstersData.isValid = false;
            index = 0;
            Flip();
        }

        Patrol(_pathPoint[index]);
    }

    private void Flip()
    {
        _facingLeft = !_facingLeft;
        Vector3 playerScale = transform.localScale;
        playerScale.x *= -1;
        transform.localScale = playerScale;
    }

    private void Patrol(float pointXCoordinates)
    {
        var position = new Vector2(pointXCoordinates, transform.position.y);
        var currentPosition = rb.position;
        Vector2 newPosition = Vector2.MoveTowards(currentPosition, position, enemyMoveSpeed * Time.deltaTime);
        rb.MovePosition(newPosition);
    }

    private void TakeDamage(int damage)
    {
        _monstersData.Health -= damage;
        if (_monstersData.Health <= 0 )
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        Destroy(gameObject);
    }
}
