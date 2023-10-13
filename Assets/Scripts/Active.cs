using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Active : MonoBehaviour
{
    [SerializeField] private Transform player;

    [SerializeField] private float timer;
    [SerializeField] private float timeBetweenAction;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void Update()
    {
        if (FindObjectOfType<GameManager>().IsGameOver()) return;

        timer += Time.deltaTime;
        if (timer >= timeBetweenAction)
        {
            float dist = (player.position - transform.position).magnitude;

            if (dist < 50f)
            {
                transform.GetChild(0).gameObject.SetActive(true);
            }

            if (dist > 50f)
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }

            timer = 0;
        }
    }
}
