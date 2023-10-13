using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private float moveSpeed;

    [SerializeField] private GameObject onHitSprite;
    [SerializeField] private Rigidbody2D rb;

    [SerializeField] private int fireRate;

    [SerializeField] private Transform player;

    [SerializeField] private float dist;

    [SerializeField] private bool isDead;
    int extraDamage;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>();
        isDead = false;
        health = Random.Range(75, 150);
    }

    private void Update()
    {
        if (FindObjectOfType<GameManager>().IsGameOver()) return;

        Vector3 dir = (player.position - transform.position);
        dist = dir.magnitude;
        dir = dir.normalized;

        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.eulerAngles = new Vector3(0, 0, angle - 90);

        if (!FindObjectOfType<PauseMenu>().isGamePaused && dist > 8f)
        {
            moveSpeed = Random.Range(20, 30);
            rb.MovePosition(transform.position + dir * moveSpeed * Time.deltaTime);
        }

        if (health <= 0)
        {
            isDead = true;
            extraDamage = health;
            StartCoroutine(DestryGO(0.05f));
        }

    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        Instantiate(onHitSprite, transform.position, Quaternion.identity);
        if (!isDead)
        {
            FindObjectOfType<GameManager>().totalDamageGiven += damage;
        }
        //tookDamage = true;
    }

    IEnumerator DestryGO(float delay)
    {
        yield return new WaitForSeconds(delay);
        FindObjectOfType<GameManager>().totalDamageGiven += extraDamage;
        FindObjectOfType<GameManager>().enemiesKilled += 1;
        Destroy(gameObject);
    }
}
