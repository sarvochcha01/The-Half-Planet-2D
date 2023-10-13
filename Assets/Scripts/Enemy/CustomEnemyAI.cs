using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CustomEnemyAI : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] private Vector3 initialPos;

    [SerializeField] private float speed;

    [SerializeField] private float patrolRadius;
    [SerializeField] private bool isWalkPointeSet;
    [SerializeField] private Vector3 walkPoint;

    [SerializeField] private LayerMask obstacleMask;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
        initialPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        Patrol();
    }

    private void Patrol()
    {
        if (!isWalkPointeSet)
        {
            SetWalkPoint();
        }

        if (isWalkPointeSet)
        {
            transform.position = Vector3.MoveTowards(transform.position, walkPoint, speed * Time.deltaTime);
        }

        if ((walkPoint - transform.position).magnitude <= 1f)
        {
            isWalkPointeSet = false;
        }
    }

    private void SetWalkPoint()
    {
        Vector3 tempPoint = GenerateRandomPosition(patrolRadius);

        if (VerifyRandomPosition(tempPoint))
        {
            walkPoint = initialPos + tempPoint;
            isWalkPointeSet = true;
            Debug.DrawLine(transform.position, walkPoint, Color.green);

            return;
        }

        tempPoint = GenerateRandomPosition(patrolRadius);
    }

    private Vector3 GenerateRandomPosition(float range)
    {
        float randomX = Random.Range(-range, range);
        float randomY = Random.Range(-range, range);

        return new Vector3(randomX, randomY, 0);
    }

    private bool VerifyRandomPosition(Vector3 position)
    {
        Vector3 dir = (position - transform.position).normalized;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, 100f, obstacleMask);
        
        Debug.DrawLine(transform.position, hit.point, Color.red);
        if (hit)
        {
            Debug.Log(hit.transform.name);
            return false;
        }

        return true;
    }
}
