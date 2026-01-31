using UnityEngine;

public class Mask : MonoBehaviour
{
    protected int[] visibleLayer;
    public int[] VisibleLayer { get { return visibleLayer; } }
    public virtual void ActivateAbility(){}
    public virtual void ActivateMask(){}
    public virtual void DeactivateMask(){}
}
