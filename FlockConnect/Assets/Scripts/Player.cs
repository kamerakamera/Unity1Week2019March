using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    GameManeger gameManeger;
    Rigidbody2D rb;
    public int Hp { get; private set; }
    private int maxHp = 5;
    public Camera mainCamera;
    [SerializeField]
    float invincibleCoolTime = 0, deathCount;
    bool isAvoid, avoidAble, avoidInput;
    Vector2 moveDirection;
    Vector3 startPos = Vector2.zero;
    bool right, left, up, down;
    int vertical, horizontal;
    public bool isInvincible, isDeath;
    public float MovePower { get; set; }
    public AudioClip damegeSoundEffect, flocksSoundEffect, deathSoundEffect;
    [SerializeField]
    private GameObject flocksPrefab;
    private List<GameObject> flocks = new List<GameObject>();
    [SerializeField]
    private ScoreManeger scoreManeger;
    [SerializeField]
    Animator animator;
    [SerializeField]
    AudioSource audioSource;//カメラにつけることでPlayerがfalseでも聞こえるようにしよう

    // Use this for initialization
    void Start()
    {
        gameManeger = GameObject.Find("GameManeger").GetComponent<GameManeger>();
        mainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();
        Hp = 1;
        rb = GetComponent<Rigidbody2D>();
        MovePower = 12;
        isDeath = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (isDeath)
        {
            Death();
        }
        if (!isDeath)
        {
            MoveInput();
        }
        if (transform.position.x < mainCamera.ViewportToWorldPoint(Vector2.zero).x - 2.8f)
        {
            Death();
        }
    }

    void FixedUpdate()
    {
        Move();
        //無敵時間
        if (isInvincible)
        {
            invincibleCoolTime += Time.fixedDeltaTime;
            if (invincibleCoolTime >= 1.0f)
            {
                isInvincible = false;
            }
        }
    }

    void MoveInput()
    {
        if (Input.GetKey("w"))
        {
            up = true;
        }
        else up = false;

        if (Input.GetKey("s"))
        {
            down = true;
        }
        else down = false;
        vertical = (up ? 1 : 0) + (down ? -1 : 0);
        if (Input.GetKey("a"))
        {
            left = true;
        }
        else left = false;
        if (Input.GetKey("d"))
        {
            right = true;
        }
        else right = false;
        horizontal = (right ? 1 : 0) + (left ? -1 : 0);
    }

    void Move()
    {
        if (horizontal >= 1)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        }
        else if (horizontal <= -1)
        {
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        }
        if (transform.position.x <= mainCamera.ViewportToWorldPoint(Vector2.zero).x)
        {
            if (rb.velocity.x <= 0)
            {
                rb.velocity = new Vector2(0, rb.velocity.y);
            }
            if (horizontal <= -1)
            {
                rb.AddForce(new Vector2(0, vertical * MovePower), ForceMode2D.Force);
                return;
            }
        }
        rb.AddForce(new Vector2(horizontal * MovePower, vertical * MovePower) - rb.velocity, ForceMode2D.Force);
    }

    void IntervalCount(ref float count, float intervalTime, ref bool isTrigger, bool setBool)
    {
        count += Time.deltaTime;
        if (count >= intervalTime)
        {
            isTrigger = setBool;
            count = 0;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Burrow" && !isDeath)//巣穴での群れ回収
        {
            audioSource.clip = flocksSoundEffect;
            audioSource.Play();
            Hp++;
            CollFlocks();
        }
    }

    private void CollFlocks()
    {
        scoreManeger.AddFlockScore();
        MovePower -= 1;
        flocks.Add(Instantiate(flocksPrefab, transform.position, Quaternion.identity));
        if (flocks.Count == 1)
        {
            flocks[flocks.Count - 1].GetComponent<Flocks>().SetFollowObj(gameObject);
        }
        else
        {
            flocks[flocks.Count - 1].GetComponent<Flocks>().SetFollowObj(flocks[flocks.Count - 2]);
        }
    }
    public void Damege()
    {
        if (!isInvincible)
        {
            Hp = Hp / 2;
            MovePower = 13 - Hp;
            if (Hp < 1)
            {
                Hp = 0;
                isDeath = true;
            }
            audioSource.clip = damegeSoundEffect;
            audioSource.Play();
            for (int i = flocks.Count + 1; (i > Hp) && Hp >= 1; i--)
            {
                Destroy(flocks[i - 2]);
                flocks.RemoveAt(i - 2);
            }
            isInvincible = true;
            invincibleCoolTime = 0;
            animator.SetTrigger("Damege");
        }
    }

    public void SetStart()
    {
        transform.position = new Vector3(0, 0, 0);
        Start();
    }

    void Death()
    {
        for (int i = 0; i < flocks.Count; i++)
        {
            Destroy(flocks[0]);
            flocks.RemoveAt(0);
        }
        audioSource.clip = deathSoundEffect;
        audioSource.Play();
        Debug.Log("オイオイ死んだわアイツ");
        gameManeger.SetState(State.end);
        gameObject.SetActive(false);
        //死亡したときの処理
    }
}
