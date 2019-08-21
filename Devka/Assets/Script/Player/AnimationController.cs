using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class AnimationController : MonoBehaviour
{
    // Start is called before the first frame update
    public string idleAnimation = "idle";

	public string runAnimation = "running";

	public string jumpAnimation = "jumpStay";
	public string glideAnimation = "Glide";

	public string dieAnimation = "Die";

	public string attackAnimation = "Attack";

    enum State {IDLE, RUN,SHOOT,JUMP,DEAD};
    private State state = State.IDLE;

    private UnityArmatureComponent armatureComponent;
	private DragonBones.AnimationState aimState = null;

	void Start () {
		armatureComponent = GetComponent<UnityArmatureComponent>();
	}

    // Update is called once per frame
    void Update()
    {
       Debug.Log(armatureComponent.animation.lastAnimationName);
       armatureComponent.animation.Play("jumpStay");
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

    public void Run(float speed) {
        if(state !=State.DEAD){
            if(speed > 0){
                if(state != State.RUN){
                    armatureComponent.animation.FadeIn(runAnimation, 0f, -1);
			        armatureComponent.animation.timeScale = 1f;
			        state = State.RUN;
                }
                armatureComponent.armature.flipX = false;
            }else{
                if(state != State.RUN){
                    armatureComponent.animation.FadeIn(runAnimation, 0f, -1);
			        armatureComponent.animation.timeScale = 1f;
			        state = State.RUN;
                }
                armatureComponent.armature.flipX = true;
            }
        }
	}

    public void Jump(int direction){
        if(state != State.DEAD){
            if(direction == 1){
                if (state != State.JUMP) {
                    armatureComponent.animation.FadeIn(jumpAnimation, 0f, -1);
                    armatureComponent.animation.timeScale = 1f;
                    state = State.JUMP;
		        }
                armatureComponent.armature.flipX = false;
            }else 
            {
                if (state != State.JUMP) {
                    armatureComponent.animation.FadeIn(jumpAnimation, 0f, -1);
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
