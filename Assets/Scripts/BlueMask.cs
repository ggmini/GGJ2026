using UnityEngine;

public class BlueMask : Mask
{

    SpriteRenderer sr;
    void Start() {
        sr = GetComponent<SpriteRenderer>();
        base.visibleLayer = new int[] {1};
    }

    public override void ActivateAbility()
    {
        Debug.Log("Blue Mask Ability Activated!");
    }
    public override void ActivateMask() {
        sr.enabled = true;
    }
    public override void DeactivateMask() {
        sr.enabled = false;
    }
}
