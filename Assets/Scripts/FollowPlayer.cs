using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    public Transform player;

    public Vector3 offset;

    [SerializeField] private static FollowPlayer Instance;

    private void Start()
    {
        //if (Instance != null)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}

        //Instance = this;
        //GameObject.DontDestroyOnLoad(this.gameObject);
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    private void LateUpdate()
    {
        if (FindObjectOfType<GameManager>().IsGameOver()) return;
        transform.position = Vector3.Lerp(transform.position, player.position + offset, 0.05f);
    }
}
