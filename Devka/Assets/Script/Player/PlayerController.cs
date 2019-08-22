using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (AnimationController))]
public class PlayerController : MonoBehaviour
{
    public Transform groundCheck; // зона снизу


    protected AnimationController sprite;
    private Vector3 speed;
    private Rigidbody2D player;
    private float moveX = 0;
    private int diraction = 1;
    private bool isDeath = false;
    private bool onGround = false;
    private string TagGround = "GROUND";
    private bool isJump = false;
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
            HandlerJumo();
        }
        ControlAnimation();
    }


    // управление движением
    void HandlerMove(){
        //speed = new Vector3 ( player.velocity.x, player.velocity.y, 0f);
        if(isJump) return;
        moveX = Input.GetAxis ("Horizontal");
        if(Input.GetButton("Shift") && moveX!=0)
        {
            speed = new Vector3 (moveX * 12, player.velocity.y, 0f);
        }else speed = new Vector3 (moveX * 7, player.velocity.y, 0f);
        diraction = moveX != 0 ?(moveX>0?diraction = 1:diraction=-1):diraction;
        player.velocity = speed;
    }

    //управление прыжком
    void HandlerJumo(){
        if(OnGround() && Input.GetButton("Jump")){
            isJump = true;
            if(moveX != 0){
                if(diraction == 1)
                    player.AddForce (new Vector2 (-0.5f, 2.4f),ForceMode2D.Impulse);
                else 
                    player.AddForce (new Vector2 (0.5f, 2.4f),ForceMode2D.Impulse);
            }else{
                if(diraction == 1)
                    player.AddForce (new Vector2 (0f, 2f),ForceMode2D.Impulse);
                else 
                    player.AddForce (new Vector2 (0f, 2f),ForceMode2D.Impulse);
            }

        }
        if(player.velocity.y == 0)
            isJump = false;

    }


    void ControlAnimation(){
        if(isDeath){
            return;
        }else if(isJump){
            sprite.Jump(diraction);
        }else if(OnGround()){
            if(isRuning()){
                sprite.Run(moveX,Input.GetButton("Shift"));
            }else{
                sprite.Idle(diraction);
            }
        }
    }

    bool isRuning(){
        return moveX == 0?false:true;
    }

    bool OnGround(){
        onGround = Physics2D.Linecast(transform.position, groundCheck.position,(1<<10));
        return onGround;
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
