using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float moveSpeed = 2f;
   // public Animator animatorK;

 /*   void Start()
    {
        if (animatorK == null)
            animatorK = GetComponent<Animator>();
    }
 */
    void Update()
    {
        float moveX = 0f;
        float moveZ = 0f;

        // Corrected direction
        if (Input.GetKey(KeyCode.W)) moveZ += 1f;
        if (Input.GetKey(KeyCode.S)) moveZ -= 1f;
        if (Input.GetKey(KeyCode.A)) moveX -= 1f;
        if (Input.GetKey(KeyCode.D)) moveX += 1f;

        Vector3 moveDir = new Vector3(moveX, 0, moveZ).normalized;
        transform.Translate(moveDir * moveSpeed * Time.deltaTime, Space.Self);

        // 🔹 Simple animation trigger
     /*   if (Input.GetKey(KeyCode.W))
        {
            animatorK.Play("Walking");
        }
        else if (Input.GetKey(KeyCode.S))
        {
            animatorK.Play("Walking Backwards");
        }
        else
        {
            animatorK.Play("Idle");
        }
     */
    }
}
