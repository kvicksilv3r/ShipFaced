using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public GameObject playerManager;
    public int checkpointID;

    void OnTriggerEnter(Collider c)
    {
        if (!c.gameObject.name.Contains("Boat"))
        {
            return;
        }
        for (int i = 0; i < playerManager.GetComponent<PlayerManager>().players.Length; i++)
        {
            if (c.gameObject.transform.parent.gameObject.GetComponent<SimpleCarController>() == playerManager.GetComponent<PlayerManager>().players[i])
            {
                if (checkpointID + 1 > playerManager.GetComponent<PlayerManager>().checkpoints.Count)
                {
                    break;
                }
                playerManager.GetComponent<PlayerManager>().nextCheckpoint[i] = checkpointID + 1;
                break;
            }
        }
        playerManager.GetComponent<PlayerManager>().CheckPositions();
    }
}
