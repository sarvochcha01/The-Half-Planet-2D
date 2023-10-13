using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamTarget : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private Transform target;
    [SerializeField] private float threshold;

    private void Start()
    {
        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<GameManager>().IsGameOver()) return;

        Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

        Vector2 targetPos = (new Vector2(target.position.x, target.position.y)  + mousePos)/2;

        targetPos.x = Mathf.Clamp(targetPos.x, -threshold + target.position.x, threshold + target.position.x);
        targetPos.y = Mathf.Clamp(targetPos.y, -threshold + target.position.y, threshold + target.position.y);

        transform.position = targetPos;
    }
}
