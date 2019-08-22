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
      //Debug.Log(armatureComponent.animation.GetState(jumpRunAnimation)._animationData.frameCount);
    }


    // Методы для анимации
    public void Idle(int direction) {
        if(state == State.FAIL){
            if(armatureComponent.animation.GetState(jumpShiftAnimation)!=null){
                armatureComponent.animation.GetState(jumpShiftAnimation).Play();
                if(armatureComponent.animation.GetState(jumpShiftAnimation).isCompleted) state = State.JUMP;
            }
            if(armatureComponent.animation.GetState(jumpRunAnimation)!=null){
                armatureComponent.animation.GetState(jumpRunAnimation).Play();
                if(armatureComponent.animation.GetState(jumpRunAnimation).isCompleted) state = State.JUMP;
            }
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
            if(armatureComponent.animation.GetState(jumpShiftAnimation)!=null){
                armatureComponent.animation.GetState(jumpShiftAnimation).Play();
                if(armatureComponent.animation.GetState(jumpShiftAnimation).isCompleted) state = State.JUMP;
            }
            if(armatureComponent.animation.GetState(jumpRunAnimation)!=null){
                armatureComponent.animation.GetState(jumpRunAnimation).Play();
                if(armatureComponent.animation.GetState(jumpRunAnimation).isCompleted) state = State.JUMP;
            }
            else state = State.FAIL;
           /*  armatureComponent.animation.GetState(jumpRunAnimation).Play();
           if(armatureComponent.animation.GetState(jumpRunAnimation).isCompleted) state = State.JUMP;
            else state = State.FAIL;*/
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
                
                if((speed < -3 && speed >= -7)||(speed > 3 && speed <= 7)){
                    armatureComponent.animation.Play(jumpRunAnimation, -1);
                    time = Time.time+1.71;
                }
                else if(speed <= -8 || speed >= 8){
                    armatureComponent.animation.Play(jumpShiftAnimation, -1);
                    time = Time.time+1.71;
                }
                else
                    armatureComponent.animation.Play(jumpStayAnimation, -1);
                armatureComponent.animation.timeScale = 1f;
                state = State.JUMP;
            }
        }
        //Debug.Log(armatureComponent.animation.GetState(jumpStayAnimation).weight );
        if(state == State.JUMP && Time.time > time  && state != State.DEAD && state != State.FAIL){
           if(armatureComponent.animation.GetState(jumpShiftAnimation)!=null)
                armatureComponent.animation.GotoAndStopByFrame(jumpShiftAnimation,31);
           if(armatureComponent.animation.GetState(jumpRunAnimation)!=null)
                armatureComponent.animation.GotoAndStopByFrame(jumpRunAnimation,31);
           state = State.FAIL;
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
