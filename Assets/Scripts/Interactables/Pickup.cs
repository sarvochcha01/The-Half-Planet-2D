using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    private enum Type
    {
        Gun,
        Shield,
    }

    [SerializeField] private Type type;

    [SerializeField] private CrosshairManager crosshairManager;
    [SerializeField] private UINotificationManager notificationManager;
    [SerializeField] private CanBePicked canBePicked;

    [SerializeField] private Transform holder;

    [SerializeField] private Transform player;

    [SerializeField] private bool isEquipped;
    [SerializeField] private bool isPlayerFound;

    [SerializeField] private Vector3 offset;
    [SerializeField] private Vector3 rotation;

    [SerializeField] private float pickUpRadius;
    [SerializeField] private float dist;

    // Start is called before the first frame update
    void Start()
    {
        isPlayerFound = false;
        crosshairManager = FindObjectOfType<CrosshairManager>();
        notificationManager = FindObjectOfType<UINotificationManager>();

        canBePicked = GetComponent<CanBePicked>();

        switch (type)
        {
            case Type.Gun:
                holder = GameObject.Find("GunHolder").transform;
                break;
            case Type.Shield:
                holder = GameObject.Find("ShieldHolder").transform;
                break;
            default:
                break;
        }
    }

    private void Update()
    {
        if (FindObjectOfType<GameManager>().IsGameOver()) return;

        if (!isPlayerFound)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            isPlayerFound = true;
        }

        dist = (player.position - transform.position).magnitude;

        if (dist > 40f)
        {
            StartCoroutine(DestryGO());
            GetComponentInParent<CollectableSpawner>().SetSpawnBool(false); 
        }

        bool isHolderEmpty = holder.GetComponent<Holder>().GetIsEmpty();

        if (canBePicked.GetOnPickableObject() && !isEquipped)
        {
            if (Input.GetMouseButtonUp(0) && isHolderEmpty && dist <= pickUpRadius)
            {
                transform.SetParent(holder);
                transform.localPosition = offset;
                transform.localEulerAngles = rotation;
                isEquipped = true;
                holder.GetComponent<Holder>().SetIsEmpty(false);
                notificationManager.SetText(type.ToString() + " Equipped");
            }

            if (dist > pickUpRadius)
            {
                if (Input.GetMouseButtonUp(0))
                {
                    notificationManager.SetText("object too far");
                }
                crosshairManager.ChangeSpriteTransparency(0.4f);
            }
            else crosshairManager.ChangeSpriteTransparency(1f);
        }

        if (isEquipped)
        {
            Holder holderScript = holder.GetComponent<Holder>();

            if (Input.GetKeyDown(KeyCode.G) && type == Type.Shield)
            {
                transform.SetParent(null);
                transform.position += offset;
                isEquipped = false;
                holderScript.SetIsEmpty(true);
                notificationManager.SetText("Shield Dropped");
                return;
            }
            if (Input.GetKeyDown(KeyCode.F) && type == Type.Gun && !holder.GetChild(0).GetComponent<Shooting>().IsReloading())
            {
                transform.SetParent(null);
                transform.position += offset;
                isEquipped = false;
                holderScript.SetIsEmpty(true);
                notificationManager.SetText("Gun Dropped");
                return;
            }

            if (type == Type.Gun)
            {
                var gunScript = holder.GetChild(0).GetComponent<Shooting>();
                if (gunScript == null)
                {
                    return;
                }
                if (gunScript.IsReloading() && (Input.GetKeyDown(KeyCode.F)))
                {
                    notificationManager.SetText("Cannot drop while reloading");
                }
            }
        }
    }

    public bool IsEquipped()
    {
        return isEquipped;
    }

    IEnumerator DestryGO()
    {
        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }
}
