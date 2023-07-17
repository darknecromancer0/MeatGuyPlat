using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Entity
{
    [SerializeField] private float speed = 3f; // скорость движения
    [SerializeField] private int lives = 5; // количество жизней
    [SerializeField] private float jumpForce = 3f; // сила прыжка
    private bool isGrounded = false;
    //private Vector3 validDirection = Vector3.up;  // What you consider to be upwards

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    public static Player Instance { get; set; }

    private States state
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>(); //GetComponentInParent??
    }

/*    private void FixedUpdate()
    {
        CheckGround();
    }
*/

    private void Update()
    {
        if (isGrounded) state = States.idle;

        if (Input.GetButton("Horizontal"))
            Run();
        if (isGrounded && Input.GetButton("Jump"))
            Jump();
        if (lives < 1)
            {
                Start();
                Death();
            }
    }

    void Start()
    {
        //Start the coroutine we define below named ExampleCoroutine.
        StartCoroutine(Death());
    }

    private IEnumerator Death() 
    {
        
                transform.position = new Vector3(-4f, -2f, 0.0f);
                Debug.Log("Вы погибли");
                state = States.death;

        //Print the time of when the function is first called.
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(0.35f);

        //After we have waited 5 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);

                lives = 5;
            
    }

    private void Run()
    {
        if (isGrounded) state = States.run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        sprite.flipX = dir.x < 0.0f;
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        state = States.jump;
    }


/*    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.8f);
        isGrounded = collider.Length > 1;

        //if (!isGrounded) state = States.jump;
    }
*/

    private void OnCollisionStay2D(Collision2D collision)
    {
        isGrounded = true;
    }


    private void OnCollisionExit2D(Collision2D collision)
    {
        isGrounded = false;
    }

    public override void GetDamage()
    {
        lives -=1;
        Debug.Log("У вас осталось " + lives + " жизней");
    }

    public enum States
    {
        idle,
        run,
        jump,
        death
    }
}
    
