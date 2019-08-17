using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControllPlayer : MonoBehaviour
{
    public float speed;
    public Transform groundCheck;
    public int gravity;
    public float jumpForce = 1000f;
    public Animator anim;

    private Rigidbody2D player;
    private bool jump = false;
    private bool grounded = false;
    private bool isLeft = false;
    // Start is called before the first frame update 
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame 
    void Update()
    {
        //Debug.Log("player : "+transform.position);
        //Debug.Log("ground : "+groundCheck.position);
       // Debug.Log(Physics2D.Linecast(transform.position, groundCheck.position,(1<<8)).transform);//.transform.name);
        grounded = Physics2D.Linecast(transform.position, groundCheck.position,(1<<10));
        Debug.Log(grounded);
        if (Input.GetButtonDown("Jump") && grounded)
        {
            jump = true;
        }
    }

    void FixedUpdate ()
    {
        if (jump)
        {
            player.AddForce(new Vector2(0f, jumpForce));
            jump = false;
        }
        float x = Input.GetAxis ("Horizontal");

        isLeft = x != 0 ?(x>0?isLeft = false:isLeft=true):isLeft = isLeft;
        anim.SetFloat("speed", x);
        anim.SetBool("isLeft", isLeft);
        Vector3 move = new Vector3 (x * speed, player.velocity.y, 0f);
        player.velocity = move;
    }
}
