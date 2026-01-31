using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

sealed class PlayerController : MonoBehaviour {

    [Header("Input Actions")]
    [SerializeField]
    InputActionAsset inputs;
    [SerializeField]
    InputActionReference moveAction;
    [SerializeField]
    InputActionReference jumpAction;
    //[SerializeField]
    //InputActionReference switchMaskAction;
    //[SerializeField]
    //InputActionReference maskAbilityAction;
    [SerializeField]
    InputActionReference crouchAction;
    [SerializeField]
    InputActionReference sprintAction;
    [Space(10)]

    [SerializeField]
    LayerMask environmentLayer;

    public ObjectManager OM;

    [SerializeField]
    List<Mask> masks = new();
    Mask currentMask;
    int currentMaskIndex = 0;

    [SerializeField]
    Player player;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void OnEnable() {
        currentMask = masks[0];
        currentMaskIndex = 0;

        inputs.Enable();

        moveAction.action.performed += Move;
        moveAction.action.canceled += Move;
        jumpAction.action.performed += JumpActionPerformed;
        jumpAction.action.canceled += JumpActionCancel;
        //switchMaskAction.action.performed += SwitchMask;
        //maskAbilityAction.action.performed += ActivateMaskAbility;
        crouchAction.action.performed += CrouchAction;
        crouchAction.action.canceled += UncrouchAction;
        sprintAction.action.performed += SprintAction;
        sprintAction.action.canceled += StopSprintAction;
    }

    void OnDisable() {
        moveAction.action.performed -= Move;
        moveAction.action.canceled -= Move;
        jumpAction.action.performed -= JumpActionPerformed;
        jumpAction.action.canceled -= JumpActionCancel;
        //switchMaskAction.action.performed -= SwitchMask;
        //maskAbilityAction.action.performed -= ActivateMaskAbility;
        crouchAction.action.performed -= CrouchAction;
        crouchAction.action.canceled -= UncrouchAction;
        sprintAction.action.performed -= SprintAction;
        sprintAction.action.canceled -= StopSprintAction;

        inputs.Disable();
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

    void CrouchAction(InputAction.CallbackContext context) {
        player.Crouch();
    }

    void UncrouchAction(InputAction.CallbackContext context) {
        player.Uncrouch();
    }

    void SprintAction(InputAction.CallbackContext context) {
        player.StartSprint();
    }

    void StopSprintAction(InputAction.CallbackContext context) {
        player.StopSprint();
    }
}
