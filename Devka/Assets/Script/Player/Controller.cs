using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float speed = 200f;
    public float jumpForce = 300f;

    private float MoveX = 0;
    private Rigidbody rgb3d;
    private Vector3 velocity;

    // Start is called before the first frame update
    void Start()
    {
        rgb3d = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
    }

    void FixedUpdate(){
        HandlerMove();
        rgb3d.velocity = velocity;
    }


    private void HandlerMove(){
        

        velocity = new Vector3(MoveX*speed,rgb3d.velocity.y,0f);
    }

    private void GetInput(){
        MoveX = Input.GetAxis ("Horizontal");
    }
}
