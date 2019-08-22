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
    public string jumpShiftAnimation = "jump3";
	public string glideAnimation = "Glide";

	public string dieAnimation = "Die";
	public string attackAnimation = "Attack";

    enum State {IDLE, RUN,SHOOT,JUMP,DEAD,FAIL};
    private State state = State.IDLE;

    private double time;
    private UnityArmatureComponent armatureComponent;
	private DragonBones.AnimationState animState;
    private DragonBones.AnimationData data;
	void Start () {
		armatureComponent = GetComponent<UnityArmatureComponent>();
	}

    // Update is called once per frame
    void Update()
    {
      // Debug.Log(state);
    }


    // Методы для анимации
    public void Idle(int direction) {
        if(state == State.FAIL){
            armatureComponent.animation.GetState(jumpRunAnimation).Play();
            Debug.Log(state);
            if(armatureComponent.animation.GetState(jumpRunAnimation).isCompleted) state = State.JUMP;
            else state = State.FAIL;
        }
        if(state != State.DEAD && state != State.FAIL){
            if(direction ==1)armatureComponent.armature.flipX = false;
            else armatureComponent.armature.flipX = true;
            if (state != State.IDLE) {
                armatureComponent.animation.FadeIn(idleAnimation, 0.1f, -1);
                armatureComponent.animation.timeScale = 1f;
                state = State.IDLE;
		    }
        }
	}
        

    public void Run(float speed,bool shift) {
        if(state == State.FAIL){
            armatureComponent.animation.GetState(jumpRunAnimation).Play();
            Debug.Log(state);
           if(armatureComponent.animation.GetState(jumpRunAnimation).isCompleted) state = State.JUMP;
            else state = State.FAIL;
        }
        if(state !=State.DEAD  && state != State.FAIL){
            if(speed > 0)armatureComponent.armature.flipX = false;
            else  armatureComponent.armature.flipX = true;
            if(state != State.RUN){
                animState = armatureComponent.animation.FadeIn(runAnimation, 0f, -1);
			    state = State.RUN;
                animState.currentTime = 0.6f;
            }
            if(!shift)armatureComponent.animation.timeScale = 1f;
            else armatureComponent.animation.timeScale = 2f;
        }
	}
       

    public void Jump(int direction,float speed){
        if(state != State.DEAD && state != State.FAIL){
            if(direction == 1) armatureComponent.armature.flipX = false;
            else armatureComponent.armature.flipX = true;
            if (state != State.JUMP) {
                time = Time.time+1;
                if((speed < -3 && speed >= -7)||(speed > 3 && speed <= 7))
                    animState = armatureComponent.animation.Play(jumpRunAnimation, -1);
                else if(speed <= -8 || speed >= 8){
                   armatureComponent.animation.Play(jumpShiftAnimation, -1);
                }
                else
                    armatureComponent.animation.Play(jumpStayAnimation, -1);
                armatureComponent.animation.timeScale = 1f;
                state = State.JUMP;
            }
        }
        //Debug.Log(armatureComponent.animation.GetState(jumpStayAnimation).weight );
        if(state == State.JUMP && Time.time > time +1 && state != State.DEAD && state != State.FAIL){
           armatureComponent.animation.GotoAndStopByFrame(jumpRunAnimation,60);
           state = State.FAIL;
          // Debug.Log(animState.isCompleted);
          // armatureComponent.animation.GotoAndPlayByTime(jumpRunAnimation,1.2f);
        }
        /* if(Time.time > time+2){
            armatureComponent.animation.GetState(jumpRunAnimation).Play();
        }*/
       // Debug.Log(armatureComponent.animation.GetState(jumpRunAnimation).isCompleted);
        //if(r) Debug.Log("test");
    }

    public bool Fail(){
        if(state == State.FAIL){
            armatureComponent.animation.GetState(jumpRunAnimation).Play();
            Debug.Log("test");
            return armatureComponent.animation.GetState(jumpRunAnimation).isCompleted;
        }else return true;
    }
}
