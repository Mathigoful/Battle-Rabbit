using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B_Jumper : MonoBehaviour {

    public Movements_P1 _player;
    public Rigidbody2D _pRigidBody;
    public GameObject _character;

    public Movements _player2;
    public Rigidbody2D _pRigidBody2;
    public GameObject _character2;

    public float _jumpersForce;

    // Use this for initialization
    void Start()
    { 
        _player = GameObject.Find("Player").GetComponent<Movements_P1>();
        _pRigidBody = _character.GetComponent<Rigidbody2D>();

        _player2 = GameObject.Find("Player2").GetComponent<Movements>();
        _pRigidBody2 = _character2.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player")
        {
            Debug.Log("Stop MoveP");
            _pRigidBody.AddForce(new Vector3(_jumpersForce, 0f));
            Debug.Log("Jumper Activated !");
        }

        if (collision.transform.tag == "Player1")
        {
            Debug.Log("Stop MoveP");
            _pRigidBody2.AddForce(new Vector3(_jumpersForce, 0f));
            Debug.Log("Jumper Activated !");
        }
    }
}
