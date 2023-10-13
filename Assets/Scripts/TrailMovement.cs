using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailMovement : MonoBehaviour
{
    [SerializeField] private float speed;

    [SerializeField] private float aliveDuration;
    [SerializeField] private float timer;

    private void Start()
    {
        aliveDuration = GetComponent<TrailRenderer>().time;
    }
    // Update is called once per frame
    void Update()
    {
        transform.position += transform.right * speed * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer >= aliveDuration) Destroy(gameObject);
    }
}
