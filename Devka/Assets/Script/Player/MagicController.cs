using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagicController : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D magic;
    public float speed = 5f;

    private bool IsLeft = false;
    void Start()
    {
        magic = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        magic.velocity = new Vector2(speed,0);
    }
    void OnCollisionEnter2D(Collision2D coll)
    {
       // Debug.Log("entered");
       //coll.gameObject.SendMessage("Test");
       Destroy(gameObject);
    } 
}
