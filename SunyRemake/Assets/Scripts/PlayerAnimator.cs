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
            animator.SetBool("pSwim", playerControls.SwimValue());
        }
        else
        {
            animator.SetInteger("pMove", playerControls.MoveValue());
            animator.SetInteger("pJump", playerControls.JumpValue());
        }        
    }
}
