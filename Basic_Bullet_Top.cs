using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_Bullet_Top : MonoBehaviour {

    float velX = 0f;
    public float velY = 5f;
    Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();

        Destroy(this.gameObject, 2.0f);
    }

    // Update is called once per frame
    void Update()
    {

        rb.velocity = new Vector2(velX, velY);

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Wall")
        {
            Debug.Log("Destroy Bullet");
            Destroy(this.gameObject);
        }

        if (collision.transform.tag == "Player1")
        {
            Debug.Log("Destroy Bullet");
            Destroy(this.gameObject);
        }

    }
}
