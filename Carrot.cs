using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Carrot : MonoBehaviour
{

    public int heartsP;

    private Rigidbody2D c_rigidbody2D;

    // Use this for initialization
    void Start()
    {
        c_rigidbody2D = GetComponent<Rigidbody2D>();
        GameObject.Find("Player").GetComponent<Character_Moves>();
        heartsP = Character_Moves._hearts;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == "Player" && (heartsP < 5))
        {
            Debug.Log("DESTROYABLE CARROTS");
            Destroy();
        }
        if (collision.transform.tag == "Bullet")
        {
            StartCoroutine("Fall");
        }
    }

    IEnumerator Fall()
    {
        c_rigidbody2D.constraints = RigidbodyConstraints2D.None;
        yield return new WaitForSeconds(0.3f);
        c_rigidbody2D.constraints = RigidbodyConstraints2D.FreezePositionY;
    }

    private void Destroy()
    {
        Destroy(this.gameObject);
    }
}
