using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{

    public GameObject playerManager;
    public List<Action> powerUps = new List<Action>();
    public int activatingPlayer;
    void Awake()
    {
        playerManager = GameObject.Find("PlayerManager");
        // powerUps.Add(PowerUpShuffleOthers);
        // powerUps.Add(PowerUpSwitchLROthers);
        // powerUps.Add(PowerUpSkipShuffle);
        // powerUps.Add(PowerUpDisableOtherTrails);
        // powerUps.Add(PowerUpSwitchControls);
        powerUps.Add(PowerUpStop);
    }

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

    void PowerUpSwitchLROthers()
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

    void PowerUpSkipShuffle()
    {
        for (int i = 0; i < playerManager.GetComponent<PlayerManager>().players.Length; i++)
        {
            if (i == activatingPlayer)
            {
                playerManager.GetComponent<PlayerManager>().skip = true;
                playerManager.GetComponent<PlayerManager>().playerSkip = i;
                break;
            }
        }
        print("Skip Next Shuffle!");
    }

    void PowerUpDisableOtherTrails()
    {
        for (int i = 0; i < playerManager.GetComponent<PlayerManager>().players.Length; i++)
        {
            if (i == activatingPlayer)
            {
                continue;
            }
            StartCoroutine(DisableTrail(playerManager.GetComponent<PlayerManager>().players[i].gameObject.GetComponentInChildren<TrailRenderer>()));
        }
        print("Disable Trails!");
    }

    IEnumerator DisableTrail(TrailRenderer trail)
    {
        trail.enabled = false;
        yield return new WaitForSeconds(3f);
        trail.enabled = true;
    }

    void PowerUpSwitchControls()
    {
        KeyCode lastLeft = KeyCode.None;
        KeyCode lastRight = KeyCode.None;
        KeyCode tempLeft = KeyCode.None;
        KeyCode tempRight = KeyCode.None;
        KeyCode prevLeft = KeyCode.None;
        KeyCode prevRight = KeyCode.None;
        for (int i = 0; i < playerManager.GetComponent<PlayerManager>().players.Length; i++)
        {
            if (i == activatingPlayer)
            {
                continue;
            }
            tempLeft = playerManager.GetComponent<PlayerManager>().players[i].leftKey;
            tempRight = playerManager.GetComponent<PlayerManager>().players[i].rightKey;
            if (i == playerManager.GetComponent<PlayerManager>().players.Length - 1)
            {
                lastLeft = tempLeft;
                lastRight = tempRight;
            }
            if (prevLeft != KeyCode.None && prevRight != KeyCode.None)
            {
                playerManager.GetComponent<PlayerManager>().players[i].leftKey = prevLeft;
                playerManager.GetComponent<PlayerManager>().players[i].rightKey = prevRight;
            }

            prevLeft = tempLeft;
            prevRight = tempRight;
            playerManager.GetComponent<PlayerManager>().textList[i].text = "P" + (i + 1) + ": " + playerManager.GetComponent<PlayerManager>().players[i].leftKey.ToString() + " || " + playerManager.GetComponent<PlayerManager>().players[i].rightKey.ToString();
        }
        int temp;
        if (activatingPlayer != 0)
        {
            temp = 0;
        }
        else
        {
            temp = 1;
        }

        playerManager.GetComponent<PlayerManager>().players[temp].leftKey = lastLeft;
        playerManager.GetComponent<PlayerManager>().players[temp].rightKey = lastRight;
        playerManager.GetComponent<PlayerManager>().textList[temp].text = "P" + (temp + 1) + ": " + playerManager.GetComponent<PlayerManager>().players[temp].leftKey.ToString() + " || " + playerManager.GetComponent<PlayerManager>().players[temp].rightKey.ToString();

        print("Switch Controls with others!");
    }

    void PowerUpStop()
    {
        for (int i = 0; i < playerManager.GetComponent<PlayerManager>().players.Length; i++)
        {
            if (i == activatingPlayer)
            {
                continue;
            }
            StartCoroutine(Stop(playerManager.GetComponent<PlayerManager>().players[i].gameObject));
        }
        print("Stop!");
    }

    IEnumerator Stop(GameObject goToStop)
    {
        foreach (WheelCollider wheel in goToStop.GetComponentsInChildren<WheelCollider>())
        {
            wheel.enabled = false;
        }
        yield return new WaitForSeconds(1.25f);
        foreach (WheelCollider wheel in goToStop.GetComponentsInChildren<WheelCollider>())
        {
            wheel.enabled = true;
        }

    }


}
