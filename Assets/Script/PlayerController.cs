using UnityEngine;
using UnityEngine.Tilemaps;
using Managers;  // InputManager를 사용하기 위해 네임스페이스 포함

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("이동 속도")]
    public float moveSpeed = 7.0f;

    [Header("경계 추출할 Tilemap")]
    public Tilemap groundTilemap;

    private float pixelsPerUnit = 16f;
    private Rigidbody2D rb;
    private Animator animator;
    private Vector2 movement;

    private Vector2 minBounds;
    private Vector2 maxBounds;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        if (groundTilemap == null)
        {
            Debug.LogError("PlayerController: groundTilemap이 할당되지 않았습니다.");
            return;
        }

        Bounds localBounds = groundTilemap.localBounds;
        Vector3 tilemapPos = groundTilemap.transform.position;
        Vector3 worldMin = localBounds.min + tilemapPos;
        Vector3 worldMax = localBounds.max + tilemapPos;

        Vector3 cellSize = groundTilemap.cellSize;
        float extraShrinkX = 0.5f;
        float baseShrinkX  = cellSize.x;
        float shrinkX      = baseShrinkX + extraShrinkX;
        float shrinkY      = cellSize.y;

        minBounds = new Vector2(worldMin.x + shrinkX, worldMin.y + shrinkY);
        maxBounds = new Vector2(worldMax.x - shrinkX, worldMax.y - shrinkY);
    }

    private void Update()
    {
        movement = InputManager.Instance.GetMovement();

        float speedValue = Mathf.Abs(movement.x) + Mathf.Abs(movement.y);
        animator.SetFloat("Speed", speedValue);
    }

    private void FixedUpdate()
    {
        Vector2 rawTarget = rb.position + movement * moveSpeed * Time.fixedDeltaTime;
        float snappedX = Mathf.Round(rawTarget.x * pixelsPerUnit) / pixelsPerUnit;
        float snappedY = Mathf.Round(rawTarget.y * pixelsPerUnit) / pixelsPerUnit;
        Vector2 pixelPerfectTarget = new Vector2(snappedX, snappedY);

        float clampedX = Mathf.Clamp(pixelPerfectTarget.x, minBounds.x, maxBounds.x);
        float clampedY = Mathf.Clamp(pixelPerfectTarget.y, minBounds.y, maxBounds.y);
        Vector2 clampedPos = new Vector2(clampedX, clampedY);

        rb.MovePosition(clampedPos);
    }
}
