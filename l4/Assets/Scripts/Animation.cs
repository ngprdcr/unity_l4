using UnityEngine;
public class Animation : MonoBehaviour
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
    void Start()
    {
        //����������� ����������
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }
    void Update()
    {
        //����� ��� ��� ���������� ����-������
        horizontal = Input.GetAxis("Horizontal");
        //������� ������
        //�������� �� �������� ����� �� ����
        CheckGround();
        //���� �������� ���������
        if (horizontal > 0 && facingRight || horizontal < 0 && !facingRight)
            Flip();
        //������� �����
        if (Input.GetMouseButtonDown(0))
            anim.SetTrigger("Attack");
        //��������� ������� �� ���� �������
        if (Input.GetKeyDown(KeyCode.Space) && grounded)
        {
           
            anim.SetTrigger("Jump");
            Jump();
        }
        //������� ����������� ���� �������
      
        //���������� ��������� ����-������
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
        position.x = Mathf.Clamp(position.x, -5, 6);
        rb.position = position;
    }
    private void Flip()
    {
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }
}

