using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] 
    float moveSpeed = 5f;
    [SerializeField]
    float jumpForce = 10f;
    [SerializeField] 
    float jumpForceShort = 5f;
    [SerializeField]
    float rayDistance = 5f;
    [SerializeField]
    Color rayColor = Color.red;
    [SerializeField]
    LayerMask groundLayer;
    Rigidbody2D rb2D;
    SpriteRenderer spr;
    Animator anim;
    [SerializeField]
    Vector3 rayOrigin;
    
    GameInputs gameInputs;
    void Awake() 
    {
        rb2D = GetComponent<Rigidbody2D>();
        spr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        gameInputs = new GameInputs();
    }
    
    void Start()
    {
        //_ = context
        //gameInputs.Gameplay.Jump.performed += _ => Jump();
    }

    void OnEnable()
    {
        gameInputs.Enable();
    }

    void OnDisable()
    {
        gameInputs.Disable();
    }

    public void OnJump(InputAction.CallbackContext context){
        switch(context.phase){
            case InputActionPhase.Canceled:
                Jump();
                Debug.Log("Largo");
                break;
            case InputActionPhase.Performed:
                JumpShort();
                Debug.Log("corto");
                break;
        }
    }
    void FixedUpdate()
    {
        rb2D.position += Vector2.right * Horizontal * moveSpeed * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Vector2.right * Axis.x * moveSpeed * Time.deltaTime); 
        spr.flipX = FlipSpriteX;
        /*if(JumpButton && IsGrounding)
        {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }*/

    }

    void LateUpdate()
    {
        anim.SetFloat("AxisX", Mathf.Abs(Horizontal));
        anim.SetBool("ground", IsGrounding);
    }

    void OnDrawGizmosSelected() 
    {
        Gizmos.color = rayColor;
        Gizmos.DrawRay(transform.position + rayOrigin, Vector2.down*rayDistance);
        
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.CompareTag("coin"))
        {
            Coin coin = other.GetComponent<Coin>();
            GameManager.instance.GetScore.AddPoints(coin.GetPoints);
            Destroy(other.gameObject);
        }
    }

    void Jump()
    {
        if(IsGrounding)
        {
            rb2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }
    }
    void JumpShort(){
        if(IsGrounding)
        {
            rb2D.AddForce(Vector2.up * jumpForceShort, ForceMode2D.Impulse);
            anim.SetTrigger("jump");
        }
    }
    float Horizontal => gameInputs.Gameplay.Horizontal.ReadValue<float>();
    bool IsGrounding => Physics2D.Raycast(transform.position + rayOrigin, Vector2.down, rayDistance, groundLayer);
    bool FlipSpriteX => Horizontal > 0f ? false : Horizontal < 0f ? true : spr.flipX;

    //Vector2 Axis => new Vector2(Input.GetAxis("Horizontal"),Input.GetAxis("Vertical"));
}
