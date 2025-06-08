using UnityEngine;
using UnityEngine.Tilemaps;

namespace Managers
{
	[RequireComponent(typeof(Camera))]
	public class CameraFollow : MonoBehaviour
	{
		[Header("추적 대상")]
		public Transform target;
		public Vector3 offset = new Vector3(0f, 0f, -10f);

		[Header("경계 추출할 Tilemap")]
		public Tilemap groundTilemap;

		private float smoothSpeed = 0.125f;
		private Camera cam;
		private float halfHeight;
		private float halfWidth;
		private Vector2 minBounds;
		private Vector2 maxBounds;

		private void Start()
		{
			cam = GetComponent<Camera>();
			halfHeight = cam.orthographicSize;
			halfWidth  = halfHeight * cam.aspect;

			if (groundTilemap == null)
			{
				Debug.LogError("CameraFollow: groundTilemap이 할당되지 않았습니다.");
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

		private void LateUpdate()
		{
			if (target == null) return;

			Vector3 desiredPos = target.position + offset;
			Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothSpeed);

			float clampedX = Mathf.Clamp(smoothedPos.x, minBounds.x + halfWidth, maxBounds.x - halfWidth);
			float clampedY = Mathf.Clamp(smoothedPos.y, minBounds.y + halfHeight, maxBounds.y - halfHeight);

			transform.position = new Vector3(clampedX, clampedY, smoothedPos.z);
		}
	}
}
