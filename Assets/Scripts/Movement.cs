using UnityEngine;

public class Movement : MonoBehaviour
{
     [SerializeField]  Rigidbody2D rb;
     [SerializeField]  float movementSpeed;
     [SerializeField]  float jumpForce;
     [SerializeField]  Vector2 movement;
     [SerializeField]  bool isGrounded;
     [SerializeField]  float jumpDamage;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (Input.GetButtonDown("Jump") && isGrounded)  
        {
            isGrounded = false;
            rb.AddForce(Vector2.up * jumpForce);
            Debug.Log("JUMPED!!!");
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float Xmovement = Input.GetAxis("Horizontal");

        movement = new Vector2 (Xmovement, 0);

        if (Xmovement != 0) 
        {
            movement = new Vector2(Xmovement * movementSpeed, rb.linearVelocity.y);
        }

        else 
        {
            movement = new Vector2(0, rb.linearVelocity.y);
        }

        rb.linearVelocity = movement;
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground")) 
        {
            isGrounded = true;
            Debug.Log("On the Ground!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyJumpDamaged")) 
        {
            Debug.Log("Dealt: " + jumpDamage);
        }
    }
}
