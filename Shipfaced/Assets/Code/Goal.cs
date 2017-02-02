using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Goal : MonoBehaviour {

    public Text winText;

    private void OnTriggerEnter(Collider other)
    {
        winText.gameObject.SetActive(true);
        Time.timeScale = 0.25f;
    }
}
