using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    public InputActionAsset actions;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction switchMaskAction;
    InputAction maskAbilityAction;
    InputAction crouchAction;
    Rigidbody2D rb;
    CircleCollider2D upperCollider;
    BoxCollider2D lowerCollider;
    [SerializeField]
    PlayerAnimator animator;

    [SerializeField]
    LayerMask environmentLayer;

    public ObjectManager OM;

    [SerializeField]
    float moveSpeed = 500f;

    public bool airborne = false;
    bool canDoubleJump = false;
    
    [SerializeField]
    List<Mask> masks = new List<Mask>();
    Mask currentMask;
    int currentMaskIndex = 0;

    float timeJumpPressed = 0f;
    float maxJumpTime = 0.75f;
    bool jumping = false;

    private void Awake() {
        moveAction = actions.FindActionMap("Player").FindAction("Move");
        switchMaskAction = actions.FindActionMap("Player").FindAction("SwitchMask");
        jumpAction = actions.FindActionMap("Player").FindAction("Jump");
        maskAbilityAction = actions.FindActionMap("Player").FindAction("MaskAbility");
        crouchAction = actions.FindActionMap("Player").FindAction("Crouch");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        upperCollider = GetComponent<CircleCollider2D>();
        lowerCollider = GetComponent<BoxCollider2D>();
        currentMask = masks[0];
        currentMaskIndex = 0;

        jumpAction.performed += JumpAction;
        jumpAction.canceled += JumpAction;
        switchMaskAction.performed += SwitchMask;
        maskAbilityAction.performed += ActivateMaskAbility;
        crouchAction.performed += CrouchAction;
        crouchAction.canceled += CrouchAction;
    }

    // Update is called once per frame
    void FixedUpdate() {
        Move();
        if(jumping) {
            if (timeJumpPressed < maxJumpTime)
                Jump();
            timeJumpPressed += Time.fixedDeltaTime;
        }
        CheckFloor();
    }

    void CheckFloor() {
        //TODO: Fix: Raycast is only in center, so might miss ground when on edges
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.1f, environmentLayer);
        if (hit.collider != null) {
            airborne = false;
            canDoubleJump = true;
        } else
            airborne = true;
    }

    void Move() {
        float dir = moveAction.ReadValue<Vector2>().x;
        rb.linearVelocity = new Vector2(dir * moveSpeed * Time.fixedDeltaTime, rb.linearVelocity.y);
        if (dir != 0) {
            int lookDir = dir > 0 ? 1 : -1;
            transform.localScale = new Vector3(lookDir, 1, 1);
        }
        if (airborne)
            animator.Jump();
        else if (dir != 0)
            animator.Run();            
        else
            animator.Idle();
    }

    void JumpAction(InputAction.CallbackContext context) {
        if (context.performed) {
            if (!airborne)
                jumping = true;
            else if (canDoubleJump) {
                jumping = true;
                canDoubleJump = false;
            }
        } else if (context.canceled) {
            timeJumpPressed = 0f;
            jumping = false;
        }
    }

    void Jump() {
        float jumpVelocity = 10f;
        jumpVelocity *=  maxJumpTime - (timeJumpPressed / maxJumpTime);
        rb.linearVelocityY = jumpVelocity;
    }

    public void SwitchMask(InputAction.CallbackContext context) {
        if (context.performed) {
            currentMask.DeactivateMask();
            if (currentMaskIndex + 1 == masks.Count)
                currentMaskIndex = 0;
            else
                currentMaskIndex += 1;
            currentMask = masks[currentMaskIndex];
            currentMask.ActivateMask();
            OM.switchMask(currentMask.VisibleLayer);
        }
    }

    public void ActivateMaskAbility(InputAction.CallbackContext context) {
        if (context.performed) 
            currentMask.ActivateAbility();
    }

    public void CrouchAction(InputAction.CallbackContext context) {
        if (context.performed)
            Crouch();
        else if (context.canceled)
            Uncrouch();
    }

    void Crouch() {
        upperCollider.enabled = false;
        lowerCollider.size = new Vector2(1, 0.25f);
        lowerCollider.offset = new Vector2(0, -0.375f);
        // animator.Crouch();
    }

    void Uncrouch() {
        //TODO: Check for ceiling before uncrouching
        upperCollider.enabled = true;
        lowerCollider.size = new Vector2(1, 0.5f);
        lowerCollider.offset = new Vector2(0, -0.25f);
        animator.Idle();
    }
}
