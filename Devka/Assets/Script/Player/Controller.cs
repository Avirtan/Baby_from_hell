using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour
{
    //Скорость по x
    public float speed = 200f;
    //Сила прыжка
    public float jumpForce = 300f;


    //время доступное после окончания платформы
    float fGroundedRemember = 0;
    float fGroundedRememberTime = 0.25f;
    //какая кнопка нажата D(1),A(-1),ничего(0)
    private float MoveX = 0;
    //Нажат ли пробел
    private bool Jump = false;
    //Физический объект
    private Rigidbody rgb3d;
    //таймер для нажатия в сторону при нажатии в сторону на стене 
    private float timerWall = 0;
    //Скорость по x,y,z
    private Vector3 velocity = new Vector3(0,0,0);
    //Направление движения персонажа ->(1),<-(-1),|(0)
    private int direction = 0;
    //Запрет передвижения при анимации приземления 
    private bool isLanding = false;
    public bool IsLanding
    {
        get
        {
            return isLanding;
        }
 
        set
        {
            isLanding = value;
        }
    }
    private int isRise = 0;
    public int IsRise
    {
        get
        {
            return isRise;
        }
 
        set
        {
            isRise = value;
        }
    }

    //Класс с анимациями
    protected AnimationController sprite;

    // Инициализация переменных перед появлением 
    void Start()
    {
        rgb3d = GetComponent<Rigidbody>();
        sprite = GetComponent<AnimationController>();
        rgb3d.velocity = new Vector3(0,0,0);
    }

    // Метод в котором будет отслеживаться ввод и расчёты, анимация
    void Update()
    {
        GetInput();
        HandlerMove();
        AnimationController();
        
        //Debug.Log(IsRise);
        fGroundedRemember -= Time.deltaTime;
        if (isGround() && !isJump())
        {
            fGroundedRemember = fGroundedRememberTime;
        }
    }

    // Для физических действий
    void FixedUpdate(){
        HandleJump();
        HandlerRise();
        rgb3d.velocity = velocity;
    }

    //метод для отслеживания движения
    private void HandlerMove(){
        velocity = new Vector3(MoveX*speed,rgb3d.velocity.y,0f);
        if(isLanding)
            velocity = Vector3.zero;
        if(Input.GetButton("Shift") && !isLanding){
            velocity.x = MoveX*12;
        }
        if(timerWall > Time.time)
            velocity.x = 0;
    }

    //для отслеживания прыжков
    private void HandleJump(){

        if(isGround() && Jump && !isLanding){
            fGroundedRemember = -1;
            rgb3d.AddForce(new Vector3(MoveX*200,jumpForce,0));
        }
        if(!isGround() && fGroundedRemember>0  && Jump && rgb3d.velocity.y < 0){
            rgb3d.AddForce(new Vector3(MoveX*200,jumpForce+100,0));
        }

        if(wallDirection()==MoveX && MoveX!=0 && !isGround() && rgb3d.velocity.y > 0){
            velocity.y = 0;
        }
        
        //Отскок от стены
        if(IsSlide()){
            velocity.y =-2;
            if((Input.GetAxis("Horizontal") < 0 && wallDirection() == 1) || (Input.GetAxis("Horizontal") > 0 && wallDirection() == -1)) {
                velocity.x = speed*MoveX;
            }
            if(Mathf.FloorToInt(MoveX) != wallDirection() && Input.GetAxis("Horizontal")!=0 && Input.GetButton("Jump")){
                rgb3d.AddForce(new Vector3(1250*direction,jumpForce+200,0));
                if(timerWall!=0)
                    timerWall = -1;
            }
        }
        if(!isWall() || isGround()) timerWall = 0;
    }

    private void HandlerRise(){
           //для правой стороны
           if((CheckBlocksForRiseRight() && direction == 1)){
               isRise = 1;
           }
           //для левой стороны
           if((CheckBlocksForRiseLeft() && direction == -1)){
               isRise = -1;
           }
           Debug.Log(sprite.riseTime);
           if(isRise != direction || sprite.riseTime > 0.71f || isGround()){
               isRise = 0;
               sprite.riseTime = 0;
           }
           if(isRise!=0){
               if(sprite.riseTime<=0.42 ){
                   velocity.y = 5f;
                 }
                if(sprite.riseTime > 0.42f && sprite.riseTime < 0.81f){
                    velocity.x = 5*isRise;
                }
               /*  else if(sprite.riseTime > 0.42f && sprite.riseTime < 0.81f && isRise == -1){
                     velocity.x -=5;
                }*/
           }
    }

    //отслеживание ввода с клавиатуры
    private void GetInput(){
        MoveX = Input.GetAxis ("Horizontal");
        if(MoveX!=0 && Mathf.FloorToInt(MoveX) != wallDirection() && IsSlide()){
                timerWall = Time.time + 1.5f;
        }
        if(Time.time > timerWall)
            direction = MoveX != 0 ?(MoveX>0?direction = 1:direction=-1):direction;
        sprite.FlipX(direction);
        Jump = Input.GetButtonDown("Jump");
    }

    //Для задания анимации
    private void AnimationController(){
        if(false){
            return;
        }else if(isRise!=0){//(wallDirection()==1 && CheckBlocksForRiseRight()) || isRise){
            sprite.Rise();
        }else if(wallDirection()!=0  && rgb3d.velocity.y < 0 && !isGround()){
             sprite.Slide();
        }else if(!isGround()){
            if(rgb3d.velocity.y > 0) {sprite.Jump();}
            if(rgb3d.velocity.y < 0) {sprite.Fall(rgb3d.velocity.y);} 
        }else if(isGround()){
            if(velocity.x != 0){
                sprite.Run(Input.GetButton("Shift"));
            }else{
                sprite.Idle();
            }
        }else Debug.Log("else");
    }


    // Расстояние до земли и определение нахождения на земле 
    private float lenghtToGround = 1.4f; 
    private bool isGround(){
       //Debug.DrawLine(rgb3d.position,new Vector3(rgb3d.position.x,rgb3d.position.y-lenghtToGround,rgb3d.position.z),Color.red,2f);
       return Physics.Raycast(rgb3d.position, Vector3.down, lenghtToGround);
    }

    //Расстояние до правой и левой стены и определение нахождения рядом со стеной 
    private float lenghtToWallLeft = 0.6f;
    private float lenghtToWallRight = 0.6f;
    private bool isWall(){
        bool left = Physics.Raycast(rgb3d.position, Vector3.left, lenghtToWallLeft);
        bool right = Physics.Raycast(rgb3d.position, Vector3.right, lenghtToWallRight);
        /* Debug.DrawLine(rgb3d.position,new Vector3(rgb3d.position.x-lenghtToWallLeft,rgb3d.position.y,rgb3d.position.z),Color.red,1f);
        Debug.Log("leftWall"+left);
        Debug.Log("Right"+right);
        Debug.DrawLine(rgb3d.position,new Vector3(rgb3d.position.x+lenghtToWallRight,rgb3d.position.y,rgb3d.position.z),Color.red,1f);*/
        if(left || right) return true;
        else return false;
    }
    
    //Определение у какой стены -1 - левая, 1 - правая, 0 - рядом нет стены
    private int wallDirection(){
        bool left = Physics.Raycast(rgb3d.position, Vector3.left, lenghtToWallLeft);
        bool right = Physics.Raycast(rgb3d.position, Vector3.right, lenghtToWallRight);
        if(left) return -1;
        else if(right) return 1;
        else return 0;
    }

    //Проверка соприкосается ли игрок с землёй или стеной
    private bool isTouchingGroundOrWall(){
        if(isWall() || isGround()) return true;
        else return false;
    }

    //Проверка скольжения 
    private bool IsSlide(){
        return rgb3d.velocity.y < 0 && wallDirection() == direction && !isGround();
    }
    //проверка прыжка
    private bool isJump(){
        return !(isGround() && (Mathf.FloorToInt(rgb3d.velocity.y) < 0 || Mathf.FloorToInt(rgb3d.velocity.y) > -1));
    }

    //Заползание 
    private float Xlength = 0.8f;
    private bool CheckBlocksForRiseRight(){
        //Debug.DrawLine(new Vector3(rgb3d.position.x,rgb3d.position.y+1.8f,rgb3d.position.z),new Vector3(rgb3d.position.x+Xlength,rgb3d.position.y+1.8f,rgb3d.position.z),Color.red,1f);
       // Debug.DrawLine(new Vector3(rgb3d.position.x,rgb3d.position.y+2.5f,rgb3d.position.z),new Vector3(rgb3d.position.x+Xlength,rgb3d.position.y+2.5f,rgb3d.position.z),Color.red,1f);
        //Debug.DrawLine(new Vector3(rgb3d.position.x,rgb3d.position.y+1.5f,rgb3d.position.z),new Vector3(rgb3d.position.x+Xlength,rgb3d.position.y+1.5f,rgb3d.position.z),Color.red,1f);
        bool Up = Physics.Linecast(new Vector3(rgb3d.position.x,rgb3d.position.y+2.5f,rgb3d.position.z),new Vector3(rgb3d.position.x+Xlength,rgb3d.position.y+2.5f,rgb3d.position.z));
        bool UpDown =  Physics.Linecast(new Vector3(rgb3d.position.x,rgb3d.position.y+1.8f,rgb3d.position.z),new Vector3(rgb3d.position.x+Xlength,rgb3d.position.y+1.8f,rgb3d.position.z));
        bool Middle = Physics.Linecast(new Vector3(rgb3d.position.x,rgb3d.position.y+1.5f,rgb3d.position.z),new Vector3(rgb3d.position.x+Xlength,rgb3d.position.y+1.5f,rgb3d.position.z));
        /*Debug.Log("Up:"+Up);
        Debug.Log("Updown:"+UpDown);
        Debug.Log("middle:"+Middle);*/
        if(!Up && !UpDown && Middle ) return true;
        else return false;
    }

    private bool CheckBlocksForRiseLeft(){
        //Debug.DrawLine(new Vector3(rgb3d.position.x,rgb3d.position.y+1.8f,rgb3d.position.z),new Vector3(rgb3d.position.x-Xlength,rgb3d.position.y+1.8f,rgb3d.position.z),Color.red,1f);
        //Debug.DrawLine(new Vector3(rgb3d.position.x,rgb3d.position.y+2.5f,rgb3d.position.z),new Vector3(rgb3d.position.x-Xlength,rgb3d.position.y+2.5f,rgb3d.position.z),Color.red,1f);
       //Debug.DrawLine(new Vector3(rgb3d.position.x,rgb3d.position.y+1.5f,rgb3d.position.z),new Vector3(rgb3d.position.x-Xlength,rgb3d.position.y+1.5f,rgb3d.position.z),Color.red,1f);
        bool Up = Physics.Linecast(new Vector3(rgb3d.position.x,rgb3d.position.y+2.5f,rgb3d.position.z),new Vector3(rgb3d.position.x-Xlength,rgb3d.position.y+2.5f,rgb3d.position.z));
        bool UpDown =  Physics.Linecast(new Vector3(rgb3d.position.x,rgb3d.position.y+1.8f,rgb3d.position.z),new Vector3(rgb3d.position.x-Xlength,rgb3d.position.y+1.8f,rgb3d.position.z));
        bool Middle = Physics.Linecast(new Vector3(rgb3d.position.x,rgb3d.position.y+1.5f,rgb3d.position.z),new Vector3(rgb3d.position.x-Xlength,rgb3d.position.y+1.5f,rgb3d.position.z));
        /*Debug.Log("Up:"+Up);
        Debug.Log("Updown:"+UpDown);
        Debug.Log("middle:"+Middle);*/
        if(!Up && !UpDown && Middle ) return true;
        else return false;
    }


    void OnCollisionEnter(Collision collision) {
       // Debug.Log(collision.gameObject.name);
    }
}
