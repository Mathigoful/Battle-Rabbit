using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint_P2 : MonoBehaviour {

    private GameMaster2 gm;

    private SpriteRenderer rend;
    private Sprite _checkPoint, _checkPointOpen;

    void Start()
    {
        gm = GameObject.FindGameObjectWithTag("GM2").GetComponent<GameMaster2>();
        rend = GetComponent<SpriteRenderer>();
        _checkPoint = Resources.Load<Sprite>("CheckPoint");
        _checkPointOpen = Resources.Load<Sprite>("CheckPointOpen");
        rend.sprite = _checkPoint;
    }
    // Use this for initialization
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player1"))
        {
            rend.sprite = _checkPointOpen;
            gm.lastCheckPointPos2 = transform.position;
            Debug.Log("Checkpointing");
        }
    }
}
