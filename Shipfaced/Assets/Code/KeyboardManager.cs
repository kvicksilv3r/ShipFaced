using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
    public List<KeyCode> keys = new List<KeyCode>();

    void Start()
    {
        UpdateKeys();
    }

    public void UpdateKeys()
    {
        keys.Clear();
        foreach (UnityEngine.UI.Toggle toggle in GetComponentsInChildren<UnityEngine.UI.Toggle>())
        {
            if (toggle.isOn)
            {
                keys.Add(toggle.gameObject.GetComponent<Key>().keyCode);
            }
        }
    }

    
}
