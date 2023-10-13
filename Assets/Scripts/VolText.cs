using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class VolText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI volText;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private enum Category
    {
        gun,
        ui,
        bgm
    }

    [SerializeField] private Category category; 

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        volText = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        switch (category)
        {
            case Category.gun:
                volText.text = gameManager.gunVol.ToString();
                break; 
            case Category.ui:
                volText.text = gameManager.uiVol.ToString();
                break; 
            case Category.bgm:
                volText.text = gameManager.bgmVol.ToString();
                break;


        }
    }
}
