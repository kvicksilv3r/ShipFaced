using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalGameObject : MonoBehaviour
{
    public List<KeyCode> keys = new List<KeyCode>();

    void Awake()
    {
        DontDestroyOnLoad(transform.gameObject);
    }

}
