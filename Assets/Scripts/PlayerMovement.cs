using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerMovement2D : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private Animator animator;

    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    // private Animator animator;
    private Vector2 movementInput;

    // Mendefinisikan Input Action secara langsung di dalam kode
    private InputAction moveAction;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb.freezeRotation = true;

        // Membuat dan mengatur tombol untuk pergerakan (WASD & Arrow Keys)
        moveAction = new InputAction("Move", binding: "2DVector");
        moveAction.AddCompositeBinding("Dpad")
            .With("Up", "<Keyboard>/w")
            .With("Up", "<Keyboard>/upArrow")
            .With("Down", "<Keyboard>/s")
            .With("Down", "<Keyboard>/downArrow")
            .With("Left", "<Keyboard>/a")
            .With("Left", "<Keyboard>/leftArrow")
            .With("Right", "<Keyboard>/d")
            .With("Right", "<Keyboard>/rightArrow");
    }

    // Wajib diaktifkan saat objek nyala
    private void OnEnable()
    {
        moveAction.Enable();
    }

    // Wajib dimatikan saat objek mati agar tidak memakan memori
    private void OnDisable()
    {
        moveAction.Disable();
    }

    private void Update()
    {
        // Membaca nilai X dan Y dari Input System yang ditekan
        movementInput = moveAction.ReadValue<Vector2>();

        // Mengirim data ke Parameter Animator
        animator.SetFloat("Horizontal", movementInput.x);
        animator.SetFloat("Vertical", movementInput.y);
        animator.SetFloat("Speed", movementInput.sqrMagnitude);

        // Membalik (Flip) arah sprite ketika bergerak ke kiri atau kanan
        if (movementInput.x < 0)
        {
            spriteRenderer.flipX = true; // Hadap kiri
        }
        else if (movementInput.x > 0)
        {
            spriteRenderer.flipX = false; // Hadap kanan
        }

        // Mengunci arah hadap terakhir saat karakter diam (Idle)
        if (movementInput.x != 0 || movementInput.y != 0)
        {
            animator.SetBool("isWalking", true);
            animator.SetFloat("LastHorizontal", movementInput.x);
            animator.SetFloat("LastVertical", movementInput.y);
        } else
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void FixedUpdate()
    {
        // Mengeksekusi pergerakan. 
        // Nilai Vector2 dari Input System Dpad secara otomatis sudah dinormalisasi.
        rb.linearVelocity = movementInput * moveSpeed;
    }
}