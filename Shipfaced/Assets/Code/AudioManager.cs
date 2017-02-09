using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

	AudioSource audio;
	[SerializeField]
	AudioClip clip;

	[SerializeField]
	float startTime = 0;
	// Use this for initialization
	void Start () {
		audio = GetComponent<AudioSource>();

		audio.time = startTime;
    }
	
	// Update is called once per frame
	void Update () {

	}

	public void PlayClip()
	{
		audio.clip = clip;
		audio.Play();
	}
}
