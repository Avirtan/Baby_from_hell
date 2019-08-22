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

    enum State {IDLE, RUN,SHOOT,JUMP,DEAD};
    private State state = State.IDLE;

    private UnityArmatureComponent armatureComponent;
	private DragonBones.AnimationState animState;
    private DragonBones.AnimationData data;
	void Start () {
		armatureComponent = GetComponent<UnityArmatureComponent>();
	}

    // Update is called once per frame
    void Update()
    {
       //Debug.Log(armatureComponent.TimeScale);
    }


    // Методы для анимации
    public void Idle(int direction) {
        if(state != State.DEAD){
            if(direction == 1){
                if (state != State.IDLE) {
                    armatureComponent.animation.FadeIn(idleAnimation, 0.1f, -1);
                    armatureComponent.animation.timeScale = 1f;
                    state = State.IDLE;
		        }
                armatureComponent.armature.flipX = false;
            }else 
            {
                if (state != State.IDLE) {
                    armatureComponent.animation.FadeIn(idleAnimation, 0.1f, -1);
                    armatureComponent.animation.timeScale = 1f;
                    state = State.IDLE;
		        }
                armatureComponent.armature.flipX = true;
            }
        }
		
	}

    public void Run(float speed,bool shift) {
        if(state !=State.DEAD){
            if(speed > 0){
                if(state != State.RUN){
                    animState = armatureComponent.animation.FadeIn(runAnimation, 0f, -1);
			        state = State.RUN;
                    animState.currentTime = 0.6f;
                }
                if(!shift)armatureComponent.animation.timeScale = 1f;
                else armatureComponent.animation.timeScale = 2f;
                armatureComponent.armature.flipX = false;
            }else{
                if(state != State.RUN){
                    armatureComponent.animation.FadeIn(runAnimation, 0f, -1);
			        armatureComponent.animation.timeScale = 1f;
			        state = State.RUN;
                }
                if(!shift)armatureComponent.animation.timeScale = 1f;
                else armatureComponent.animation.timeScale = 2f;
                armatureComponent.armature.flipX = true;
            }
        }
	}

    public void Jump(int direction,float speed){
        if(state != State.DEAD){
            if(direction == 1){
                if (state != State.JUMP) {
                    if(speed < -3 || speed > 3)
                        armatureComponent.animation.FadeIn(jumpRunAnimation, 0f, -1);
                    else if(speed <= -11 || speed >= 11){
                        armatureComponent.animation.FadeIn(jumpShiftAnimation, 0f, -1);
                        Debug.Log("test");
                    }
                    else
                        armatureComponent.animation.FadeIn(jumpStayAnimation, 0f, -1);
                    armatureComponent.animation.timeScale = 1f;
                    state = State.JUMP;
		        }
                armatureComponent.armature.flipX = false;
            }else 
            {
                if (state != State.JUMP) {
                    if((speed < -3 && speed > -5)||(speed > 3 && speed < 5))
                        armatureComponent.animation.FadeIn(jumpRunAnimation, 0f, -1);
                    else if(speed <= -11 || speed >= 11){
                        armatureComponent.animation.FadeIn(jumpShiftAnimation, 0f, -1);
                        Debug.Log("test");
                    }
                    else
                        armatureComponent.animation.FadeIn(jumpStayAnimation, 0f, -1);
                    armatureComponent.animation.timeScale = 1f;
                    state = State.JUMP;
		        }
                armatureComponent.armature.flipX = true;
            }
        }
    }


    public void Flip(){
        armatureComponent.armature.flipX = true;
    }
}
