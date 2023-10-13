using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private int attackRadius;

    [SerializeField] private GameObject trail;

    [SerializeField] private int damage;

    [SerializeField] private float fireRate;
    [SerializeField] private float nextTimeToFire = 0f;

    [SerializeField] private float dist;

    [SerializeField] private Transform firePoint;

    [SerializeField] private LayerMask playerMask;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        fireRate = Random.Range(0, 10);
    }

    private void Update()
    {
        if (FindObjectOfType<GameManager>().IsGameOver()) return;

        bool attack = false;
        dist = (player.position - transform.position).magnitude;
        //RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right, attackRadius);
        //if (hit)
        //{
        //    if (hit.collider.CompareTag("Player") || hit.collider.CompareTag("Shield"))
        //    {
        //        attack = true;
        //    }
        //    else attack = false;
        //}
        //else
        //{
            
        //}

        if (dist <= 20f) attack = true;
        else attack = false;

        if (attack && Time.time >= nextTimeToFire)
        {
            nextTimeToFire = Time.time + 1f / fireRate;
            Recoil();
            FindObjectOfType<GunAudioManager>().Play("EnemyShoot");
            Shoot();

        }
    }

    private void Shoot()
    {
        RaycastHit2D hit = Physics2D.Raycast(firePoint.position, firePoint.right, attackRadius, playerMask);

        var trailInst = Instantiate(trail, firePoint.position, firePoint.rotation);

        if (hit)
        {
            PlayerMoevement playerMoevement = hit.transform.GetComponent<PlayerMoevement>();
            Shield shield = hit.transform.GetComponent<Shield>();

            if (playerMoevement != null)
            {
                playerMoevement.TakeDamage(damage);
            }

            if (shield != null)
            {
                shield.TakeDamage(damage);
            }
        }
    }

    private void Recoil()
    {
        Vector2 playerPos = player.position;

        float recoilRadius = 1f;

        float xpos = playerPos.x + Random.Range(-recoilRadius, recoilRadius);
        float ypos = playerPos.y + Random.Range(-recoilRadius, recoilRadius);

        Vector2 randomRecoilPos = new Vector2(xpos, ypos);
        Vector3 dir = (randomRecoilPos - (Vector2)firePoint.position).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        firePoint.eulerAngles = Vector3.Slerp(firePoint.eulerAngles, new Vector3(0, 0, angle), 5f);
    }

    public void SetFireRate(int rate)
    {
        fireRate = rate;
    }
}

