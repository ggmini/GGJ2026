using UnityEngine;

public class RedMask : Mask
{
    SpriteRenderer sr;
    void Start() {
        sr = GetComponent<SpriteRenderer>();
        base.visibleLayer = new int[] {0};
    }

    public override void ActivateAbility()
    {
        Debug.Log("Red Mask Ability Activated!");
    }
    public override void ActivateMask() {
        sr.enabled = true;
    }
    public override void DeactivateMask() {
        sr.enabled = false;
    }
}

