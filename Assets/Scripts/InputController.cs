using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputController : MonoBehaviour
{
    Player _player;
    void Start()
    {
        _player = GetComponent<Player>();
    }
    void Update()
    {
        HorizontalPressed();
        JumpPressed();
        Shoot();
    }

    private void HorizontalPressed()
    {
        _player._moveX = Input.GetAxis("Horizontal");
    }

    private void JumpPressed()
    {
        if (Input.GetKeyDown(KeyCode.Space)) 
        { 
            _player._isJump = true;
            return;
        }

        _player._isJump = false;
    }

    private void Shoot()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
            _player._isShoot = true;
            return;
        }

        _player._isShoot = false;
    }
}
