using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

sealed class PlayerController : MonoBehaviour {

    public InputActionAsset actions;
    InputAction moveAction;
    InputAction jumpAction;
    InputAction switchMaskAction;
    InputAction maskAbilityAction;
    InputAction crouchAction;
    InputAction sprintAction;

    [SerializeField]
    LayerMask environmentLayer;

    public ObjectManager OM;
    
    [SerializeField]
    List<Mask> masks = new List<Mask>();
    Mask currentMask;
    int currentMaskIndex = 0;

    float timeJumpPressed = 0f;
    bool jumping = false;

    [SerializeField]
    Player player;

    void Awake() {
        moveAction = actions.FindActionMap("Player").FindAction("Move");
        switchMaskAction = actions.FindActionMap("Player").FindAction("SwitchMask");
        jumpAction = actions.FindActionMap("Player").FindAction("Jump");
        maskAbilityAction = actions.FindActionMap("Player").FindAction("MaskAbility");
        crouchAction = actions.FindActionMap("Player").FindAction("Crouch");
        sprintAction = actions.FindActionMap("Player").FindAction("Sprint");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start() {
        currentMask = masks[0];
        currentMaskIndex = 0;

        jumpAction.performed += JumpAction;
        jumpAction.canceled += JumpAction;
        switchMaskAction.performed += SwitchMask;
        maskAbilityAction.performed += ActivateMaskAbility;
        crouchAction.performed += CrouchAction;
        crouchAction.canceled += CrouchAction;
        sprintAction.performed += context => player.SetSprinting(true);
        sprintAction.canceled += context => player.SetSprinting(false);
    }

    // Update is called once per frame
    void FixedUpdate() {
        Move();
        if(jumping) {
            if (timeJumpPressed < player.MaxJumpTime)
                player.Jump(timeJumpPressed);
            timeJumpPressed += Time.fixedDeltaTime;
        }
        player.CheckFloor();
    }

    

    void Move() {
        float dir = moveAction.ReadValue<Vector2>().x;
        player.Move(dir);
    }

    void JumpAction(InputAction.CallbackContext context) {
        if (context.performed) {
            if (player.TryJump()) {
                jumping = true;
            }
        } else if (context.canceled) {
            timeJumpPressed = 0f;
            jumping = false;
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

    public void CrouchAction(InputAction.CallbackContext context) {
        if (context.performed)
            player.Crouch();
        else if (context.canceled)
            player.Uncrouch();
    }
}
