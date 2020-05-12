using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Jump : MonoBehaviour {

    public Rigidbody2D rb;

    public float _jumpForce;

    public bool _onGround2;

    public bool _isJumping2;
    public Transform _groundCheck;
    const float k_GroundedRadius = .4f;
    const float k_CeilingRadius = .2f;
    public LayerMask m_whatisGround;

    public UnityEvent OnLandEvent;

    // Use this for initialization
    void Start() {

    }

    private void FixedUpdate()
    {
        bool wasGrounded = _onGround2;
        _onGround2 = false;
        Debug.Log("GroundedFalse2");

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, k_GroundedRadius, m_whatisGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                _onGround2 = true;
                _isJumping2 = false;
                Debug.Log("Grounded2");
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }


        }

        // Update is called once per frame

    }
    void Update()
    {

        if (Input.GetButton("XButton") && !_isJumping2)
        {
            _isJumping2 = true;
            rb.velocity = Vector2.up * _jumpForce;
        }

    }
}
