using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DDOLCanvas : MonoBehaviour
{
    [SerializeField] private static DDOLCanvas Instance;

    // Start is called before the first frame update
    void Start()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        GameObject.DontDestroyOnLoad(this.gameObject);
    }
}
