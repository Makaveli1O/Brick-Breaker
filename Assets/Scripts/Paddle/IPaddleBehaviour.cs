using UnityEngine.InputSystem;

public interface IPaddleBehaviour
{
    void Action(InputAction.CallbackContext ctx);
    float Speed { get; }
}