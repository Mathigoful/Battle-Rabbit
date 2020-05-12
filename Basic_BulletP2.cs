using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Basic_BulletP2 : MonoBehaviour {

    public float velX = 5f;
    float velY = 0f;
    Rigidbody2D rb;

    // Use this for initialization
    void Start()
    {

        rb = GetComponent<Rigidbody2D>();

        Destroy(this.gameObject, 1.0f);
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

        if (collision.transform.tag == "Player")
        {
            Debug.Log("Destroy Bullet");
            Destroy(this.gameObject);
        }

    }
}
