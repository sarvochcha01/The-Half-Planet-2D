using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Hover : MonoBehaviour, IPointerEnterHandler, IPointerDownHandler
{
    [SerializeField] private UIAudioManager uiAudioManager;

    private void Update()
    {
        if (uiAudioManager == null)
            uiAudioManager = FindObjectOfType<UIAudioManager>();

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        uiAudioManager.Play("Hover");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        uiAudioManager.Play("Click");
    }

}
