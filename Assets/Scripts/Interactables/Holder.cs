using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Holder : MonoBehaviour
{
    [SerializeField] private bool isEmpty;

    public bool GetIsEmpty()
    {
        return isEmpty;
    }

    public void SetIsEmpty(bool value)
    {
        isEmpty = value;
    }
}
