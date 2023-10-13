using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CanBePicked : MonoBehaviour
{
    //[SerializeField] private RaycastBelow raycastBelow;

    [SerializeField] private bool onPickableObject;


    public bool GetOnPickableObject()
    { return onPickableObject; }

    public void SetOnPickableObject(bool value)
    {
        onPickableObject= value;
    }
}
