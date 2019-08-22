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
       //Debug.Log(animState.currentTime);
    }


    // Методы для анимации
    public void Idle(int direction) {
        if(state != State.DEAD){
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
        if(state !=State.DEAD){
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
        if(state != State.DEAD){
            if(direction == 1) armatureComponent.armature.flipX = false;
            else armatureComponent.armature.flipX = true;
            if (state != State.JUMP) {
                    if((speed < -3 && speed >= -7)||(speed > 3 && speed <= 7))
                        armatureComponent.animation.FadeIn(jumpRunAnimation, 0f, -1);
                    else if(speed <= -8 || speed >= 8){
                        armatureComponent.animation.FadeIn(jumpShiftAnimation, 0f, -1);
                    }
                    else
                        armatureComponent.animation.FadeIn(jumpStayAnimation, 0f, -1);
                    armatureComponent.animation.timeScale = 1f;
                    state = State.JUMP;
            }
        }
    }
       


    public void Flip(){
        armatureComponent.armature.flipX = true;
    }
}
