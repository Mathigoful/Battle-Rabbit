using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster2 : MonoBehaviour {

    private static GameMaster2 instance;
    public Vector2 lastCheckPointPos2;

    // Use this for initialization
    void Start()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
