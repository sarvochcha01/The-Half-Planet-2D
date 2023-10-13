using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damagable : MonoBehaviour
{
    [SerializeField] private int _health;

    public void TakeDamage(int damage)
    {
        _health -= damage;
    }

    public void SetHealth(int health)
    {
        _health = health;
    }

    private void Update()
    {
        if (_health <= 0)
        {
            Destroy(gameObject);
        }
    }
}
