using System;
using System.Collections.Generic;
using UnityEngine;

public class RaycastBelow : MonoBehaviour
{
    [SerializeField] private LayerMask collectableLayer;
    [SerializeField] private bool onPickableObject;

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity, collectableLayer);
        if (hit.collider != null)
        {
            onPickableObject = true;
            FindObjectOfType<CrosshairManager>().SetOnPickableObjectBool(onPickableObject);
            hit.transform.GetComponent<CanBePicked>().SetOnPickableObject(onPickableObject);
            FindObjectOfType<CrosshairManager>().isItemEquipped = hit.transform.GetComponent<Pickup>().IsEquipped();
        }
        else
        {
            onPickableObject = false;
            FindObjectOfType<CrosshairManager>().SetOnPickableObjectBool(onPickableObject);
            FindObjectOfType<CrosshairManager>().isItemEquipped = false;

            CanBePicked[] canBePickeds = new CanBePicked[10];
            canBePickeds = FindObjectsOfType<CanBePicked>();

            foreach (var cbpScript in canBePickeds)
            {
                cbpScript.SetOnPickableObject(onPickableObject);
            }

        }
    }

}
