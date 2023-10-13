using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthKit : MonoBehaviour
{
    [SerializeField] private int health;
    [SerializeField] private int pickupRadius;
    [SerializeField] private CanBePicked canBePicked;
    [SerializeField] private bool picked;
    [SerializeField] private Transform player;

    [SerializeField] private float dist;
    [SerializeField] private UINotificationManager notificationManager;
    [SerializeField] private CrosshairManager crosshairManager;

    private void Start()
    {
        health = Random.Range(30, 100);
        canBePicked = GetComponent<CanBePicked>();
        player = GameObject.FindGameObjectWithTag("Player").transform;
        notificationManager = FindObjectOfType<UINotificationManager>();
        crosshairManager = FindObjectOfType<CrosshairManager>();
        picked = false;
    }

    private void Update()
    {
        if (FindObjectOfType<GameManager>().IsGameOver()) return;
        dist = (player.position - transform.position).magnitude;

        if (dist > 40f)
        {
            StartCoroutine(DestroyGO());
            GetComponentInParent<CollectableSpawner>().SetSpawnBool(false);
        }

        if (canBePicked.GetOnPickableObject() && Input.GetMouseButtonUp(0) && !picked && (transform.position - player.position).magnitude <= pickupRadius)
        {
            picked = true;
            FindObjectOfType<GameManager>().totalHealthHealed += health;
            FindObjectOfType<PlayerMoevement>().AddHealth(health);
            notificationManager.SetText(health + " health gained");
            StartCoroutine(DestroyGO());
        }

        if (dist > pickupRadius)
        {
            if (Input.GetMouseButtonUp(0))
            {
                notificationManager.SetText("object too far");
            }
            crosshairManager.ChangeSpriteTransparency(0.4f);
        }
        else crosshairManager.ChangeSpriteTransparency(1f);

    }

    IEnumerator DestroyGO()
    {
        yield return new WaitForSeconds(0.01f);

        Destroy(gameObject);
    }
}
