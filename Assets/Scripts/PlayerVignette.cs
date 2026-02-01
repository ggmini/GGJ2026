using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

sealed class PlayerVignette : MonoBehaviour {
    [SerializeField]
    VolumeProfile volume;
    [SerializeField]
    Gradient colorOverHealth;
    [SerializeField]
    AnimationCurve intensityOverHealth = AnimationCurve.Linear(0, 0, 1, 1);

    void OnEnable() {
        Apply(1);
    }

    void OnDisable() {
        Apply(1);
    }

    void Update() {
        if (Player.instance) {
            Apply((float)Player.instance.health / Player.instance.maxHealth);
        }
    }

    void Apply(float normalizedHP) {
        if (volume.TryGet<Vignette>(out var vignette)) {
            vignette.color.Override(colorOverHealth.Evaluate(normalizedHP));
            vignette.intensity.Override(intensityOverHealth.Evaluate(normalizedHP));
        }
    }
}
