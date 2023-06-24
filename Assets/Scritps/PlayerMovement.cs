using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float PlayerSpeed;
    [SerializeField] private float JumpForce;
    [SerializeField] private float JumpTime;
    [SerializeField] private float GroundRadius;
    [SerializeField] private Transform GroundCheck;
    [SerializeField] private LayerMask GroundLayers;
    [SerializeField] private Animator PlayerAnimator;

    private float Xaxis;
    private Rigidbody2D rb;
    private GameManagerScript GMS;

    private float JumpTimeCounter;

    private bool IsGrounded;
    private bool IsJumping;
    private bool LookingRight;
    private void Awake(){
        LookingRight = true;
        rb = GetComponent<Rigidbody2D>();
        GMS = GameObject.Find("Game Manager").GetComponent<GameManagerScript>();
    }
    private void Update(){
        Checking();
        Animate();
    }
    private void FixedUpdate() => rb.velocity = new Vector2(Xaxis * PlayerSpeed * Time.fixedDeltaTime, rb.velocity.y);
    private void Checking(){
        //Movement Check : on X axis
        Xaxis = Input.GetAxisRaw("Horizontal");
        //Ground Checking
        IsGrounded = Physics2D.OverlapCircle(GroundCheck.position, GroundRadius, GroundLayers);
        //Flip Check
        if(Xaxis > 0 && !LookingRight){
            Flip();
        }else if(Xaxis < 0 && LookingRight){
            Flip();
        }
        //Jump Check
        if(IsGrounded && Input.GetKeyDown(KeyCode.Space)){
            IsJumping = true;
            JumpTimeCounter = JumpTime;
            rb.velocity = new Vector2(rb.velocity.x, JumpForce * Time.fixedDeltaTime);
        }
        if (Input.GetKey(KeyCode.Space) && IsJumping){
            if(JumpTimeCounter > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, JumpForce * Time.fixedDeltaTime);
                JumpTimeCounter -= Time.deltaTime;
            }
            else
            {
                IsJumping = false;
            }
        }
        if (Input.GetKeyUp(KeyCode.Space))
        {
            IsJumping = false;
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        PortalScript PS = collision.gameObject.GetComponent<PortalScript>();
        if (collision.CompareTag("Portal0")){
            GMS.InPortal("0", PlayerSpeed, PS.NextPortal);
        }
        if (collision.CompareTag("Portal1"))
        {
            GMS.InPortal("1", PlayerSpeed, PS.NextPortal);
        }
        if (collision.CompareTag("Portal2"))
        {
            GMS.InPortal("2", PlayerSpeed, PS.NextPortal);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        GMS.CanTelePort = true;
    }
    private void Flip()
    {
        float PlayerTx = gameObject.transform.localScale.x;
        PlayerTx *= -1;
        gameObject.transform.localScale = new Vector3(PlayerTx, 1, 1);
        LookingRight = !LookingRight;
    }
    private void Animate()
    {
        //Walking Animation
        PlayerAnimator.SetFloat("X", Mathf.Abs(Xaxis));
        //Jumping Animation
        PlayerAnimator.SetFloat("Y", rb.velocity.y);
        PlayerAnimator.SetBool("Grounded", IsGrounded);
        print(rb.velocity.y);
    }
    private void OnDrawGizmos() => Gizmos.DrawWireSphere(GroundCheck.position, GroundRadius);
}
