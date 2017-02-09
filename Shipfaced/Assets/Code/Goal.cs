using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//This class is used for the goal the players need to reach.
public class Goal : MonoBehaviour
{

    public Text winText;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.transform.parent.GetComponent<SimpleCarController>() != null)
        {
            winText.text = (other.gameObject.transform.parent.GetComponent<SimpleCarController>().nameOfCar + " won!");
            winText.gameObject.SetActive(true);
        }



        Time.timeScale = 0.25f;
    }
}
