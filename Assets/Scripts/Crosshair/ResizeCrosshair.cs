using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResizeCrosshair : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private float defaultScale;
    [SerializeField] private float resizeRate;

    [SerializeField] private static ResizeCrosshair Instance;

    private void Start()
    {
        //if (Instance != null)
        //{
        //    Destroy(this.gameObject);
        //    return;
        //}

        //Instance = this;
        //GameObject.DontDestroyOnLoad(this.gameObject);

        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void IncreaseSize(float dist, float factor)
    {
        if (dist >= 2f) transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * dist / factor, resizeRate);
    }

    public void DecreaseSize()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, Vector3.one * defaultScale, resizeRate);
    }

    public void DecreaseSize(float scale)
    {
        transform.localScale = new Vector3(scale, scale, scale);
    }

    public void DecreaseSizeImmediately()
    {
        transform.localScale = Vector3.one * defaultScale;
    }

}
