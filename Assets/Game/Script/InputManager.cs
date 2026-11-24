using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using static GameInputAction;

public class InputManager : MonoBehaviour, IPlayerActions
{
    // Variable untuk menyimpan reference object input action
    private GameInputAction _inputAction;

    public UnityEvent<Vector2> OnMoveInput;
    public UnityEvent<bool> OnSprintInput;
    public UnityEvent OnInteractInput;
     public UnityEvent OnFlashlightInput;

    private void Awake()
    {
        // Membuat object GameInputAction dan menyimpan reference-nya ke variable _inputAction
        _inputAction = new GameInputAction();

        // Mengaktifkan input action
        _inputAction.Enable();

        // Mengaktifkan action map Player
        _inputAction.Player.Enable();

        // Memberi tahu bahwa kelas ini akan mendeteksi input dari action map Player
        _inputAction.Player.SetCallbacks(this);
    }

    private void OnEnable()
    {
        _inputAction?.Enable();
    }

    private void OnDisable()
    {
        // Praktik baik: nonaktifkan input action ketika object nonaktif
        _inputAction?.Disable();
    }

    public void OnInteraction(InputAction.CallbackContext context)
    {
        // context.performed digunakan untuk mengecek apakah input ditekan
        if (context.performed)
        {
            // Memunculkan log interact di console ketika input interact ditekan
            Debug.Log("Interact");
            OnInteractInput?.Invoke();
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        // Memanggil on move input ketika input move ditekan dan dilepas
        // Event akan mengirimkan data arah input
        OnMoveInput?.Invoke(context.ReadValue<Vector2>());
    }

    public void OnSprint(InputAction.CallbackContext context)
    {
        // Mengecek apakah input ditekan
        if (context.performed)
        {
            // Jika input ditekan maka trigger event OnSprintInput (mengirim data true)
            OnSprintInput?.Invoke(true);
        }

        // Mengecek apakah input dilepas
        if (context.canceled)
        {
            // Jika input dilepas maka trigger event OnSprintInput (mengirim data false)
            OnSprintInput?.Invoke(false);
        }
    }

    public void OnFlashLight(InputAction.CallbackContext context)
    {
        // context.performed digunakan untuk mengecek apakah input ditekan
        if (context.performed)
        {
            // Jika input ditekan maka trigger event OnFlashlightInput
            OnFlashlightInput?.Invoke();
        }
    }
}
