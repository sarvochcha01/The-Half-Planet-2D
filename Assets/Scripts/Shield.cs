using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Shield : MonoBehaviour
{
    public float health; /*{ get; private set; }*/

    //[SerializeField] private Slider shieldHealthSlider;

    //[SerializeField] private bool isSliderFound;

    // Start is called before the first frame update
    void Start()
    { 
        //isSliderFound = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (FindObjectOfType<GameManager>().IsGameOver())
        { 
            return;
        }

        //if (!isSliderFound)
        //{
        //    shieldHealthSlider = GameObject.FindGameObjectWithTag("ShieldHealthSlider").GetComponent<Slider>();
        //    shieldHealthSlider.maxValue = health;
        //    isSliderFound = true;
        //}

        if (GetComponent<Pickup>().IsEquipped())
        {
            //shieldHealthSlider.value = health;
            if (health <= 0)
            {
                FindObjectOfType<UINotificationManager>().SetText("Shield Destroyed");
                StartCoroutine(DestroyShield());
            }
        }
        else
        { 
            //shieldHealthSlider.value = 0; 

        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        FindObjectOfType<GameManager>().totalDamageTaken += damage;
    }

    IEnumerator DestroyShield()
    {
        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }
}
