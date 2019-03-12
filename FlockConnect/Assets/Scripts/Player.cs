using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {
    Rigidbody2D rb;
    public int Hp { get; set; }
    public int startHp;
    public Camera mainCamera;
    [SerializeField]
    float invincibleCoolTime = 0,deathCount;
    bool isAvoid, avoidAble, avoidInput;
    Vector2 moveDirection;
    bool right,left,up,down;
    int vertical,horizontal;
    public bool isInvincible,isDeath;
    public float MovePower { get; set; }
    public float StartMovePower { get; set; }
    public Image damegeFlash;
    AudioSource soundEffect;
    public AudioClip shotSoundEffect;
    public AudioClip damegeSoundEffect;
    

    // Use this for initialization
    void Start () {
        Hp = startHp;
        rb = GetComponent<Rigidbody2D>();
        //soundEffect = GetComponent<AudioSource>();
        MovePower = 10;
        StartMovePower = MovePower;
        //damegeFlash.enabled = false;
        isDeath =false;
	}
	
	// Update is called once per frame
	void Update () {
        if (isDeath) {
            Death();
        }
        if (!isDeath) {
            MoveInput();
        }
        
    }

    void FixedUpdate() {
        Move();
        //無敵時間
        if (isInvincible) {
            invincibleCoolTime += Time.fixedDeltaTime;
            damegeFlash.color = new Color(damegeFlash.color.r, damegeFlash.color.g, damegeFlash.color.b, 1 - invincibleCoolTime);
            if (invincibleCoolTime >= 1.0f) {
                isInvincible = false;
            }
        }
    }

    void MoveInput() {
        if (Input.GetKey("w")) {
            up = true;
        } else up = false;

        if (Input.GetKey("s")) {
            down = true;
        } else down = false;
        vertical = (up ? 1:0) + (down ? -1:0);
        if (Input.GetKey("a")) {
            left = true;
        } else left = false;
        if (Input.GetKey("d")) {
            right = true;
        } else right = false;
        horizontal = (right ? 1:0) + (left ? -1:0);
    }

    void Move() {
        if(horizontal >= 1){
            transform.rotation = Quaternion.Euler(new Vector3(0,180,0));
        }else if(horizontal <= -1){
            transform.rotation = Quaternion.Euler(new Vector3(0,0,0));
        }
        if(transform.position.x <= mainCamera.ViewportToWorldPoint(Vector2.zero).x){
            if(rb.velocity.x <= 0){
                rb.velocity = new Vector2(0,rb.velocity.y);
            }
            if(horizontal <= -1){
                rb.AddForce(new Vector2(0, vertical * MovePower),ForceMode2D.Force);
                return;
            }
        }else if(transform.position.x >= mainCamera.ViewportToWorldPoint(new Vector2(1,1)).x){
            if(rb.velocity.x >= 0){
                rb.velocity = new Vector2(0,rb.velocity.y);
            }
            if(horizontal >= 1){
                rb.AddForce(new Vector2(0, vertical * MovePower),ForceMode2D.Force);
                return;
            }
        }
        rb.AddForce(new Vector2(horizontal * MovePower, vertical * MovePower) - rb.velocity,ForceMode2D.Force);
    }
    void IntervalCount(ref float count, float intervalTime, ref bool isTrigger, bool setBool) {
        count += Time.deltaTime;
        if (count >= intervalTime) {
            isTrigger = setBool;
            count = 0;
        }
    }
    public void Damege() {
        if (!isInvincible) {
            Hp -= 1;
            soundEffect.clip = damegeSoundEffect;
            soundEffect.Play();
            isInvincible = true;
            damegeFlash.enabled = true;
            invincibleCoolTime = 0;
            if (Hp <= 0) {
                isDeath = true;
            }
        }
    }

    public void Damege(int damege) {
        Hp -= damege - 1;
        Damege();
    }

    void Death() {
        //死亡したときの処理
    }
}
