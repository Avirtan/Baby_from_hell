using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllPlayer : MonoBehaviour
{
    public float speed;
    public Transform groundCheck;
    public int gravity;
    public float jumpForce = 1000f;
    public Animator anim;

    private Rigidbody2D player;
    private bool  jump = false;
    private bool isJump = false;
    private bool grounded = false;
    private bool isLeft = false;
    private bool isFall = false;
    private double rateJump  = 0;
    private float moveX = 0;
    // Start is called before the first frame update 
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    /* Update is called once per frame 
    void Update()
    {
        //Debug.Log("player : "+transform.position);
        //Debug.Log("ground : "+groundCheck.position);
       //Debug.Log(Physics2D.Linecast(transform.position, groundCheck.position,(1<<8)).transform);//.transform.name);
        
        /* if (Input.GetButtonDown("Jump") && grounded)
        {
            //jump = true;
        }
    }*/

    void DebugF(){
        Debug.Log(rateJump);
        Debug.Log( Input.GetButton("Jump"));
    }
    void FixedUpdate ()
    {
        grounded = Physics2D.Linecast(transform.position, groundCheck.position,(1<<10));
        InputKey();
        Actions();
        AnimationSet();
        Vector3 move = new Vector3 (moveX * speed, player.velocity.y, 0f);
        player.velocity = move;
        DebugF();
    }
    void InputKey(){
        if( Input.GetButton("Jump") && grounded) if( rateJump < 1) rateJump+=Time.deltaTime;
        if( !Input.GetButton("Jump") && grounded && rateJump>0) jump = true;
        moveX = Input.GetAxis ("Horizontal");
        isLeft = moveX != 0 ?(moveX>0?isLeft = false:isLeft=true):isLeft = isLeft;
        TimeKeyDown();
    }
    void Actions(){
        if (jump)
        {
            player.AddForce(new Vector2(0f, (float)(rateJump*jumpForce)));
            jump = false;
            rateJump = 0.0f;
        }
    }

    void AnimationSet(){
        anim.SetFloat("speed", moveX);
        anim.SetBool("isLeft", isLeft);
        anim.SetFloat("Jump", player.velocity.y);
    }
}
