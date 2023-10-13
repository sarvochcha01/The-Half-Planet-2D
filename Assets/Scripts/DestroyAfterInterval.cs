using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterInterval : MonoBehaviour
{
    [SerializeField] private Vector3 spawnPos;
    [SerializeField] public Vector3 spawnScale;

    [SerializeField] private float aliveTime;
    [SerializeField] private float timer;

    [SerializeField] private bool animate;
    // Update is called once per frame

    private void Start()
    {
        spawnPos = transform.position;
        spawnScale = transform.localScale;  
    }

    void Update()
    {
        Vector3.Lerp(transform.position, transform.position + new Vector3(0, 2, 0), 0.0012f);

        timer += Time.deltaTime;

        if (timer > aliveTime)
        {
            Destroy(gameObject);
        }

        if (animate)
        {
            transform.position = Vector3.Lerp(transform.position, spawnPos + new Vector3(0, 1, 0), 0.0125f);
            transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(0.15f, 0.1f, 1f), 0.0125f);
        }
    }
}
