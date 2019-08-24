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
    private double time = 0;
    public bool isLanding = false; 

    void Start()
    {
        player = GetComponent<Rigidbody2D>();
        sprite = GetComponent<AnimationController>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isDeath){
            HandlerMove();
            HandlerJump();
            player.velocity = speed;
            speed.y = 0;
        }
        ControlAnimation();
        Debug.Log(player.velocity);
    }

    // управление движением
    void HandlerMove(){
        if(((time!= 0 && time != -1) || (OnWall()!=0 && (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))&& time != -1))&& !OnGround()) return;
        if(Input.GetButton("Shift") && moveX!=0 )
        {
            speed = new Vector3 (moveX * 12, player.velocity.y, 0f);
        }else speed = new Vector3 (moveX * 7, player.velocity.y, 0f);
        if (isLanding) speed.x = 0;
        moveX = Input.GetAxis ("Horizontal");
        sprite.FlipX(Diration());
    }

    //управление прыжком
    void HandlerJump(){
        if(OnGround() && Input.GetButton("Jump")){
            if(speed.x < -3 || speed.x > 3 ){
                if(Diration() == 1)player.AddForce (new Vector2 (70, 160f));
                else player.AddForce(new Vector2 (-70, 160f));
            }else if(moveX ==0){
                if(Diration() == 1) player.AddForce (new Vector2 (0f, 100f));
                else player.AddForce (new Vector2 (0f, 100f));
            }

        }
        //if(OnWall()!=0 && speed.y > 0) speed.y = 0;
        if(!OnGround() &&  player.velocity.y < 0 && OnWall()!=0 && OnWall() == Diration() && time != -1){
            speed = new Vector3(moveX * 7, -2f, 0f);
            if((OnWall() == -1 && Input.GetKey(KeyCode.D)) || (OnWall() == 1 && Input.GetKey(KeyCode.A)) && time==0){
                time = Time.time + 0.06;
            }
            if(time!=0 && time != -1){
                if(Input.GetAxis ("Horizontal") > 0 && Input.GetButton("Jump")){
                    //player.AddForce(new Vector2(-moveX * 80 * airAcceleration * 7, 50 * airAcceleration * 7));
                    player.AddForce(new Vector2(1000, 0));
                    //speed = new Vector3(moveX * 7, player.velocity.y, 0f);
                }
                if(Input.GetAxis ("Horizontal") < 0 && Input.GetButton("Jump")){
                    //player.AddForce(new Vector2(moveX * 80 * airAcceleration * 7, 50 * airAcceleration * 7));
                    player.AddForce(new Vector2(-500, 500));
                    speed = new Vector3(moveX * 7, player.velocity.y, 0f);
                }
            }
            if(Time.time > time && time!=0){
                time = -1;
            }
        }
        if(OnWall() == 0 || OnGround()) time =0;
    }

    
    void ControlAnimation(){
        if(isDeath){
            return;
        }else if(IsJump() && player.velocity.y<0 && OnWall()!=0 && time!=-1 && OnWall()==Diration()){
            //if(OnWall()==Diration())
           // Debug.Log("slide");
            sprite.Slide();
            //else sprite.Fall(player.velocity.y);
        }else if(IsJump()){
            if(player.velocity.y > 0) sprite.Jump(player.velocity.y);
            if(player.velocity.y < 0) {sprite.Fall(player.velocity.y);} 
        }else if(OnGround()){
            if(isRuning()){
                sprite.Run(Input.GetButton("Shift"));
            }else{
                sprite.Idle();
            }
        }else Debug.Log("else");
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

    bool IsJump(){
        if(!OnGround()) return true;
        else return false;
    }

    int Diration(){
        diraction = moveX != 0 ?(moveX>0?diraction = 1:diraction=-1):diraction;
        return diraction;
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
