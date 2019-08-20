using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DragonBones;

public class TestRun : MonoBehaviour
{
    // Start is called before the first frame update
    private Vector3 move;
    private Rigidbody2D player;
    private float moveX = 0;
    private UnityArmatureComponent armatureComponent;
    enum State {Stop, Run};
	private State state = State.Stop;
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
         armatureComponent = GetComponent<UnityArmatureComponent> ();
    }

    // Update is called once per frame
    void Update()
    {
        moveX = Input.GetAxis ("Horizontal");
        move = new Vector3 (moveX * 10, player.velocity.y, 0f);
        if(state != State.Run && moveX > 0){
            armatureComponent.animation.FadeIn("run");
            state = State.Run;
        }
        if(moveX == 0 && state != State.Stop) {
            state = State.Stop;
            armatureComponent.animation.FadeIn("animtion0");
            }
        player.velocity = move;
    }
}
