using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{

    private Animator animator;
    private PlayerControls playerControls;


    void Awake()
    {
        animator = GetComponent<Animator>();
        playerControls = GetComponent<PlayerControls>();
    }


    void Update()
    {
        if(playerControls.AreaSwin() == true)
        {
           
            animator.SetBool("pSwim", playerControls.AreaSwin());
        }
        else
        {

            animator.SetInteger("pJump", playerControls.JumpValue());
            animator.SetBool("pClimb", playerControls.ClimbingValue());
        }

        animator.SetInteger("pMove", playerControls.MoveValueX() + playerControls.MoveValueY());
        animator.SetBool("pGround", playerControls.Grounded());

    }
}
