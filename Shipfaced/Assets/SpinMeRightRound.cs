﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinMeRightRound : MonoBehaviour {

	public float rotSpeed;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles + new Vector3(0, 0, rotSpeed));
	}
}
