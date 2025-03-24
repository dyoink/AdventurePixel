using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;

    [Header("MOVEMENT")]
    [SerializeField] private float moveSpeed;
    [SerializeField] private float jumpForce;
    [SerializeField] private float doubleJumpForce;
    [SerializeField] private float xInput;
    [SerializeField] private float yInput;
    private bool canDoubleJump;
    private bool isDoubleJumpping;
    private bool moved = true; // player có di chuyển được không



    [Header("WALL INTERACTIONS")]
    [SerializeField] private float wallJumpDura = 0.6f;
    [SerializeField] private Vector2 wallJumpForce;
    private bool isWallJumping;

    [Header("KNOCK BACK")]
    [SerializeField] private float knockBackDura = 0.6f;
    [SerializeField] private Vector2 knockBackPower;
    private bool isKnocked;



    [Header("COLLISION")]
    [SerializeField] private float groundCheckDistance;// Khoảng cách với Ground
    [SerializeField] private float wallCheckDistance;// khoảng cách với Wall
    [SerializeField] private LayerMask Ground; // 1 layer tên Ground
    private bool isGrounded; // trên mặt đất ?
    private bool faceRight = true; // mặt nhân vật hướng về bên phải
    private bool isWalled; // chạm vào tường ?
    private float faceDirect = 1; // hướng bên phải =1 , ngược lại
    [Space]
    [SerializeField] private float enemyDetectionDistance;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform enemyDetection;

    [SerializeField] private GameObject player_disappear;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
    void Update()
    {
        if (!moved)
        {
            HandleCollision();
            HandleAnimation();
            return;
        }
        HandleCollision();
        HandleInput();
        HandleFlip();
        HandleMovement();
        HandleWallSlide();
        HandleAnimation();
    }
    private IEnumerator KnockBackRoutine()
    {
        anim.SetBool("knockBack", true);
        yield return new WaitForSeconds(knockBackDura);
        isKnocked = false;
        this.enabled = true;
        anim.SetBool("knockBack", false);

    }
    public void KnockBack(float DmgDirection)
    {
        float direction = DmgDirection > transform.position.x ? 1 : -1;
        if (isKnocked) return;
        isKnocked = true;
        this.enabled = false;
        StartCoroutine(KnockBackRoutine());
        rb.linearVelocity = new Vector2(knockBackPower.x * -direction, knockBackPower.y);

    }
    public void Push(Vector2 forceDirection, float delay)
    {
        StartCoroutine(StartPush(forceDirection, delay));
        isDoubleJumpping = false;
        canDoubleJump = false;

    }
    private IEnumerator StartPush(Vector2 forceDirection, float delay)
    {
        moved = false;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(forceDirection, ForceMode2D.Impulse);// tạo lực đẩy theo hướng và mode vật lí
        yield return new WaitForSeconds(delay);
        moved = true;
    }
    public void Die()
    {
        GameObject player_disapp = Instantiate(player_disappear, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
    private void HandleWallSlide()
    {
        bool canWallSlide = isWalled && rb.linearVelocityY < 0;
        float slidingSpeed = yInput < 0 ? 1 : 0.1f;
        if (canWallSlide)
            rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * slidingSpeed);
        else return;
    }

    private void HandleInput() // 
    {
        xInput = Input.GetAxisRaw("Horizontal");
        yInput = Input.GetAxisRaw("Vertical");
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            jumpState();
        }
    }
    private void jumpState()
    {

        if (isGrounded)
        {
            Jump();
        }
        else if (isWalled && !isGrounded)
        {
            WallJump();
        }
        else if (canDoubleJump)
        {
            DoubleJump();
        }

    }
    
    private void HandleFlip()
    {
        if (xInput < 0 && faceRight || xInput > 0 && !faceRight)
        {
            Flip();
        }
    }
    private void DoubleJump()
    {
        isDoubleJumpping = true;
        isWallJumping = false;// nếu false thì có thể move để chuyển hướng khi double jump
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, doubleJumpForce);
        canDoubleJump = false;
        
    }
    private void Flip() // Xoay nhân vật
    {
        faceDirect = faceDirect * -1;
        transform.Rotate(0, 180, 0);
        faceRight = !faceRight;
    }
    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
        canDoubleJump = true;
        isDoubleJumpping = false;
    }
    private void WallJump()
    {
        canDoubleJump = true;
        isWallJumping = true;
        rb.linearVelocity = new Vector2(wallJumpForce.x * -faceDirect, wallJumpForce.y);// nhảy về phía đối diện mặt nhân vật
        Flip();
        StopAllCoroutines();
        StartCoroutine(WallJumpRoutine());
    }
    private IEnumerator WallJumpRoutine() // khi walljump sẽ bị ảnh hưởng bởi tọa độ X của của movement 
                                          // nên cần khóa movement trong 1 khoảng tg để walljump
    {
        isWallJumping = true;
        yield return new WaitForSeconds(wallJumpDura);
        isWallJumping = false;
    }

    private void HandleAnimation() // Gán các condition cho animator để chuyển animation
    {

        anim.SetFloat("xVelocity", rb.linearVelocity.x);
        anim.SetFloat("yVelocity", rb.linearVelocity.y);
        anim.SetBool("isGrounded", isGrounded);
        anim.SetBool("isWalled", isWalled);
        anim.SetBool("isDoubleJump", isDoubleJumpping);


    }

    void HandleMovement() // di chuyển ngang
    {
        if (!moved) return;
        if (isWallJumping) return; // thoát / khóa movement
        rb.linearVelocity = new Vector2(xInput * moveSpeed, rb.linearVelocity.y);

    }
    private void HandleCollision() // kiểm tra va chạm 
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, Ground);
        isWalled = Physics2D.Raycast(transform.position, Vector2.right * faceDirect, wallCheckDistance, Ground);
        HandleCollisionWithEnemy();
    }
    private void OnDrawGizmos() // vẽ 1 đường thẳng trong scene View để nhìn thấy vùng va chạm
    {
        Gizmos.DrawWireSphere(enemyDetection.position, enemyDetectionDistance);
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x + (faceDirect * wallCheckDistance), transform.position.y));
    }
    private void HandleCollisionWithEnemy()
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(enemyDetection.position, enemyDetectionDistance, enemyLayer);// kiểm tra va chạm với enemy

        foreach (var enemy in enemies)
        {
            Enemy newEnemy = enemy.GetComponent<Enemy>();
            
            newEnemy.Die();
            Jump();
        }
    }
}
