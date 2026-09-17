using Unity.VisualScripting;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{

    [Header("Move Settings")]
    [SerializeField] private float moveSpeed;

    
    [Header("Jump Settings")]
    [SerializeField] private float jumpForce;
    [SerializeField] private Transform sensorGround;
    [SerializeField] private Vector3 sensorSize;
    [SerializeField] private float jumpTimeDuration;
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private float localGravity;


    [Header("Swim Settings")]
    [SerializeField] private float swimGravity;
    [SerializeField] private float swimForce;


    private Vector2 direction;
    private float currentJumpTime;
    private bool areaClimb;
    private bool climbing;
    private bool areaSwim;
    private bool swim;


    private Rigidbody2D rigidbody2D;
    private SpriteRenderer spriteRenderer;


    void Awake()
    {
        rigidbody2D = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Start()
    {
        rigidbody2D.gravityScale = localGravity;          
    }

    
    void Update()
    {
        if(areaSwim == true)
        {
            Swim();
        }
        else
        {            
            Jump();
            Climb();
        }

        Move();
    }


    void FixedUpdate()
    {
        OnMove();
        OnJump();
        OnSwim();
    }

    // ******** MOVE ***********

    void Move()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")) * moveSpeed;

        if (climbing == true) return;

        if (direction.x > 0)
        {
            spriteRenderer.flipX = false;
        }
        else if (direction.x < 0)
        {
            spriteRenderer.flipX = true;
        }
    }


    void OnMove()
    {
        if(climbing == true)
        {
            rigidbody2D.linearVelocity = direction;
        }
        else
        {
            rigidbody2D.linearVelocity = new Vector2(direction.x, rigidbody2D.linearVelocityY);
        }        
    }

    // ******** JUMP ***********

    void Jump()
    {
        if (Input.GetButtonDown("Jump") && Grounded() == true)
        {
            currentJumpTime = jumpTimeDuration;
        }
        else if (Input.GetButton("Jump") && currentJumpTime > 0)
        {
            currentJumpTime -= Time.deltaTime;
        }
        else if (Input.GetButtonUp("Jump"))
        {
            currentJumpTime = 0;
        }
    }


    void OnJump()
    {
        if (currentJumpTime > 0)
        {
            rigidbody2D.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    // ******** CLIMB ***********

    void Climb()
    {
        if(areaClimb == true && ((Input.GetAxisRaw("Vertical") > 0)))
        {
            ClimbEnable();           
        }
        else if (areaClimb == true && Input.GetButtonDown("Jump"))
        {
            ClimbODisable();
        }
        else if(areaClimb == false)
        {
            ClimbODisable();
        }

    }

    void ClimbEnable()
    {
        rigidbody2D.gravityScale = 0.0f;
        climbing = true;
    }

    void ClimbODisable()
    {
        rigidbody2D.gravityScale = localGravity;
        climbing = false;
    }

    // ******** SWIMMING ***********

    void Swim()
    {
        if (areaSwim == true && rigidbody2D.gravityScale != swimGravity)
        {
            rigidbody2D.gravityScale = swimGravity;
        }

        if(Input.GetButtonDown("Jump") && areaSwim == true)
        {
            swim = true;            
        }
    }

    void OnSwim()
    {
        if(swim == true)
        {
            rigidbody2D.AddForce(Vector2.up * swimForce, ForceMode2D.Impulse);
            swim = false;
        }
    }


    // ******** DETECTION ***********

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Climb"))
        {
            areaClimb = true;
        }
        else if (other.CompareTag("Swim"))
        {
            areaSwim = true;
        }
    }


    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Climb"))
        {
            areaClimb = false;
        }
        else if (other.CompareTag("Swim"))
        {
            areaSwim = false;
        }
    }

    // ******** RETURN ***********

    public int MoveValueX()
    {
        return (int)direction.x;
    }

    public int MoveValueY()
    {
        return (int)direction.y;
    }

    public int JumpValue()
    {
        return (int)rigidbody2D.linearVelocityY;
    }

    public bool SwimValue()
    {
        return swim;
    }

    public bool AreaSwin()
    {
        return areaSwim;
    }

    public bool ClimbingValue()
    {
        return climbing;
    }

    public bool Grounded()
    {
        return Physics2D.OverlapBox(sensorGround.position, sensorSize, 0, layerGround);
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawCube(sensorGround.position, sensorSize);
    }
}
