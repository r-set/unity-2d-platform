using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] public float _bulletSpeed = 10f;

    private void Update()
    {
        transform.Translate(Vector2.right * _bulletSpeed * Time.deltaTime);
    }

    public void OnBecameInvisible()
    {
        gameObject.SetActive(false);
    }
}
