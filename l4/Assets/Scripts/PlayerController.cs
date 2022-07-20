using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public float jumpForce = 12f;
    public float distanceToGround;
    public LayerMask groundMask;
    private Rigidbody2D rb;
    private Animator anim;
    private float horizontal;
    private bool facingRight;
    private bool grounded;
    private bool dead = false;

    public Image[] hearts;
    public int numOfHearts;
    public Sprite fullHeart;
    public Sprite noHeart;
    public float health;

    public GameObject respawn;

    public int coin;
    public Text collected;

    void Start()
    {
        //ініціалізація компонентів
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        collected.text = $"{coin}";
       if (dead == true)
        {
            SceneManager.LoadScene("game_over");
        }
            health = numOfHearts;
        
        for (int i = 0; i < hearts.Length; i++)
        {
            if (i < Mathf.RoundToInt(health))
            {
                hearts[i].sprite = fullHeart;
            }
            else
            {
                hearts[i].sprite = noHeart;
            }
            if (i < numOfHearts)
            {
                hearts[i].enabled = true;
            }
            else
            {
                //hearts[i].enabled = false;
                hearts[i].sprite = noHeart;
            }
        }
        //вхідні дані про переміщення вліво-вправо
        horizontal = Input.GetAxis("Horizontal");
        //анімація ходьби
        //перевірка чи персонаж стоїть на землі
        CheckGround();
        //зміна напрямку персонажа
        if (horizontal > 0 && facingRight || horizontal < 0 && !facingRight)
            Flip();
        //реалізація стрибку та його анімації
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
            anim.SetTrigger("Jump");
            Jump();
        }
        //анімація приземлення після стрибку

        //переміщення персонажа вліво-вправо
        if (horizontal != 0)
        {
            anim.SetBool("Iswalk", true);
            Move();
        }
        else anim.SetBool("Iswalk", false);
    }
    private void CheckGround()
    {
        grounded = Physics2D.Raycast(rb.position, Vector3.down, distanceToGround,
                  groundMask);
    }
    private void Jump()
    {
        rb.AddForce(Vector3.up * jumpForce, ForceMode2D.Impulse);
        anim.SetBool("Jump", true);
    }
    private void Move()
    {
        rb.velocity = new Vector3(horizontal * speed, rb.velocity.y);
        Vector3 position = rb.position;
        position.x = Mathf.Clamp(position.x, -15, 57);
        rb.position = position;
    }
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Finish")
        {
            rb.transform.position = respawn.transform.position;
            numOfHearts -= 1;
            if (numOfHearts == 0)
            {
                dead = true;
            }
        }
        if (other.tag == "coin")
        {
            if(numOfHearts  == 3)
            {
                numOfHearts = 3;
            }
            else
            {
                numOfHearts += 1;
            }
            
            coin++;
            Destroy(other.gameObject);
        }
    }
   
}