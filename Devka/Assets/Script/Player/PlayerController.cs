using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AnimationController))]
public class PlayerController : MonoBehaviour
{
    public Transform groundCheck; // зона снизу
    public Transform wallCheckLeft; // зона слева
    public Transform wallCheckRight; // зона справа
    public float airAcceleration = 2f; // сопротивление воздуха
    public float speedX = 12;

    protected AnimationController sprite;
    private Vector3 speed;
    private Rigidbody2D player;
    private float moveX = 0;
    private int diraction = 1;
    private bool isDeath = false;
    private string TagGround = "GROUND";
    private bool isJump = false;
    private bool isFail = false;
    public bool IsFail
    {
        get
        {
            return isFail;
        }
 
        set
        {
            isFail = value;
        }
    }

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        sprite = GetComponent<AnimationController>();
        //player.transform.rotation = Quaternion.Euler(0,90,0);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isDeath){
            HandlerMove();
            HandlerJump();
        }
        ControlAnimation();
        //Debug.Log(player.velocity.y);
    }


    // управление движением
    void HandlerMove(){
        moveX = 0;
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.D))
            moveX = Input.GetAxis ("Horizontal");
        if(Input.GetButton("Shift") && moveX!=0)
        {
            speed = new Vector3 (moveX * 12, player.velocity.y, 0f);
        }else speed = new Vector3 (moveX * 7, player.velocity.y, 0f);
        diraction = moveX != 0 ?(moveX>0?diraction = 1:diraction=-1):diraction;
        sprite.FlipX(diraction);
        player.velocity = speed;
    }

    //управление прыжком
    void HandlerJump(){
        if(OnGround() && Input.GetButton("Jump")){
            isJump = true;
            if(speed.x < -3 || speed.x > 3){
                if(diraction == 1)player.AddForce (new Vector2 (70, 160f));
                else player.AddForce(new Vector2 (-70, 160f));
            }else if(moveX ==0){
                if(diraction == 1) player.AddForce (new Vector2 (0f, 100f));
                else player.AddForce (new Vector2 (0f, 100f));
            }

        }
        if(!OnGround() &&  player.velocity.y < 0 && OnWall()!=0){
            speed = new Vector3 (moveX * 12, -1f, 0f);
            if( Input.GetButton("Jump"))
            {
                if (OnWall()==-1 && moveX > 0) {
                    player.velocity = Vector2.zero;
                    player.AddForce(new Vector2(moveX * 80 * airAcceleration * 7, 50 * airAcceleration * 7));
                    isJump = true;
                }
                else if (OnWall()==1 && moveX < 0)
                {
                    player.velocity = Vector2.zero;
                    player.AddForce(new Vector2(moveX * 80 * airAcceleration * 7, 50 * airAcceleration * 7));
                    isJump = true;
                }
            }
        }
        if(player.velocity.y == 0 && OnGround()){ //&& !isFail){
            isJump = false;
        }

    }

    
    void ControlAnimation(){
        if(isDeath){
            return;
        }else if(OnWall()!=0 && !OnGround()){
            sprite.Slide();
        }else if(isJump){
            if(player.velocity.y > 0) sprite.Jump(diraction,speed.x);
            if(player.velocity.y < 0) sprite.Fall();
        }else if(OnGround()){
            if(isRuning()){
                sprite.Run(Input.GetButton("Shift"));
            }else{
                sprite.Idle();
            }
        }
    }

    bool isRuning(){
        return moveX == 0?false:true;
    }

    bool OnGround(){
        bool onGround = Physics2D.Linecast(transform.position, groundCheck.position,(1<<10));
        return onGround;
    }

    // 0 = нет или с 2 сторон, 1 - правая, -1 - левая
    int OnWall(){
        bool wallLeft = Physics2D.Linecast(transform.position, wallCheckLeft.position,(1<<10));
        bool wallRight = Physics2D.Linecast(transform.position, wallCheckRight.position,(1<<10));
        if(wallLeft && wallRight)
            return 0;
        else if(wallRight)
            return 1;
        else if(wallLeft)
            return -1;
        return 0;
    }

    /*
    protected virtual void OnCollisionStay2D(Collision2D collision) {
		if (collision.gameObject.CompareTag(TagGround)) {
			//onGround = true;
		}
	}
	protected virtual void OnCollisionExit2D(Collision2D collision) {
		if (collision.gameObject.CompareTag (TagGround)) {
			//onGround = false;
		}
	}
    void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag(TagGround)) {
			//onGround = true;
		}
    } */
}
