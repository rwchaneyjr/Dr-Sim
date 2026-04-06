using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 2f;

    [Header("Animation")]
    public Animator animatorK;

    void Start()
    {
        // If you forget to drag the Animator in Inspector,
        // this will try to find it on this same object.
        if (animatorK == null)
        {
            animatorK = GetComponent<Animator>();
        }
    }

    void Update()
    {
        float moveX = 0f;
        float moveZ = 0f;

        // Movement input
        if (Input.GetKey(KeyCode.W)) moveZ += 1f;
        if (Input.GetKey(KeyCode.S)) moveZ -= 1f;
        if (Input.GetKey(KeyCode.A)) moveX -= 1f;
        if (Input.GetKey(KeyCode.D)) moveX += 1f;

        // Build movement direction
        Vector3 moveDir = new Vector3(moveX, 0f, moveZ).normalized;

        // Move player
        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.Self);

        // Check if player is moving
        bool isMoving = moveDir.magnitude > 0f;

        // Set Animator bools
        if (animatorK != null)
        {
            animatorK.SetBool("Walk", isMoving);
            animatorK.SetBool("Idle", !isMoving);
        }
    }
}