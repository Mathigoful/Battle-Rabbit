using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioScript : MonoBehaviour {

    public AudioSource _source;
    public AudioClip _gun;

    public Shooting _shoot;

	// Use this for initialization
	void Start () {

        _source.clip = _gun;
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
