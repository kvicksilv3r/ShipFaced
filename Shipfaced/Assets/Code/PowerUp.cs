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
        if (!c.gameObject.name.Contains("Boat"))
        {
            return;
        }
        for (int i = 0; i < powerUpManager.playerManager.GetComponent<PlayerManager>().players.Length; i++)
        {
            if (c.gameObject.transform.parent.gameObject.GetComponent<SimpleCarController>() == powerUpManager.playerManager.GetComponent<PlayerManager>().players[i])
            {
                powerUpManager.activatingPlayer = i;
                powerUpManager.target = FirstPlayer();
                break;
            }
        }
        int temp = Random.Range(0, powerUpManager.powerUps.Count);
        if (temp == 4 && powerUpManager.playerManager.GetComponent<PlayerManager>().players.Length == 2)
        {
            temp = 2;
        }
        powerUpManager.powerUps[temp]();
        Destroy(gameObject);
    }

    GameObject FirstPlayer()
    {
        PlayerManager pM = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();

        GameObject firstPlayer = null;
        pM.CheckPositions();
        firstPlayer = pM.players[pM.playerPositions[0]].gameObject;
        return firstPlayer;
    }
}