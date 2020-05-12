using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour {

    private GameMaster gm;

    private SpriteRenderer rend;
    private Sprite _checkPoint, _checkPointOpen;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM").GetComponent<GameMaster>();
        rend = GetComponent<SpriteRenderer>();
        _checkPoint = Resources.Load<Sprite>("CheckPoint");
        _checkPointOpen = Resources.Load<Sprite>("CheckPointOpen");
        rend.sprite = _checkPoint;
    }
    // Use this for initialization
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            rend.sprite = _checkPointOpen;
            gm.lastCheckPointPos = transform.position;
            Debug.Log("Checkpointing");
        }
    }
}
