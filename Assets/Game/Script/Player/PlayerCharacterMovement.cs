using UnityEngine;

public class PlayerCharacterMovement : MonoBehaviour
{
    // Membuat variable untuk menyimpan data arah gerakan character
    private Vector3 _movementDirection;

    // Membuat variable untuk menentukan data kecepatan saat ini
    private float _currentSpeed = 0f;

    // Membuat variable untuk menentukan kecepatan saat berjalan
    [SerializeField]
    private float _walkSpeed = 1f;

    // Membuat variable untuk menentukan kecepatan saat berlari
    [SerializeField]
    private float _sprintSpeed = 2f;

    // Membuat variable untuk menentukan besar acceleration
    [SerializeField]
    private float _acceleration = 0.5f;

    // Membuat variable untuk menyimpan status apakah player sedang sprint
    private bool _isSprint;

    // Property IsSprint untuk mengakses variable _isSprint dari luar class
    public bool IsSprint => _isSprint;

    // Membuat variable untuk menyimpan arah dan kecepatan gerakan character di sumbu X dan Z
    private Vector3 _velocityXZ;

    // Membuat variable untuk menentukan besar gaya gravitasi
    [SerializeField]
    private float _gravityScale = 1f;

    // Membuat variable untuk menyimpan arah dan kecepatan gerakan character di sumbu Y
    private float _velocityY;

    // Membuat variable untuk menyimpan status apakah character sedang menyentuh tanah (ground)
    private bool _isGrounded;

    // Membuat variable untuk menyimpan reference ke component CharacterController
    [SerializeField]
    private CharacterController _characterController;

    private void Awake()
    {
        // Otomatis mengambil CharacterController jika belum di-assign di Inspector
        if (_characterController == null)
        {
            _characterController = GetComponent<CharacterController>();
        }
    }

    // Function untuk mengisi arah input ke arah gerakan character
    // Function ini akan listen ke event OnMoveInput
    public void SetMoveDirection(Vector2 inputDirection)
    {
        // Mengisikan arah input sumbu x ke arah gerakan character sumbu x
        // Mengisikan arah input sumbu y ke arah gerakan character sumbu z
        _movementDirection = new Vector3(inputDirection.x, 0, inputDirection.y);
    }

    // Function untuk menentukan status apakah player sedang sprint
    // Function ini akan listen ke event OnSprintInput
    public void SetSprint(bool isSprint)
    {
        _isSprint = isSprint;
    }

    private void CalculateAcceleration()
    {
        // Mengecek apakah player character bergerak atau tidak
        if (_movementDirection.magnitude > 0.01f)
        {
            // Jika karakter bergerak maka akan mengecek apakah character sedang sprint atau tidak
            if (_isSprint)
            {
                // Jika sedang sprint maka kecepatan akan bertambah sebesar nilai acceleration setiap detik
                _currentSpeed += _acceleration * Time.deltaTime;
            }
            else
            {
                // Jika berhenti sprint maka kecepatan akan berkurang sebesar nilai acceleration setiap detik
                _currentSpeed -= _acceleration * Time.deltaTime;
            }

            // Membatasi kecepatan, minimum: walk speed, maksimum: sprint speed
            _currentSpeed = Mathf.Clamp(_currentSpeed, _walkSpeed, _sprintSpeed);
        }
        else
        {
            // Jika tidak bergerak kecepatan diset nol, agar character tidak bisa bergerak
            _currentSpeed = 0f;
        }
    }

    private void CalculateVelocityXZ()
    {
        // Mendapatkan transform camera untuk mendapatkan rotasi camera
        Transform cameraTransform = Camera.main.transform;

        // Menghitung arah gerakan sumbu x disesuaikan dengan arah samping camera
        Vector3 xDirection = _movementDirection.x * cameraTransform.right;

        // Menghitung arah gerakan sumbu z disesuaikan dengan arah depan camera
        Vector3 zDirection = _movementDirection.z * cameraTransform.forward;

        // Menggabung arah gerakan sumbu x dan sumbu z ke dalam satu vector
        Vector3 direction = xDirection + zDirection;

        // Arah gerakan y dibuat nol, karena tidak ada gerakan ke arah atas dan bawah dari input horizontal
        direction.y = 0;

        // Mengecek apakah menekan input bergerak atau tidak
        if (_movementDirection.magnitude > 0.01f)
        {
            // Jika ada input move ditekan, karakter bergerak berdasarkan arah dan kecepatan
            // Dikalikan Time.deltaTime supaya kecepatan bergerak sama di semua FPS
            _velocityXZ = direction.normalized * _currentSpeed * Time.deltaTime;
        }
        else
        {
            // Jika tidak ada input move ditekan, karakter tidak bergerak
            _velocityXZ = Vector3.zero;
        }
    }

    private void CalculateVelocityY()
    {
        // Menghitung kecepatan gravitasi gerakan character di sumbu y
        _velocityY += Physics.gravity.y * _gravityScale * Time.deltaTime;
    }

    private void CheckIsGrounded()
    {
        // Mendapatkan layer Ground
        LayerMask groundLayer = LayerMask.GetMask("Ground");

        // Membuat pendeteksi berbentuk bola di posisi kaki character
        _isGrounded = Physics.CheckSphere(transform.position, 0.5f, groundLayer);
    }

    private void ResetVelocityY()
    {
        // Mengecek apakah character sudah ada di ground dan sedang bergerak ke bawah
        if (_isGrounded && _velocityY < 0)
        {
            // Reset velocity ke nilai kecil negatif agar karakter tetap menempel di tanah
            _velocityY = -2f;
        }
    }

    public void Move()
    {
        // Menghitung arah dan kecepatan gerakan character di sumbu x dan z
        CalculateVelocityXZ();

        // Menghitung arah dan kecepatan gerakan character di sumbu y (gravitasi)
        CalculateVelocityY();

        // Menggabung arah dan kecepatan gerakan character di sumbu x, y, dan z
        // _velocityY dikalikan Time.deltaTime agar perpindahan gravitasi konsisten per frame
        Vector3 velocity = new Vector3(_velocityXZ.x, _velocityY * Time.deltaTime, _velocityXZ.z);

        // Menggerakkan character sesuai arah dan kecepatan yang sudah dihitung
        _characterController.Move(velocity);
    }

    // Function yang akan dijalankan terus menerus setiap frame
    private void Update()
    {
        // 1. Cek apakah character berada di ground
        CheckIsGrounded();

        // 2. Menghitung acceleration kecepatan character
        CalculateAcceleration();

        // 3. Cek apakah kecepatan gravitasi Y perlu di-reset
        ResetVelocityY();

        // 4. Gerakkan character
        Move();
    }
}
