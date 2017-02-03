using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpManager powerUpManager;

    void Start()
    {
        powerUpManager = GameObject.Find("PowerUpManager").GetComponent<PowerUpManager>();
    }

    void OnTriggerEnter(Collider c)
    {
        for (int i = 0; i < powerUpManager.playerManager.GetComponent<PlayerManager>().players.Length; i++)
        {
            if (c.gameObject.transform.parent.parent.gameObject.GetComponent<SimpleCarController>() == powerUpManager.playerManager.GetComponent<PlayerManager>().players[i])
            {
                powerUpManager.activatingPlayer = i;
                break;
            }
        }
        Destroy(gameObject);
        powerUpManager.powerUps[Random.Range(0, powerUpManager.powerUps.Count)]();
    }

}
