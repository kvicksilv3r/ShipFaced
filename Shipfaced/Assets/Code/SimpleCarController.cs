using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//This code is taken from <https://docs.unity3d.com/Manual/WheelColliderTutorial.html>


[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
    
}

public class SimpleCarController : MonoBehaviour
{
    public string nameOfCar;
    public int leftOrRight;
    public KeyCode leftKey;
    public KeyCode rightKey;
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public float maxBreakTorque;
    float motor;
    PlayerManager pM;


    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
    }

    private void Awake()
    {
        pM = PlayerManager.instance;
    }

    private void Start()
    {
        nameOfCar = "";
        motor = maxMotorTorque;
    }

    public void FixedUpdate()
    {
        if (Input.GetKey(rightKey))
            leftOrRight = 1;
        else if (Input.GetKey(leftKey))
            leftOrRight = -1;
        else
            leftOrRight = 0;



        float steering = maxSteeringAngle * leftOrRight;


        foreach (AxleInfo axleInfo in axleInfos)
        {

            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }

            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    public void ShuffleKeys()
    {
        if (leftKey != KeyCode.None || rightKey != KeyCode.None)
        {
            int temp = Random.Range(0, pM.remainingKeys.Count);
            leftKey = pM.remainingKeys[temp];
            pM.remainingKeys.RemoveAt(temp);
            temp = Random.Range(0, pM.remainingKeys.Count);
            rightKey = pM.remainingKeys[temp];
            pM.remainingKeys.RemoveAt(temp);
        }
        else
        {
            KeyCode temp1 = leftKey, temp2 = rightKey;
            int temp = Random.Range(0, pM.remainingKeys.Count);
            leftKey = pM.remainingKeys[temp];
            pM.remainingKeys.RemoveAt(temp);
            temp = Random.Range(0, pM.remainingKeys.Count);
            rightKey = pM.remainingKeys[temp];
            pM.remainingKeys.RemoveAt(temp);
            pM.remainingKeys.Add(temp1);
            pM.remainingKeys.Add(temp2);
        }

    }
}