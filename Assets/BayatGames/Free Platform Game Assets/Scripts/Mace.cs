using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mace : Entity
{
    [SerializeField] private int lives = 3;
//    [SerializeField] private float speed = 3.5f;
    private Vector3 direction;
    private SpriteRenderer sprite;

    private void Awake()
    {
        sprite = GetComponentInParent<SpriteRenderer>();
    }

    private void Start()
    {
        direction = transform.right;
    }    

    private void DeathCoroutine() 
    {
        StartCoroutine(Death());
    }

    private IEnumerator Death() 
    {
                transform.position = new Vector3(-20f, -20f, 0f);
                Debug.Log("Булава уничтожена");

        //Print the time of when the function is first called.
        //Debug.Log("Started Coroutine at timestamp : " + Time.time);

        //yield on a new YieldInstruction that waits for 5 seconds.
        yield return new WaitForSeconds(10f);

        //After we have waited 5 seconds print the time again.
        //Debug.Log("Finished Coroutine at timestamp : " + Time.time);

                transform.position = new Vector3(0.5f, -1.28f, 0f);
                lives = 3;
                Update();
            
    }

    private void Move()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.1f + transform.right * direction.x * 0.7f, 0.1f);

        if (colliders.Length > 0) direction *= -1f;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Time.deltaTime);
        sprite.flipX = direction.x > 0.0f;

    }

    private void Update()
    {
        Move();
        if (lives < 1) DeathCoroutine();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == Player.Instance.gameObject)
        {
            Player.Instance.GetDamage();
            lives--;
            Debug.Log("Булава получает 1 урон, остается " + lives);
        }
    }


}
