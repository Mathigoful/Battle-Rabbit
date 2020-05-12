using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Count : MonoBehaviour {

    public Movements _moves;
    public Movements_P1 _moves1;

    public Shooting _shoot;
    public ShootingP1 _shootP1;


	// Use this for initialization
	void Start () {

        StartCoroutine(Begin());
	}

    IEnumerator Begin()
    {
        _shoot.enabled = false;
        _shootP1.enabled = false;
        _moves.enabled = false;
        _moves1.enabled = false;
        yield return new WaitForSeconds(4);
        FindObjectOfType<AudioM>().Play("Fight");
        _shoot.enabled = true;
        _shootP1.enabled = true;
        _moves.enabled = true;
        _moves1.enabled = true;
    }

}
