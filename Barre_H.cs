using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barre_H : MonoBehaviour {

    public Character_Moves _player;
    public B_Jumper jumper;
    public Rigidbody2D _pRigidBody;
    public GameObject _character;

    public float _barreForce;

    // Use this for initialization
    void Start()
    {

        _pRigidBody = _character.GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            _pRigidBody.velocity = Vector3.zero;
            //_pRigidBody.angularVelocity = Vector3.zero;
            StartCoroutine("BarreSpeed");
        }
    }

    IEnumerator BarreSpeed()
    {
        Debug.Log("Stop MoveP");
        jumper.enabled = false;
        yield return new WaitForSeconds(1);
        Debug.Log("Wait Barre");
        _pRigidBody.AddForce(new Vector3(_barreForce, 0f));
        Debug.Log("Jumper Activated !");
        jumper.enabled = true;
        StopCoroutine("BarreSpeed");
    }
}
