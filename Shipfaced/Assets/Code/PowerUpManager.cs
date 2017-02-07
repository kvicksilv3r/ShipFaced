using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class PowerUpManager : MonoBehaviour
{

    public GameObject playerManager;
    public List<Action> powerUps = new List<Action>();
    public int activatingPlayer;

    public GameObject target;
    public GameObject beer;
    public bool beerFlying;

    void Awake()
    {
        playerManager = GameObject.Find("PlayerManager");
        //powerUps.Add(PowerUpShuffleOthers);
        //powerUps.Add(PowerUpSwitchLROthers);
        //powerUps.Add(PowerUpSkipShuffle);
        // powerUps.Add(PowerUpDisableOtherTrails);
        //powerUps.Add(PowerUpSwitchControls);
        //powerUps.Add(PowerUpStop);
        powerUps.Add(PowerUpBeer);
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

    void PowerUpBeer()
    {
        GameObject thrownBeer = Instantiate(beer, playerManager.GetComponent<PlayerManager>().players[activatingPlayer].gameObject.transform.position, playerManager.GetComponent<PlayerManager>().players[activatingPlayer].gameObject.transform.rotation);
        thrownBeer.GetComponent<Rigidbody>().velocity = CalculateArc(target, playerManager.GetComponent<PlayerManager>().players[activatingPlayer].gameObject);
        beerFlying = true;
        StartCoroutine(ReCalculate(thrownBeer, target));
    }
    public Vector3 CalculateArc(GameObject target, GameObject location)
    {
        Vector3 direction = target.transform.position - location.transform.position;
        float height = direction.y;
        height *= 3;
        direction.y = 0;
        float distance = direction.magnitude;
        if (location.transform.position.y >= 15)
        {
            beerFlying = true;
        }
        if (!beerFlying)
        {
            direction.y = distance;
        }
        distance += height;

        float velocity = Mathf.Sqrt(distance * Physics.gravity.magnitude);
        return direction.normalized * velocity;
    }
    IEnumerator ReCalculate(GameObject thrownBeer, GameObject target)
    {
        yield return new WaitForSeconds(.3f);
        while (beerFlying)
        {
            thrownBeer.transform.LookAt(target.transform);
            thrownBeer.GetComponent<Rigidbody>().velocity = CalculateArc(target, thrownBeer);
            yield return new WaitForSeconds(0.2f);
        }
    }
    public void BeerControlsMethod(GameObject target)
    {
        StartCoroutine(BeerControls(target));
    }
    IEnumerator BeerControls(GameObject target)
    {
        foreach (WheelCollider wheel in target.GetComponentsInChildren<WheelCollider>())
        {
            WheelFrictionCurve tempWheel = wheel.forwardFriction;
            tempWheel.stiffness = 200f;
            wheel.forwardFriction = tempWheel;
        }
        yield return new WaitForSeconds(5f);
        foreach (WheelCollider wheel in target.GetComponentsInChildren<WheelCollider>())
        {
            WheelFrictionCurve tempWheel = wheel.forwardFriction;
            tempWheel.stiffness = 85f;
            wheel.forwardFriction = tempWheel;
        }
    }

}


