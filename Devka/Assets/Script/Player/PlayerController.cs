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
        }
        ControlAnimation();
        Debug.Log(isRuning());
    }

    void HandlerMove(){
        moveX = Input.GetAxis ("Horizontal");
        speed = new Vector3 (moveX * 10, player.velocity.y, 0f);
        diraction = moveX != 0 ?(moveX>0?diraction = 1:diraction=-1):diraction;
        player.velocity = speed;
    }

    void ControlAnimation(){
        if(isDeath){
            return;
        }else if(OnGround()){
            if(isRuning()){
                sprite.Run(moveX);
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
