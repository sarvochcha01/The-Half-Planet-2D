using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCursor : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private PlayerMoevement playerMoevement;

    private void Start()
    {
        playerMoevement = FindObjectOfType<PlayerMoevement>();
    }

    void FixedUpdate()
    {
        transform.position = Vector2.Lerp(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.7f);

    }
}
