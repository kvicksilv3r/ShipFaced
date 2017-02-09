using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyboardManager : MonoBehaviour
{
    public List<KeyCode> keys = new List<KeyCode>();
    public GameObject globalGameObject;

    void Awake()
    {
        globalGameObject = GameObject.Find("GlobalGameObject");
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
        globalGameObject.GetComponent<GlobalGameObject>().keys.Clear();
        foreach (KeyCode key in keys)
        {
            globalGameObject.GetComponent<GlobalGameObject>().keys.Add(key);
        }
    }


}
