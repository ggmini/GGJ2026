using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour {

    public InputActionAsset actions;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction switchMaskAction;
    InputAction maskAbilityAction;
    Rigidbody2D rb;

    [SerializeField]
    LayerMask environmentLayer;

    public ObjectManager OM;

    [SerializeField]
    float moveSpeed = 500f;

    bool airborne = false;
    bool canDoubleJump = false;
    
    [SerializeField]
    List<Mask> masks = new List<Mask>();
    Mask currentMask;
    int currentMaskIndex = 0;

    private void Awake() {
        moveAction = actions.FindActionMap("Player").FindAction("Move");
        switchMaskAction = actions.FindActionMap("Player").FindAction("SwitchMask");
        jumpAction = actions.FindActionMap("Player").FindAction("Jump");
        maskAbilityAction = actions.FindActionMap("Player").FindAction("MaskAbility");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        rb = GetComponent<Rigidbody2D>();
        currentMask = masks[0];
        currentMaskIndex = 0;

        jumpAction.performed += Jump;
        switchMaskAction.performed += SwitchMask;
        maskAbilityAction.performed += ActivateMaskAbility;
    }

    // Update is called once per frame
    void FixedUpdate() {
        float dir = moveAction.ReadValue<Vector2>().x;
        Move(dir);

        CheckFloor();
    }

    void CheckFloor() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.85f, environmentLayer);
        Debug.DrawRay(transform.position, Vector2.down * 0.8f, Color.red);
        if (hit.collider != null) {
            airborne = false;
            canDoubleJump = true;
        } else airborne = true;
    }

    void Move(float dir) {
        rb.linearVelocity = new Vector2(dir * moveSpeed * Time.fixedDeltaTime, rb.linearVelocity.y);
    }

    void Jump(InputAction.CallbackContext context) {
        if (context.performed) {
            if (!airborne) {
                rb.linearVelocityY = 0;
                rb.AddForce(new Vector2(0, 15f), ForceMode2D.Impulse);
            }
            else if (canDoubleJump) {
                rb.linearVelocityY = 0;
                rb.AddForce(new Vector2(0, 15f), ForceMode2D.Impulse);
                canDoubleJump = false;
            }
        }
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
}
