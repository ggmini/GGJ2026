using UnityEngine;

sealed class MaskAnimator : MonoBehaviour {
    [SerializeField]
    public Animator animator;
    [SerializeField]
    public SpriteRenderer sprite;

    public float visibility {
        get => material.GetFloat(nameof(visibility));
        set => material.SetFloat(nameof(visibility), value);
    }

    Material material;
    void OnEnable() {
        material = sprite.material;
        visibility = 0;
    }

    void OnDisable() {
        sprite.material = sprite.sharedMaterial;
        Destroy(material);
        material = null;
    }
}