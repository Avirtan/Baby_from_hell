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
    public float speed; // скорость персонажа
    public float jumpForce = 1000f; // сила прыжка
    public GameObject magic_left,magic_right; //левая магия
    public float airAcceleration = 2f; // сопротивление воздуха
    
    private bool isDeath = false; // проверка на смерть
    private Vector3 move; // вектор движения
    private double rateJump = 0.5; // коэффициент прыжка
    private float moveX = 0; // направление движения
    private bool jump = false; // проверка прыжка
    private bool grounded = false; // проверка земли
    private bool isLeft = false; // проверка поворота персонажа
    private bool isWallLeft = false; // проверка на левую стену
    private bool isWallRight = false; // проверка на правую стену
    private bool isGlideLeft = false; // Проверка на спуск лево 
    private bool isGlideRight = false;// Проверка на спуск право 
    private bool CastMagic = false; // анимация магии
    private double timerMagic = 0; // Для произносения магии раз в какое то время сек
    
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
        //Debug.Log(player.velocity.x);
        //Debug.Log(grounded);
        //Debug.Log("лево: "+isGlideLeft);
       // Debug.Log("право: "+isGlideRight);
      // Debug.Log(isDeath);
       // Debug.Log(CastMagic);
       
    }

    /// <summary>
    /// Основной цикл игры
    /// </summary>
    void FixedUpdate ()
    {
        if(!isDeath){
            CheckSide(); 
            if(!CastMagic){ // условия для заканчивания анимаций
                InputKey();
                Actions();
            }
            AnimationSet();
            player.velocity = move;
            DebugF();
        }else {
            if(moveX!=-100)Death(); // для того, чтоб один раз пригрывалась анимация, а новую переменную лень вводить
            move = new Vector3 (0, player.velocity.y, 0f);
            moveX = -100;
            player.velocity = move;

            if(Input.GetButton("Shift") )
            {
                isDeath = !isDeath;
                moveX = 0;
            }
        }
    }

    /// <summary>
    /// НАЖАТИЯ КЛАВИШ
    /// </summary>
    void InputKey(){
        //тестовая смерть
        if(Input.GetButton("Shift") )
        {
            isDeath = true;
        }
        //увелечение силы прыжка
        if(Input.GetButton("Jump") && grounded && rateJump < 1)
        {
            rateJump += Time.deltaTime * 2;
        }
        //отжатие пробела и прыжок
        if (!Input.GetButton("Jump") && grounded && rateJump > 0.5)
        {
            jump = true;
        }
        //движение в стороны
        moveX = Input.GetAxis ("Horizontal");
        //определение стороны движения
        isLeft = moveX != 0 ?(moveX>0?isLeft = false:isLeft=true):isLeft;
        //запуск магии
        if(Input.GetButton("Ctrl") && player.velocity.x == 0 && moveX == 0 && grounded && !CastMagic && Time.time > timerMagic )
        {
            //таймер 1 раз в сек 
            timerMagic = Time.time + 1;
            CastMagic = true;
           
        }
    }

    /// <summary>
    /// ДЕЙСТВИЯ
    /// </summary>
    void Actions(){
        //бег 
        move = new Vector3 (moveX * speed, player.velocity.y, 0f);
        // прыжок на земле
        if (jump)
        {
            // задание максимума высоты прыжка
            if (rateJump > 0.7) { rateJump = 1; }
            player.AddForce(new Vector2(0f, (float)(rateJump*jumpForce)));
            jump = false;
            rateJump = 0.5;
        }
        // прыжки от стены
        if (!grounded && (isWallLeft || isWallRight) && !jump &&  player.velocity.y < 0){
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
        //проверка нахождения на скользящей поверхности и скольжение 
        if(isGlideLeft){
           player.AddForce(new Vector2(-100, player.velocity.y));
        }
        if(isGlideRight){
           player.AddForce(new Vector2(100, player.velocity.y));
        }
        //магия 
        if(CastMagic){
             Vector3 v3 = transform.position;
            if(isLeft) {
                v3+= new Vector3(-0.4f,-0.1f,0);
                GameObject magic = Instantiate(magic_left, v3, transform.rotation);
            }
            else {
                v3+= new Vector3(0.5f,-0.1f,0);
                GameObject magic = Instantiate(magic_right, v3, transform.rotation);
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

        if(isWallLeft && player.velocity.y < 0) anim.SetInteger("isWall",-1);
        else if(isWallRight && player.velocity.y < 0) anim.SetInteger("isWall",1);
        else anim.SetInteger("isWall",0);

        if(isGlideLeft && moveX == 0) anim.SetInteger("isGlide",-1);
        else if(isGlideRight && moveX == 0) anim.SetInteger("isGlide",1);
        else anim.SetInteger("isGlide",0);

        if(CastMagic && isLeft) anim.SetInteger("IsMagic",-1);
        else if(CastMagic && !isLeft) anim.SetInteger("IsMagic",1);
        else anim.SetInteger("IsMagic",0);
    }

    /// <summary>
    /// ПРОВЕРКА ВСЕХ СТОРОН
    /// </summary>
    void CheckSide(){
        CheckGround();
        isWallLeft = Physics2D.Linecast(transform.position, wallCheckLeft.position,(1<<10));
        isWallRight = Physics2D.Linecast(transform.position, wallCheckRight.position,(1<<10));
        isGlideLeft = Physics2D.Linecast(transform.position, groundCheck.position,(1<<11));
        isGlideRight = Physics2D.Linecast(transform.position, groundCheck.position,(1<<12));
    }

    /// <summary>
    /// ПРОВЕРКА Земли
    /// </summary>
    void CheckGround(){
        bool grounded1 = Physics2D.Linecast(transform.position, groundCheck.position,(1<<10));
        bool grounded2 = Physics2D.Linecast(transform.position, groundCheck.position,(1<<11));
        bool grounded3 = Physics2D.Linecast(transform.position, groundCheck.position,(1<<12));
        if(grounded1 || grounded2 || grounded3) grounded = true;
        else grounded = false;
    }

    //смерть
    void Death(){
        anim.SetTrigger("Death");
    }

    // функция дла анимации магии
    void SetMagic(){
        CastMagic = false;
    }

    //возможно удалить потом, пока не нужен
    void DelaySpell(){
        if(timerMagic>0){
            timerMagic+=Time.deltaTime;
        }
        if(timerMagic>1) timerMagic = 0;
    }

    public void Test(){
        Debug.Log("test");
    }
    
}
