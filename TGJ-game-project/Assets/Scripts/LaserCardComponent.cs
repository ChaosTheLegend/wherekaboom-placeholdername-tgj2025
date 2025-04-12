using DG.Tweening;
using System;
using UnityEngine;

public class LaserCardComponent : MonoBehaviour
{
	public cardType type;
	public bool isDragging = false;
	private Vector2 dragOffset;
	/// <summary>
	/// old position , new position
	/// </summary>
	public Action<Vector2Int, Vector2Int> OnCardMoved;
	public Action<bool> dragg;
	[SerializeField] private Renderer renderer;
	[SerializeField] private Collider collider;

	public Vector2Int position;

	private void Start()
	{
		UpdateMaterial();
		transform.localPosition = new Vector2(position.x, position.y) + Vector2.one / 2;
	}

	public bool HasSignal(Vector2Int direction)
	{
		if (direction == Vector2Int.up) return type.topSignal;
		if (direction == Vector2Int.down) return type.bottomSignal;
		if (direction == Vector2Int.right) return type.rightSignal;
		if (direction == Vector2Int.left) return type.leftSignal;
		return false;
	}

	private void UpdateMaterial()
	{
		var mat = renderer.material;
		mat.SetFloat("_top", type.topSignal ? 1 : 0);
		mat.SetFloat("_bottom", type.bottomSignal ? 1 : 0);
		mat.SetFloat("_right", type.rightSignal ? 1 : 0);
		mat.SetFloat("_left", type.leftSignal ? 1 : 0);
	}

	[Serializable]
	public struct cardType
	{
		public bool topSignal;
		public bool bottomSignal;
		public bool rightSignal;
		public bool leftSignal;
	}

	private void Update()
	{
		if (Input.GetMouseButtonDown(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (collider.Raycast(ray, out RaycastHit hit, 100f))
			{
				collider.transform.localScale *= 10;
				isDragging = true;
				dragOffset = hit.point - transform.position;
				renderer.sortingOrder++;
				transform.DOScale(0.9f, 0.2f).SetEase(Ease.OutBack);
				dragg?.Invoke(true);
			}
		}

		if (isDragging)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (collider.Raycast(ray, out RaycastHit hit, 100f))
			{
				transform.position = hit.point - (Vector3)dragOffset;
				if (Vector2Int.FloorToInt((Vector2)transform.localPosition) != position)
				{
					OnCardMoved?.Invoke(position, Vector2Int.FloorToInt((Vector2)transform.localPosition));
				}
			}
		}

		if (Input.GetMouseButtonUp(0) && isDragging)
		{
			collider.transform.localScale /= 10;
			isDragging = false;
			renderer.sortingOrder--;
			transform.DOScale(0.8f, 0.2f).SetEase(Ease.OutBack);
			dragg?.Invoke(false);
		}
	}
}
