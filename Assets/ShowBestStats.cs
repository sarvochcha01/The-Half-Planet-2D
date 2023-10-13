using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShowBestStats : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private void Start()
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
