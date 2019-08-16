using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllPlayer : MonoBehaviour
{
    public float speed;
    private Rigidbody2D player;
    private Vector3 change;
    private bool left;
    private bool top;
    // Start is called before the first frame update 
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame 
    void Update()
    {
        change = Vector3.zero;
        change.x = Input.GetAxisRaw("Horizontal");
        //change.y = Input.GetAxisRaw("Vertical");
        //Debug.Log("speed_x:"+change.x);
        // Debug.Log("speed_y:"+change.y*speed);
        if (change != Vector3.zero)
        {
            MoveCharacter();
            //Debug.Log("x:"+change.x);
            // Debug.Log("y:"+change.y);
        }
        //Debug.Log("vector:"+change);
    }
    void MoveCharacter()
    {
        player.MovePosition(
        transform.position + change * speed * Time.fixedDeltaTime
        );
    }
}
