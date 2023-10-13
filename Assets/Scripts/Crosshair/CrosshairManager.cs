using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrosshairManager : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Transform crosshairTransform;

    [SerializeField] private SpriteRenderer cursorSprite;

    [SerializeField] private ResizeCrosshair resizeCrosshair;
    [SerializeField] private PlayerMoevement playerMoevement;
    [SerializeField] private RaycastBelow raycastBelow;

    [SerializeField] private Sprite shootingCrosshair;
    [SerializeField] private Sprite shootingCrosshairFar;
    [SerializeField] private Sprite pickUpCrosshairSprite;
    [SerializeField] private Sprite pickedUpCrosshairSprite;

    [SerializeField] private bool onPickableObject;

    [SerializeField] private static CrosshairManager Instance;

    [SerializeField] private bool isPlayerLocated;
    public bool isItemEquipped;

    // Start is called before the first frame update
    void Start()
    {
        //if (Instance != null)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}

        //Instance = this;
        //GameObject.DontDestroyOnLoad(this.gameObject);

        Cursor.visible = false;
        isPlayerLocated = false;

        resizeCrosshair = FindObjectOfType<ResizeCrosshair>();
        raycastBelow = FindObjectOfType<RaycastBelow>();

    }

    private void Update()
    {
        if (FindObjectOfType<GameManager>().IsGameOver()) return;

        if (!isPlayerLocated)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
            playerMoevement = FindObjectOfType<PlayerMoevement>();
            isPlayerLocated = true;
        }

        if (onPickableObject && !isItemEquipped)
        {
            resizeCrosshair.DecreaseSize(0.3f);

            if (Input.GetMouseButton(0))
            {
                cursorSprite.sprite = pickedUpCrosshairSprite;
            }
            else cursorSprite.sprite = pickUpCrosshairSprite;
        }
        else if (!onPickableObject)
        {
            if (playerMoevement.IsMoving())
            {
                cursorSprite.sprite = shootingCrosshairFar;
                IncreaseCursorSize(12f);

            }
            else if (GameObject.FindGameObjectWithTag("GunHolder").transform.childCount > 0)
            {
                if (GameObject.FindGameObjectWithTag("GunHolder").transform.GetChild(0).GetComponent<Shooting>().IsShooting())
                {
                    cursorSprite.sprite = shootingCrosshairFar;
                    IncreaseCursorSize(15f);
                }
                else
                {
                    cursorSprite.sprite = shootingCrosshair;
                    DecreaseCursorSize();
                }
            }
            else
            {
                cursorSprite.sprite = shootingCrosshair;
                DecreaseCursorSize();
            }
        }
    }
    


    public void IncreaseCursorSize(float factor)
    {
        if (FindObjectOfType<GameManager>().IsGameOver()) return;
        float dist = (player.position - crosshairTransform.position).magnitude;

        resizeCrosshair.IncreaseSize(dist, factor);
    }

    public void DecreaseCursorSize()
    {
        resizeCrosshair.DecreaseSize();
        
    }

    public Transform GetCrosshairTransform()
    {
        return crosshairTransform;
    }

    public SpriteRenderer GetCrosshairSprite()
    {
        return cursorSprite;
    }

    public bool GetOnPickableObjectBool()
    {
        return onPickableObject;
    }

    public void SetOnPickableObjectBool(bool value)
    {
        onPickableObject = value;
    }

    public void ChangeSpriteTransparency(float alpha)
    {
        cursorSprite.color = new Color(cursorSprite.color.r, cursorSprite.color.g, cursorSprite.color.b, alpha);
    }

    //public void SetIsItemEquipped(bool value)
    //{
    //    isItemEquipped = value;
    //}
}