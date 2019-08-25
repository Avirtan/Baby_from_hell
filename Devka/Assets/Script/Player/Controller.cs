using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    public float speed = 200f;
    public float jumpForce = 300f;

    private float MoveX = 0;
    private Rigidbody rgb3d;
    private Vector3 velocity = new Vector3(0,0,0);
    

    // Start is called before the first frame update
    void Start()
    {
        rgb3d = GetComponent<Rigidbody>();
        rgb3d.velocity = new Vector3(0,0,0);
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        isWall();
        isGround();
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


    private float lenghtToGround = 1.5f; // Расстояние до земли
    private bool isGround(){
       Debug.DrawLine(rgb3d.position,new Vector3(rgb3d.position.x,rgb3d.position.y-lenghtToGround,rgb3d.position.z),Color.red,2f);
       return Physics.Raycast(rgb3d.position, Vector3.down, lenghtToGround);
    }

    //Расстояние до правой и левой стены
    private float lenghtToWallLeft = 0.6f;
    private float lenghtToWallRight = 0.6f;
    private bool isWall(){
        bool left = Physics.Raycast(rgb3d.position, Vector3.left, lenghtToWallLeft);
        bool right = Physics.Raycast(rgb3d.position, Vector3.right, lenghtToWallRight);
        Debug.DrawLine(rgb3d.position,new Vector3(rgb3d.position.x-lenghtToWallLeft,rgb3d.position.y,rgb3d.position.z),Color.red,1f);
        Debug.Log("leftWall"+left);
        Debug.Log("Right"+right);
        Debug.DrawLine(rgb3d.position,new Vector3(rgb3d.position.x+lenghtToWallRight,rgb3d.position.y,rgb3d.position.z),Color.red,1f);
        if(left || right) return true;
        else return false;
    }
}
