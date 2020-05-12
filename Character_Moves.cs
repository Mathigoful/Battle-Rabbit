using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.UI;

public class Character_Moves : MonoBehaviour {

    private Rigidbody2D m_rigidBody2D;
    private SpriteRenderer _mySpriteRenderer;
    public SpriteRenderer _bulletToLeftR;

    private Animation anim;
    private Animator m_Animator;

    public LayerMask m_whatisGround;
    public Transform _groundCheck;
    public Transform _ceilingCheck;
    const float k_GroundedRadius = .2f;
    const float k_CeilingRadius = .2f;

    public float _moveSpeed;
    public float sprintspeed;
    public float _decelerateSpeed;

    private float currentSpeed;
    private float maxSpeed;

    public float _baseJump;
    public float _maintainJump;
    public float _highJump;
    public float _downCharge;

    private bool _inAir = false;
    private bool _onGround;
    private bool _crouching;
    private bool _downChargeR = false;
    public bool _planingR = false;
    public bool _isJumping = false;
    public bool _canJump = true;
    public bool _canMove = true;
    public bool _canCrouch = true;

    private bool m_facingRight = true;
    private bool m_facingLeft = false;
    private bool m_facingTop = false;
    private bool m_isDown = false;
    //private bool _firingTop = false;

    public GameObject _player;
    public GameObject _muzzle;
    public GameObject _muzzle2;
    public GameObject _muzzle3;
    public GameObject _Camera;

    public enum _BulletType { Basic, Rebound1, Rebound2};
    public _BulletType _bulletType;

    public int _Rebound1Bullets;
    public int _Rebound2Bullets;

    public int _maxRebound1Bullets;
    public int _maxRebound2Bullets;

    public float _fireForce;

    public GameObject _bulletToLeft;
    public GameObject _bulletToRight;
    public GameObject _bulletToTop;

    public GameObject _bulletR1ToTop;
    public GameObject _bulletR1ToLeft;
    public GameObject _bulletR1ToRight;

    public GameObject _bulletR2ToTop;
    public GameObject _bulletR2ToLeft;
    public GameObject _bulletR2ToRight;


    Vector2 _bulletPos;
    float _nextFire = 0.0f;

    public int _lifePoints;
    public int _maxLifePoints;
    public static int _hearts;
    public int _maxHearts = 5;
    public int numOfHearts;
    public Image[] hearts;
    public Sprite _fullHearts;
    public Sprite _emptyHearts;
    private bool _invincibility = false;

    public UnityEvent OnLandEvent;

    public PlayerPos _scriptPos;

	// Use this for initialization
	void Start () {

        m_rigidBody2D = GetComponent<Rigidbody2D>();
        _mySpriteRenderer = GetComponent<SpriteRenderer>();
        m_Animator = _Camera.GetComponent<Animator>();

        _lifePoints = _maxLifePoints;
        _hearts = _maxHearts;

        m_rigidBody2D.gravityScale = 1f;

    }
	
	// Update is called once per frame
	void Update () {
        MoveX();
        GetDown();
        Sprint();
        BasicJump();
        MaintainJump();
        //HighJump();
        MoveCam();
        Death(_lifePoints);
        Respawn(_hearts);
        ChangeAmmo();

        if (_hearts > numOfHearts)
        {
            _hearts = numOfHearts;
        }

        for (int i = 0; i <hearts.Length; i++){

            if(i < _hearts)
            {
                hearts[i].sprite = _fullHearts;
            }
            else
            {
                hearts[i].sprite = _emptyHearts;
            }


            if(i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                hearts[i].enabled = false;
            }

        }
    }

