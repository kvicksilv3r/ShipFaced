using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Beer : MonoBehaviour
{
    PowerUpManager powerUpManager;
    PlayerManager playerManager;

    List<GameObject> inBeerRadius = new List<GameObject>();

    void Start()
    {
        powerUpManager = GameObject.Find("PowerUpManager").GetComponent<PowerUpManager>();
        playerManager = GameObject.Find("PlayerManager").GetComponent<PlayerManager>();
    }

    void Update()
    {
        if (!powerUpManager.beerFlying)
        {
            //Destroying the game object once it no longer is flying
            Destroy(gameObject);
        }
        if (Vector3.Distance(transform.position, powerUpManager.target.transform.position) < 3f)
        {
            //Beer hit
            for (int i = 0; i < playerManager.players.Length; i++)
            {
                if (Vector3.Distance(playerManager.players[i].gameObject.transform.position, transform.position) < 10f)
                {
                    //Adds Beer effect to all boats within 10 units
                    powerUpManager.BeerControlsMethod(playerManager.players[i].gameObject);
                }
            }
            //Sets the game object to no longer flying as it hit its target
            powerUpManager.beerFlying = false;
        }
        if (transform.position.y <= -5f)
        {
            //If the beer somehow misses the game object will eventually be destroyed
            powerUpManager.beerFlying = false;
        }

    }
}

