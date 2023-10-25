using System.Collections.Generic;
using UnityEngine;

public class BulletPoolManager : MonoBehaviour
{
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private int _poolSize = 10;

    private List<GameObject> _bulletPool;

    private void Start()
    {
        InitializePool();
    }

    private void InitializePool()
    {
        _bulletPool = new List<GameObject>();

        for (int i = 0; i < _poolSize; i++)
        {
            GameObject bullet = Instantiate(_bulletPrefab, transform);
            bullet.SetActive(false);
            _bulletPool.Add(bullet);
        }
    }

    public GameObject GetBullet()
    {
        foreach(var bullet in _bulletPool)
        {
            if (!bullet.activeInHierarchy)
            {
                bullet.SetActive(true);
                return bullet;
            }
        }
        return null;
    }
}