    private void FixedUpdate()
    {
        bool wasGrounded = _onGround;
        _onGround = false;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(_groundCheck.position, k_GroundedRadius, m_whatisGround);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                _onGround = true;
                Debug.Log("Grounded");
                if (!wasGrounded)
                    OnLandEvent.Invoke();
            }

        }
    }

    void MoveX()
    {

        if (Input.GetKey(KeyCode.D) && _canMove)
        {
            transform.Translate(Vector2.right * _moveSpeed * Time.deltaTime, Space.World);
            if (_mySpriteRenderer != null)
            {
                _mySpriteRenderer.flipX = false;

                m_facingRight = true;
                m_facingLeft = false;
                m_facingTop = false;
            }
        }

        if (Input.GetMouseButtonDown(0) && Time.time > _nextFire && m_facingRight && !m_facingTop)
        {
            _bulletToLeftR.flipX = false;
            FiretoRight();
        }

        if (Input.GetKey(KeyCode.Q) && _canMove)
        {
            transform.Translate(Vector2.left * _moveSpeed * Time.deltaTime, Space.World);
            if (_mySpriteRenderer != null)
            {
                _mySpriteRenderer.flipX = true;

                m_facingRight = false;
                m_facingLeft = true;
                m_facingTop = false;
            }
        }

        if (Input.GetMouseButtonDown(0) && Time.time > _nextFire && m_facingLeft && !m_facingTop)
        {
            _bulletToLeftR.flipX = true;
            FiretoLeft();
        }

        if (Input.GetKey(KeyCode.Z)){

            m_facingRight = false;
            m_facingLeft = false;
            m_facingTop = true;
        }

        if (Input.GetMouseButtonDown(0) && Time.time > _nextFire && m_facingTop)
        {
                FireUp();
        }


        if (Input.GetKeyUp(KeyCode.Z))
        {
            m_facingTop = false;
        }

    }

    public void GetDown()
    {
        _muzzle.transform.position = transform.TransformPoint(0.258f, 0, 0);

        if (Input.GetKey(KeyCode.S) && m_facingRight && _canCrouch)
        {
            _muzzle.transform.position = transform.TransformPoint(0.258f, -0.165f, 0);
            Debug.Log("Muzzle Move");
        }

        _muzzle2.transform.position = transform.TransformPoint(-0.258f, 0, 0);

        if (Input.GetKey(KeyCode.S) && !m_facingRight && _canCrouch)
        {
            _muzzle2.transform.position = transform.TransformPoint(-0.258f, -0.165f, 0);
        }

        if (Input.GetKey(KeyCode.S) && _canCrouch)
        {
            m_isDown = true;
            Debug.Log("IsDown");

            /*if (Input.GetKey(KeyCode.Space) && m_isDown)
            {
                m_isDown = false;
                Debug.Log("IsnotDown");
                MaintainJump();
            }*/
        }
        else
        {
            m_isDown = false;
            Debug.Log("IsNotDown");
        }

    }

    void ChangeAmmo()
    {
        if (Input.GetKeyDown(KeyCode.E) && _Rebound1Bullets <= 1)
        {
            Debug.Log("Change Ammo BRebound");
            _bulletType = _BulletType.Rebound1;
        }
    }
    
    void FireUp()
    {
        _muzzle3.transform.position = transform.TransformPoint(0, 0.23f, 0);

        if (_bulletType == _BulletType.Basic)
        {
            Instantiate(_bulletToTop, _muzzle3.transform.position, transform.rotation);
        }

        if (_bulletType == _BulletType.Rebound1)
        {
            if (_Rebound1Bullets > 0)
            {
                StartCoroutine("FireRebound1Top");
            }
           
        }
        if (_Rebound1Bullets <= 0)
        {
            _bulletType = _BulletType.Basic;
        }

        if (_bulletType == _BulletType.Rebound2)
        {
            if (_Rebound2Bullets > 0)
            {
                StartCoroutine("FireRebound2Top");
            }
        }
        if (_Rebound2Bullets <= 0)
        {
            _bulletType = _BulletType.Basic;
        }
    }

    IEnumerator FireRebound1Top()
    {
        Instantiate(_bulletR1ToTop, _muzzle3.transform.position, transform.rotation);
        _Rebound1Bullets -= 1;

        yield return new WaitForSeconds(0);

        if (_Rebound1Bullets > 0)
            StartCoroutine("FireRebound1Top");
        else
        {
            _bulletType = _BulletType.Basic;
            StopCoroutine("FireRebound1Top");
        }
    }

    IEnumerator FireRebound2Top()
    {
        Instantiate(_bulletR2ToTop, _muzzle3.transform.position, transform.rotation);
        _Rebound2Bullets -= 1;

        yield return new WaitForSeconds(0);

        if (_Rebound2Bullets > 0)
            StartCoroutine("FireRebound2Top");
        else
        {
            _bulletType = _BulletType.Basic;
            StopCoroutine("FireRebound2Top");
        }
    }

    void MoveCam()
    {
        if (m_facingTop && !_inAir)
        {
            m_Animator.SetBool("_LookUp", true);
        }

        if (!m_facingTop && !_inAir)
        {
            m_Animator.SetBool("_LookUp", false);
        }

        if (m_isDown && !_inAir)
        {
            m_Animator.SetBool("_LookDown", true);
        }

        if (!m_isDown && !_inAir)
        {
            m_Animator.SetBool("_LookDown", false);
        }
    }

    void Sprint()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            _moveSpeed *= 2.0f;
        }
        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
            _moveSpeed /= 2.0f;
        }
    }

    public void BasicJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _onGround && !m_isDown && !_inAir && _canJump)
        {
            m_rigidBody2D.AddForce(new Vector3(0f, _baseJump));
            Debug.Log("InAirTrue1");
            _inAir = true;
            Debug.Log("IsJumpingTrue");
            _isJumping = true;
            //Debug.Log("BasicJump");
            /*if (_inAir)
            {
                Debug.Log("InAirPossibilitesONE");
                DownChargeR();

                DownChargeD();

                //Planing();
            }*/

        }
        else
        {
            Debug.Log("InAirFalse1");
            _inAir = false;
            Debug.Log("IsJumpingFalse");
            _isJumping = false;
        }
    }
    
    public void MaintainJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && _onGround && m_isDown && !_inAir && _canJump)
        {
            m_rigidBody2D.AddForce(new Vector3(0f, _maintainJump));
            Debug.Log("InAirTrue2");
            _inAir = true;
            Debug.Log("IsJumpingTrue");
            _isJumping = true;
            //Debug.Log("MaintainJump");
            /*if (_inAir)
            {
                Debug.Log("InAirPossibilitiesTWO");
                DownChargeR();

                DownChargeD();

                //Planing();
            }*/
        }
        else
        {
            Debug.Log("InAirFalse2");
            _inAir = false;
        }

    }

    /*void HighJump()
    {
        if (Input.GetKey(KeyCode.Space) && _onGround)
        {
            m_rigidBody2D.AddForce(new Vector3(0f, _highJump));
            _inAir = true;
            Debug.Log("High Jump");
        }
        else
        {
            _inAir = false;
        }
    }*/

    void Planing()
    {

        if (Input.GetKeyDown(KeyCode.Space) && !m_isDown && _inAir && _isJumping)
        {
            Debug.Log("PlaningR");
            _planingR = true;

            if (_planingR)
            {
                
                Debug.Log("Planing");
                StartCoroutine("PlaningP");
            }
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            _planingR = false;
            StopCoroutine("PlaningP");
        }
    }

    IEnumerator PlaningP()
    {
        Debug.Log("GravityScale");
        m_rigidBody2D.gravityScale = 3f;
        yield return new WaitForSeconds(2);
        m_rigidBody2D.gravityScale = 1f;
    }

    void FiretoRight()
    {
        if (_bulletType == _BulletType.Basic)
        {
            Instantiate(_bulletToRight, _muzzle.transform.position, transform.rotation);
        }

        if (_bulletType == _BulletType.Rebound1)
        {
            if (_Rebound1Bullets > 0)
            {
                StartCoroutine("FireRebound1R");
            }
        }
        if (_Rebound1Bullets <= 0)
        {
            _bulletType = _BulletType.Basic;
        }

        if (_bulletType == _BulletType.Rebound2)
        {
            if (_Rebound2Bullets > 0)
            {
                StartCoroutine("FireRebound2R");
            }
        }
        if (_Rebound2Bullets <= 0)
        {
            _bulletType = _BulletType.Basic;
        }
    }

    IEnumerator FireRebound1R()
    {
        Instantiate(_bulletR1ToRight, _muzzle3.transform.position, transform.rotation);
        _Rebound1Bullets -= 1;

        yield return new WaitForSeconds(0);

        if (_Rebound1Bullets > 0)
            StartCoroutine("FireRebound1R");
        else
        {
            _bulletType = _BulletType.Basic;
            StopCoroutine("FireRebound1R");
        }
    }

    IEnumerator FireRebound2R()
    {
        Instantiate(_bulletR2ToRight, _muzzle3.transform.position, transform.rotation);
        _Rebound2Bullets -= 1;

        yield return new WaitForSeconds(0);

        if (_Rebound2Bullets > 0)
            StartCoroutine("FireRebound2R");
        else
        {
            _bulletType = _BulletType.Basic;
            StopCoroutine("FireRebound2R");
        }
    }

    void FiretoLeft()
    {
        if (_bulletType == _BulletType.Basic)
        {
            Instantiate(_bulletToLeft, _muzzle2.transform.position, transform.rotation);
        }

        if (_bulletType == _BulletType.Rebound1)
        {
            if (_Rebound1Bullets > 0)
            {
                StartCoroutine("FireRebound1L");
            }
        }
        if (_Rebound1Bullets <= 0)
        {
            _bulletType = _BulletType.Basic;
        }

        if (_bulletType == _BulletType.Rebound2)
        {
            if (_Rebound2Bullets > 0)
            {
                StartCoroutine("FireRebound2L");
            }
        }
        if (_Rebound2Bullets <= 0)
        {
            _bulletType = _BulletType.Basic;
        }
    }

    IEnumerator FireRebound1L()
    {
        Instantiate(_bulletR1ToLeft, _muzzle3.transform.position, transform.rotation);
        _Rebound1Bullets -= 1;

        yield return new WaitForSeconds(0);

        if (_Rebound1Bullets > 0)
            StartCoroutine("FireRebound1L");
        else
        {
            _bulletType = _BulletType.Basic;
            StopCoroutine("FireRebound1L");
        }
    }

    IEnumerator FireRebound2L()
    {
        Instantiate(_bulletR2ToLeft, _muzzle3.transform.position, transform.rotation);
        _Rebound1Bullets -= 1;

        yield return new WaitForSeconds(0);

        if (_Rebound2Bullets > 0)
            StartCoroutine("FireRebound2L");
        else
        {
            _bulletType = _BulletType.Basic;
            StopCoroutine("FireRebound2L");
        }
    }

    void Death(int _lifePoints)
    {
        if (_lifePoints <= 0)
        {
            SceneManager.LoadScene("SampleScene");
        }
    }

     void Respawn(int _hearths)
    {
        if (_hearths <= 0)
        {
            _lifePoints--;
            _hearts = _maxHearts;
            _scriptPos.ReturnCheck();
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag == "Carrot" && _hearts != _maxHearts)
        {
            _hearts++;
            Debug.Log("Add1H"); 
            Invincibility();
        }
        if (collision.transform.tag == "Bullet" && !_invincibility)
        {
            _hearts--;
            Debug.Log("Lose1H");
            Invincibility();
        }
    }

    void Invincibility()
    {
        StartCoroutine(Invincibility1());
    }

    IEnumerator Invincibility1()
    {
        _invincibility = true;
        Debug.Log("INVINCIBLE");
        yield return new WaitForSeconds(3);
        _invincibility = false;
        Debug.Log("NOTINVINCIBLE");
    }

    void DownChargeR()
    {
        Debug.Log("1");
        StartCoroutine(DownCharge1());
    }

    IEnumerator DownCharge1()
    {
        Debug.Log("2");
        if (_inAir)
        {
            _downChargeR = true;
            _inAir = true;
            Debug.Log("DownCharge Ready");
            yield return new WaitForSeconds(2.0f);
            Debug.Log("Down Charge NoReady");
            _downChargeR = false;
            _inAir = false;
        }
    }

    void DownChargeD()
    {
        StartCoroutine(DownCharge2());
    }

    IEnumerator DownCharge2()
    {
        Debug.Log("3");
        while(_downChargeR == false)
        {
            Debug.Log("4");
            yield return null;
        }
        Debug.Log("5");
        if (_downChargeR && Input.GetKey(KeyCode.S))
        {
            Debug.Log("Down Charge !");
            m_rigidBody2D.AddForce(new Vector3(0f, _downCharge));
        }
        else
        {
            Debug.Log("Down Charge NoR2");
            _downChargeR = false;
        }
    }

}
