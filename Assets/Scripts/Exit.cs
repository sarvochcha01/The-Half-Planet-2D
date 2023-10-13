using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            GameManager gm = FindObjectOfType<GameManager>();
            gm.SetIsGameOver(true);
            gm.status = "Success";
            FindObjectOfType<UINotificationManager>().SetText("You Escaped the facility");
            StartCoroutine(FindObjectOfType<GameManager>().GameFinished());
        }
    }
}
