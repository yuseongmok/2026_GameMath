using UnityEngine;
using UnityEngine.InputSystem;

public class Move : MonoBehaviour
{
    private Vector2 moveInput;

    public void OnMove(InputValue Value)
    {
      moveInput = Value.Get<Vector2>();
    }
    // Update is called once per frame
    void Update()
    {
        if(moveInput.sqrMagnitude > 0.01f)
        {
            float angle = Mathf.Atan2(moveInput.y, moveInput.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, angle);
        }
    }
}
