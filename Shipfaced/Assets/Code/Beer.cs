using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beer : MonoBehaviour
{
    public GameObject powerUpManager;
    public GameObject target;

    void Update()
    {
        //GetComponent<Rigidbody>().velocity = powerUpManager.GetComponent<PowerUpManager>().CalculateArc(target, gameObject);
    }
}
