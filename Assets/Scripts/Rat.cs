using UnityEngine;

sealed class Rat : MonoBehaviour
{
    [SerializeField]
    float dir = 1f;
    [SerializeField]
    float moveSpeed = 10f;

    
    void FixedUpdate() {
        CheckInFront();
        Move();
    }

    void CheckInFront() {
        //TODO: Adjust raycast positions based on rat size
        var origin = transform.position + new Vector3(dir * 0.75f, 0, 0);
        var hitFront = Physics2D.Raycast(origin, Vector2.right * dir, 0.1f, LayerMask.GetMask("Environment"));
        //Debug.DrawRay(origin, Vector2.right * dir * 0.1f, Color.red);
        origin.y -= 0.5f;
        var hitDown = Physics2D.Raycast(origin, Vector2.down, 0.1f, LayerMask.GetMask("Environment"));
        //Debug.DrawRay(origin, Vector2.down * 0.1f, Color.red);
        if (hitFront.collider != null || hitDown.collider == null) {
            dir *= -1f;
            var scale = transform.localScale;
            scale.x *= -1f;
            transform.localScale = scale;
        }
    }

    void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag("Player")) {
            var player = other.gameObject.GetComponent<Player>();
            //TODO: Check if player is wearing rat mask
            player.TakeDamage(1);
        }
    }

    void Move() {
        transform.position += new Vector3(dir * moveSpeed * Time.deltaTime, 0, 0);
    }

}
