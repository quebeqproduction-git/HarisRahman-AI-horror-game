using UnityEngine;

public class PlayerCharacterStamina : MonoBehaviour
{
    // Membuat variable untuk menentukan maksimum stamina player
    [SerializeField]
    private float _maxStamina = 100f;

    // Membuat variable untuk menentukan jumlah stamina yang dibutuhkan untuk sprint per detik
    [SerializeField]
    private float _sprintStaminaCost = 20f;

    // Membuat variable untuk menentukan nilai regenerasi stamina per detik
    [SerializeField]
    private float _staminaRegenValue = 20f;

    // Menghitung stamina yang sedang dimiliki player character
    private float _currentStamina;

    // Membuat variable untuk reference ke object module PlayerCharacterMovement
    [SerializeField]
    private PlayerCharacterMovement _characterMovement;

    private void Awake()
    {
        // Otomatis mengambil PlayerCharacterMovement jika berada di GameObject yang sama dan belum di-assign di Inspector
        if (_characterMovement == null)
        {
            _characterMovement = GetComponent<PlayerCharacterMovement>();
        }

        // Di awal game stamina player diset menjadi maksimum
        _currentStamina = _maxStamina;
    }

    public void CalculateStamina()
    {
        // Mengecek apakah character sedang sprint atau tidak
        if (_characterMovement.IsSprint)
        {
            // Jika sedang sprint, mengecek apakah stamina masih ada
            if (_currentStamina > 0)
            {
                // Jika sedang sprint dan stamina masih ada, stamina akan dikurangi
                // Dikalikan dengan Time.deltaTime karena function ini dipanggil oleh Update
                _currentStamina -= _sprintStaminaCost * Time.deltaTime;
            }
            else
            {
                // Jika sedang sprint dan stamina habis, maka paksa player berhenti sprint
                _characterMovement.SetSprint(false);
            }
        }
        else
        {
            // Jika character tidak sedang sprint, tambahkan stamina dengan nilai regenerasi
            // Dikalikan dengan Time.deltaTime karena function ini dipanggil oleh Update
            _currentStamina += _staminaRegenValue * Time.deltaTime;
        }

        // Membatasi stamina player, minimum: 0, maksimum: maxStamina
        _currentStamina = Mathf.Clamp(_currentStamina, 0, _maxStamina);
    }

    private void Update()
    {
        // Menghitung stamina terus menerus sepanjang game
        CalculateStamina();
    }
}
