using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

sealed class PlayerController : MonoBehaviour {

    public InputActionAsset actions;
    [SerializeField]
    InputActionReference moveAction;
    InputAction jumpAction;
    InputAction switchMaskAction;
    InputAction maskAbilityAction;
    InputAction crouchAction;
    InputAction sprintAction;

    [SerializeField]
    LayerMask environmentLayer;

    public ObjectManager OM;

    [SerializeField]
    List<Mask> masks = new();
    Mask currentMask;
    int currentMaskIndex = 0;

    [SerializeField]
    Player player;

    void Awake() {
        switchMaskAction = actions.FindActionMap("Player").FindAction("SwitchMask");
        jumpAction = actions.FindActionMap("Player").FindAction("Jump");
        maskAbilityAction = actions.FindActionMap("Player").FindAction("MaskAbility");
        crouchAction = actions.FindActionMap("Player").FindAction("Crouch");
        sprintAction = actions.FindActionMap("Player").FindAction("Sprint");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable() {
        currentMask = masks[0];
        currentMaskIndex = 0;

        moveAction.action.performed += Move;
        moveAction.action.canceled += Move;
        jumpAction.performed += JumpActionPerformed;
        jumpAction.canceled += JumpActionCancel;
        switchMaskAction.performed += SwitchMask;
        maskAbilityAction.performed += ActivateMaskAbility;
        crouchAction.performed += CrouchAction;
        crouchAction.canceled += CrouchAction;
        sprintAction.performed += context => player.SetSprinting(true);
        sprintAction.canceled += context => player.SetSprinting(false);
    }
    void OnDisable() {
        moveAction.action.performed -= Move;
        moveAction.action.canceled -= Move;
        jumpAction.performed -= JumpActionPerformed;
        jumpAction.canceled -= JumpActionCancel;
        switchMaskAction.performed -= SwitchMask;
        maskAbilityAction.performed -= ActivateMaskAbility;
        crouchAction.performed -= CrouchAction;
        crouchAction.canceled -= CrouchAction;
        sprintAction.performed -= context => player.SetSprinting(true);
        sprintAction.canceled -= context => player.SetSprinting(false);
    }

    void Move(InputAction.CallbackContext context) {
        float dir = context.ReadValue<Vector2>().x;
        player.Move(dir);
    }

    void JumpActionPerformed(InputAction.CallbackContext context) {
        player.StartJump();
    }

    void JumpActionCancel(InputAction.CallbackContext context) {
        player.CancelJump();
    }

    public void SwitchMask(InputAction.CallbackContext context) {
        if (context.performed) {
            currentMask.DeactivateMask();
            if (currentMaskIndex + 1 == masks.Count) {
                currentMaskIndex = 0;
            } else {
                currentMaskIndex += 1;
            }

            currentMask = masks[currentMaskIndex];
            currentMask.ActivateMask();
            OM.switchMask(currentMask.VisibleLayer);
        }
    }

    public void ActivateMaskAbility(InputAction.CallbackContext context) {
        if (context.performed) {
            currentMask.ActivateAbility();
        }
    }

    public void CrouchAction(InputAction.CallbackContext context) {
        if (context.performed) {
            player.Crouch();
        } else if (context.canceled) {
            player.Uncrouch();
        }
    }
}
