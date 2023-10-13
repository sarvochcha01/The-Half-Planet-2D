using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShieldHealth : MonoBehaviour
{
    [SerializeField] private Slider shieldHealthSlider;

    // Start is called before the first frame update
    void Start()
    {
        shieldHealthSlider = GameObject.FindGameObjectWithTag("ShieldHealthSlider").GetComponent<Slider>();
        shieldHealthSlider.maxValue = 350;
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount > 0)
        {
            shieldHealthSlider.value = GetComponentInChildren<Shield>().health;
        }
        else shieldHealthSlider.value = 0;
    }
}
