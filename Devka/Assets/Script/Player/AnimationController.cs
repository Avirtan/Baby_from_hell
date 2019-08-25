using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class AnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    public string idleAnimation = "idle";

	public string runAnimation = "running";

	public string jumpRunAnimation = "jump2";
    public string jumpStayAnimation = "jump1";
    public string slideAnimation = "slide";
	public string glideAnimation = "Glide";
    public string failAnimation = "fall";
	public string dieAnimation = "Die";
	public string attackAnimation = "Attack";

    enum State {IDLE, RUN,SHOOT,JUMP,DEAD,FAIL,LANDING,SLIDE};
    private State state = State.IDLE;

    private double time;
    private UnityArmatureComponent armatureComponent;
	private DragonBones.AnimationState animState;
    private DragonBones.AnimationData data;
    private Controller player;
	void Start () {
		armatureComponent = GetComponentInChildren<UnityArmatureComponent>();
        player = GetComponent<Controller>();
	}

    // Update is called once per frame
    void Update()
    {
      //Debug.Log(player.IsFail);
      //Debug.Log(armatureComponent.animation.GetState(jumpRunAnimation)._animationData.frameCount);
       //Debug.Log(armatureComponent);
    }


    // Методы для анимации
    public void Idle() {
        Landing();
        if(state != State.DEAD && state != State.LANDING){
            if (state != State.IDLE) {
                armatureComponent.animation.FadeIn(idleAnimation, 0.1f, -1);
                armatureComponent.animation.timeScale = 1f;
                state = State.IDLE;
		    }
        }
	}
        

    public void Run(bool shift) {
        Landing();
        if(state !=State.DEAD  && state != State.LANDING){
            if(state != State.RUN){
                animState = armatureComponent.animation.FadeIn(runAnimation, 0f, -1);
			    state = State.RUN;
                animState.currentTime = 0.6f;
            }
            if(!shift)armatureComponent.animation.timeScale = 1f;
            else armatureComponent.animation.timeScale = 2f;
        }
	}
       

    public void Jump(){
        if(state != State.DEAD && state != State.FAIL){
            if (state != State.JUMP) {
                armatureComponent.animation.Play(jumpRunAnimation, -1);
                time = Time.time+1.25;
                armatureComponent.animation.timeScale = 1f;
                state = State.JUMP;
            }
        }
        if(state == State.SLIDE){
            armatureComponent.animation.GotoAndPlayByTime(jumpRunAnimation,0.21f);
            armatureComponent.animation.timeScale = 1f;
            state = State.JUMP;
        }
        
    }

    public void Fall(float velocityY){
        //Debug.Log("fall");
        if((state != State.FAIL && Time.time > time)||(state == State.SLIDE && velocityY < 0)){
               armatureComponent.animation.FadeIn("fall", 0f,-1); 
               state = State.FAIL;
        }
    }

    public void Landing(){
        if(state == State.FAIL || state == State.JUMP){
            if((armatureComponent.animation.lastAnimationName == "jump2"  || armatureComponent.animation.lastAnimationName == "fall") && state != State.LANDING){
                state = State.LANDING;
                armatureComponent.animation.GotoAndPlayByTime(jumpRunAnimation,1.25f);
                player.IsLanding = true;
            }
        }
        if(armatureComponent.animation.GetState(jumpRunAnimation)!= null && (armatureComponent.animation.GetState(jumpRunAnimation).currentTime >= 1.55 || armatureComponent.animation.GetState(jumpRunAnimation).currentTime < 1.25)) 
        {
            state = State.JUMP;
            player.IsLanding = false;
        }
    }

    public void Slide(){
       // Debug.Log("slide");
        if(state != State.SLIDE && state != State.DEAD){
                armatureComponent.animation.Play(slideAnimation,  -1);
                armatureComponent.animation.timeScale = 1f;
                state = State.SLIDE;
        }
    }

    public void FlipX(int flip){
        if(flip == 1)armatureComponent.armature.flipX = false;
        else if(flip == -1) armatureComponent.armature.flipX = true;
    }

    public int GetFlipX(){
        if(!armatureComponent.armature.flipX ) return 1;
        else return -1;
    }
}
