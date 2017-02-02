using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public GameObject playerManager;
    List<Action> powerUps = new List<Action>();
    int activatingPlayer;
    void Awake()
    {
        playerManager = GameObject.Find("PlayerManager");
        powerUps.Add(PowerUpShuffleOthers);
        powerUps.Add(PowerUpSwitchOthers);
        // powerUps.Add(PowerUpSlowOthers);
    }

    void OnTriggerEnter(Collider c)
    {
        for (int i = 0; i < playerManager.GetComponent<PlayerManager>().players.Length; i++)
        {
            if (c.gameObject.transform.parent.parent.gameObject.GetComponent<SimpleCarController>() == playerManager.GetComponent<PlayerManager>().players[i])
            {
                activatingPlayer = i;
                break;
            }
        }
        Destroy(gameObject);
        powerUps[UnityEngine.Random.Range(0, powerUps.Count)]();
    }

    //PowerUps
    //UI för PowerUps

    void PowerUpShuffleOthers()
    {
        for (int i = 0; i < playerManager.GetComponent<PlayerManager>().players.Length; i++)
        {
            if (i == activatingPlayer)
            {
                continue;
            }
            playerManager.GetComponent<PlayerManager>().players[i].ShuffleKeys();
            playerManager.GetComponent<PlayerManager>().textList[i].text = "P" + (i + 1) + ": " + playerManager.GetComponent<PlayerManager>().players[i].leftKey.ToString() + " || " + playerManager.GetComponent<PlayerManager>().players[i].rightKey.ToString();
        }
        print("Shuffle!");
    }

    void PowerUpSwitchOthers()
    {
        for (int i = 0; i < playerManager.GetComponent<PlayerManager>().players.Length; i++)
        {
            if (i == activatingPlayer)
            {
                continue;
            }
            KeyCode temp;
            temp = playerManager.GetComponent<PlayerManager>().players[i].leftKey;
            playerManager.GetComponent<PlayerManager>().players[i].leftKey = playerManager.GetComponent<PlayerManager>().players[i].rightKey;
            playerManager.GetComponent<PlayerManager>().players[i].rightKey = temp;
            playerManager.GetComponent<PlayerManager>().textList[i].text = "P" + (i + 1) + ": " + playerManager.GetComponent<PlayerManager>().players[i].leftKey.ToString() + " || " + playerManager.GetComponent<PlayerManager>().players[i].rightKey.ToString();
        }
        print("Switch!");
    }

    void PowerUpSlowOthers()
    {
        for (int i = 0; i < playerManager.GetComponent<PlayerManager>().players.Length; i++)
        {
            if (i == activatingPlayer)
            {
                continue;
            }

            StartCoroutine(SlowDown(playerManager.GetComponent<PlayerManager>().players[i].gameObject.GetComponent<Rigidbody>()));

        }
        print("Slow Down!");
    }

    IEnumerator SlowDown(Rigidbody boat)
    {
        //Slowdown
        yield return new WaitForSeconds(1f);
        //return
    }

}
