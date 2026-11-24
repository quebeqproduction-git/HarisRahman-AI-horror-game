using UnityEngine;
 
public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]
    private PlayerCharacterMovement _movement;
    [SerializeField]
    private PlayerCharacterStamina _stamina;
    [SerializeField]
    private InventoryManager _inventory;
    [SerializeField]
    private InteractDetector _interactDetector;
    [SerializeField]
    private CameraManager _camera;
    [SerializeField]
    private InputManager _input;
    // Property untuk mengakses variable _input
    public InputManager Input => _input;
    [SerializeField]
    private Flashlight _flashlight;
    // Property untuk mengakses variable _camera
    public CameraManager Camera => _camera;
    public InteractDetector InteractDetector => _interactDetector;
    public PlayerCharacterMovement Movement => _movement;
    public PlayerCharacterStamina Stamina => _stamina;
    public InventoryManager Inventory => _inventory;
     public Flashlight Flashlight => _flashlight;
     private void Awake()
    {
        // Ketika game dijalankan,
        // cursor mouse akan disembunyikan
        Cursor.visible = false;
        // cursor mouse akan dikunci di tengah layar
        Cursor.lockState = CursorLockMode.Locked;
    }
     public bool IsHiding { get; private set; }
      public void SetIsHiding(bool isHiding)
    {
        IsHiding = isHiding;
    }   
    
}