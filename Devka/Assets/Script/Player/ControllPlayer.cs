using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// УПРАВЛЕНИЕ ПЕРСОНАЖЕМ
/// </summary>
public class ControllPlayer : MonoBehaviour
{
    private Rigidbody2D player; // игрок
    public Animator anim; // анимация

    public Transform groundCheck; // зона снизу
    public Transform wallCheckLeft; // зона слева
    public Transform wallCheckRight; // зона справа

    private Vector3 move; // вектор движения

    public float speed; // скорость персонажа
    public float jumpForce = 1000f; // сила прыжка
    public float airAcceleration = 2f; // сопротивление воздуха
    private double rateJump = 0.5; // коэффициент прыжка
    private float moveX = 0; // направление движения

    private bool jump = false; // проверка прыжка
    private bool grounded = false; // проверка земли
    private bool isLeft = false; // проверка поворота персонажа
    private bool isWallLeft = false; // проверка на левую стену
    private bool isWallRight = false; // проверка на правую стену

    
    void Start()
    {
        player = GetComponent<Rigidbody2D>();
    }

    /// <summary>
    /// ДЕБАГ, вывод в консоль
    /// </summary>
    void DebugF(){
        //Debug.Log(rateJump);
        //Debug.Log( Input.GetButton("Jump"));
        //Debug.Log("left:"+isWallLeft);
        //Debug.Log("right:"+isWallRight);
        //Debug.Log(moveX);
        //Debug.Log(isLeft);
    }
    void FixedUpdate ()
    {
        move = new Vector3 (moveX * speed, player.velocity.y, 0f);
        CheckSide();
        InputKey();
        Actions();
        AnimationSet();
        player.velocity = move;
        DebugF();
    }

    /// <summary>
    /// НАЖАТИЯ КЛАВИШ
    /// </summary>
    void InputKey(){
        if(Input.GetButton("Jump") && grounded && rateJump < 1)
        {
            rateJump += Time.deltaTime * 2;
        }
        if (!Input.GetButton("Jump") && grounded && rateJump > 0.5)
        {
            jump = true;
        }
        moveX = Input.GetAxis ("Horizontal");
        isLeft = moveX != 0 ?(moveX>0?isLeft = false:isLeft=true):isLeft;
    }

    /// <summary>
    /// ДЕЙСТВИЯ
    /// </summary>
    void Actions(){

        // прыжок на земле
        if (jump && (!isWallLeft && !isWallRight))
        {
            // задание максимума высоты прыжка
            if (rateJump > 0.7) { rateJump = 1; }
            // 
            player.AddForce(new Vector2(0f, (float)(rateJump*jumpForce)));
            jump = false;
            rateJump = 0.5;
        }

        // прыжки от стены
        if (!grounded && (isWallLeft || isWallRight) && !jump){
            // скольжения по стене
            move = new Vector3 (moveX * speed, -1f, 0f);
            // нажатие пробела на стене
            if( Input.GetButton("Jump"))
            {
                // прыжок от левой стены
                if (isWallLeft && moveX > 0) {
                    player.velocity = Vector2.zero;
                    player.AddForce(new Vector2(moveX * 80 * airAcceleration * 7, 50 * airAcceleration * 7));
                }
                // прыжок от правой стены
                else if (isWallRight && moveX < 0)
                {
                    player.velocity = Vector2.zero;
                    player.AddForce(new Vector2(moveX * 80 * airAcceleration * 7, 50 * airAcceleration * 7));
                }
            }
        }

        // прыжок у стены в стиле Барокко!!! Требуется срочное исправление!!!                 <---------------------|
        if (grounded && (isWallLeft || isWallRight) && jump) 
        {
            if (isWallLeft)
            {
                player.AddForce(new Vector2(1000f, (float)(0)));
            }
            else if (isWallRight)
            {
                player.AddForce(new Vector2(-1000f, (float)(0)));
            }            
        }
    }

    /// <summary>
    /// АНИМАЦИЯ
    /// </summary>
    void AnimationSet(){
        anim.SetBool("isGround",grounded);
        anim.SetFloat("speed", moveX);
        anim.SetBool("isLeft", isLeft);
        anim.SetFloat("Jump", player.velocity.y);
        if(isWallLeft) anim.SetInteger("isWall",-1);
        else if(isWallRight) anim.SetInteger("isWall",1);
        else anim.SetInteger("isWall",0);
    }

    /// <summary>
    /// ПРОВЕРКА ЗЕМЛИ И СТЕН
    /// </summary>
    void CheckSide(){
        grounded = Physics2D.Linecast(transform.position, groundCheck.position,(1<<10));
        isWallLeft = Physics2D.Linecast(transform.position, wallCheckLeft.position,(1<<10));
        isWallRight = Physics2D.Linecast(transform.position, wallCheckRight.position,(1<<10));
    }
}
