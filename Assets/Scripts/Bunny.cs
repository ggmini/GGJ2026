using UnityEngine;
using static Codice.Client.Common.EventTracking.TrackFeatureUseEvent.Features.DesktopGUI.Filters;

sealed class NewMonoBehaviourScript : MonoBehaviour {

    Rigidbody2D rb;
    [SerializeField]
    LayerMask environmentLayer;
    float timeOnGround = 0f;
    [SerializeField]
    Vector2 jumpForce = new(200, 300);
    [SerializeField]
    int dir = 1;

    void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate() {
        var grounded = isGrounded();
        if (grounded)
            timeOnGround += Time.fixedDeltaTime;
        else
            timeOnGround = 0f;
        if (timeOnGround > 0.5f)
            Jump();
        Debug.Log("Grounded: " + grounded + " Time on ground: " + timeOnGround);

    }

    bool isGrounded() {
        var origin = transform.position + new Vector3(0, -0.5f, 0);
        var hit = Physics2D.Raycast(origin, Vector2.down, 0.1f, environmentLayer);
        Debug.DrawRay(origin, Vector2.down * 0.1f, Color.red);
        if (hit.collider != null)
            return true;
        return false;
    }

    void Jump() {
        rb.AddForce(new Vector2(jumpForce.x * dir, jumpForce.y));
        timeOnGround = 0f;
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            var player = other.gameObject.GetComponent<Player>();
            //TODO: Check if player is wearing bunny mask
            player.TakeDamage(2);
        }
    }
}
