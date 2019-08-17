using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllPlayer : MonoBehaviour
{
    public float speed;
    public Transform groundCheck;
    public Transform wallCheckLeft;
    public Transform wallCheckRight;
    public float jumpForce = 1000f;
    public Animator anim;

    private Rigidbody2D player;
    private bool  jump = false;
    private bool isJump = false;
    private bool grounded = false;
    private bool isLeft = false;
    private bool isFall = false;
    private double rateJump  = 0.5;
    private float moveX = 0;
    private bool isWallLeft = false;
    private bool isWallRight = false;
    private Vector3 move;
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    void DebugF(){
        //Debug.Log(rateJump);
       // Debug.Log( Input.GetButton("Jump"));
      // Debug.Log("left:"+isWallLeft);
       //Debug.Log("right:"+isWallRight);
       Debug.Log(move.x);
       //Debug.Log(isLeft);
    }
    void FixedUpdate ()
    {
        move = new Vector3 (moveX * speed, player.velocity.y, 0f);
        CheckSide();
        InputKey();
        Actions();
        AnimationSet();
        player.velocity = move;
        DebugF();
    }
    void InputKey(){
        if( Input.GetButton("Jump") && grounded) if( rateJump < 1) rateJump+=Time.deltaTime*2;
        if( !Input.GetButton("Jump") && grounded && rateJump>0.5) jump = true;
        moveX = Input.GetAxis ("Horizontal");
        isLeft = moveX != 0 ?(moveX>0?isLeft = false:isLeft=true):isLeft = isLeft;
    }

    void Actions(){
        if (jump)
        {
            if(rateJump > 0.7)rateJump = 1;
            player.AddForce(new Vector2(0f, (float)(rateJump*jumpForce)));
            jump = false;
            rateJump = 0.5;
        }
        if(!grounded && (isWallLeft || isWallRight)){
            move = new Vector3 (moveX * speed, -0.7f, 0f);
            if( Input.GetButtonDown("Jump")){
                player.velocity = Vector2.zero;
                if(isWallLeft) {player.AddForce(new Vector2(2000, 500));//new Vector2(5000, 500),ForceMode2D.Impulse);
                 move = new Vector3 (2, 0, 0f);}
                else {player.AddForce(new Vector2(-2000, 500));
                 move = new Vector3 (-2, 0, 0f);}
                isLeft =!isLeft;
            }
        }

    }

    void AnimationSet(){
        anim.SetBool("isGround",grounded);
        anim.SetFloat("speed", moveX);
        anim.SetBool("isLeft", isLeft);
        anim.SetFloat("Jump", player.velocity.y);
        if(isWallLeft) anim.SetInteger("isWall",-1);
        else if(isWallRight) anim.SetInteger("isWall",1);
        else anim.SetInteger("isWall",0);
    }
    void CheckSide(){
        grounded = Physics2D.Linecast(transform.position, groundCheck.position,(1<<10));
        isWallLeft = Physics2D.Linecast(transform.position, wallCheckLeft.position,(1<<10));
        isWallRight = Physics2D.Linecast(transform.position, wallCheckRight.position,(1<<10));
    }
}
