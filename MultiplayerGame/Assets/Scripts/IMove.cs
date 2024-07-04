using UnityEngine.InputSystem;

public interface IMove
{
    public void HandleMovePerformed(InputAction.CallbackContext ctx);
    public void HandleMoveCancelled(InputAction.CallbackContext ctx);
    public void HandleDash(InputAction.CallbackContext ctx);
}
