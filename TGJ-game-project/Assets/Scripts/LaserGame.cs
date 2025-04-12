using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class LaserGame : MonoBehaviour
{
	[SerializeField] private LineAnimator effectLine;
	[SerializeField] private Transform emptyObject;

	public UnityEvent<int> onActivated;
	public UnityEvent<int> onDeactivated;
	private bool[] isActive;

	private List<LineAnimator> effects;
	private LaserCardComponent[,] laserCards = null;
	private Vector2[,] inerts;
	private LaserLevel level;
	private Transform[] controlPositions;

	private bool[,] used;

	private void Start()
	{
		ClearGrid();
	}

	private void ClearGrid()
	{
		for (int i = transform.childCount - 1; i >= 0; i--)
		{
			DestroyImmediate(transform.GetChild(i).gameObject);
		}

		isActive = null;
	}

	private void Update()
	{
		if (level == null) return;

		for (var x = 0; x < level.size; x++)
		{
			for (var y = 0; y < level.size; y++)
			{
				if (laserCards[x, y].isDragging)
				{
					inerts[x, y] = Vector2.zero;
					continue;
				}

				var pos = (GetPos(x - 1, y) + GetPos(x + 1, y) + GetPos(x, y - 1) + GetPos(x, y + 1)) / 4;
				inerts[x, y] += (pos - (Vector2)laserCards[x, y].transform.localPosition) / 50;
				inerts[x, y] *= 0.95f;
			}
		}

		for (var x = 0; x < level.size; x++)
		{
			for (var y = 0; y < level.size; y++)
			{
				laserCards[x, y].transform.localPosition += (Vector3)inerts[x, y];
			}
		}
	}

	private Vector2 GetPos(int x, int y)
	{
		if (x < 0 || x == level.size || y < 0 || y == level.size) return new Vector2(x, y) + Vector2.one / 2;
		return laserCards[x, y].transform.localPosition;
	}

	private void ChangeCards(Vector2Int oldPos, Vector2Int newPos)
	{
		newPos.x = Mathf.Clamp(newPos.x, 0, level.size - 1);
		newPos.y = Mathf.Clamp(newPos.y, 0, level.size - 1);

		if (oldPos == newPos) return;

		var temp = laserCards[oldPos.x, oldPos.y];
		laserCards[oldPos.x, oldPos.y] = laserCards[newPos.x, newPos.y];
		laserCards[newPos.x, newPos.y] = temp;
		laserCards[oldPos.x, oldPos.y].position = oldPos;
		laserCards[newPos.x, newPos.y].position = newPos;

		CreateLaserPositions();
	}

	[Button]
	public void CreateLevel(LaserLevel level)
	{
		this.level = level;

		inerts = new Vector2[level.size, level.size];

		transform.localScale = Vector3.one / level.size;

		ClearGrid();

		laserCards = new LaserCardComponent[level.size, level.size];
		for (int i = 0; i < level.size; i++)
		{
			for (int j = 0; j < level.size; j++)
			{
				laserCards[i, j] = Instantiate(level.LaserCards[i, level.size - 1 - j], transform);
				laserCards[i, j].position = new Vector2Int(i, j);
				laserCards[i, j].OnCardMoved += ChangeCards;
			}
		}

		if (controlPositions != null)
		{
			for (int i = 0; i < controlPositions.Length; i++)
			{
				if (controlPositions[i] != null)
					Destroy(controlPositions[i].gameObject);
			}
		}

		controlPositions = new Transform[level.ends.Length + 1];
		for (int i = 0; i < level.ends.Length; i++)
		{
			controlPositions[i] = Instantiate(emptyObject, transform);
			controlPositions[i].localPosition = new Vector2(level.ends[i].x, level.ends[i].y) + Vector2.one / 2;
		}
		controlPositions[level.ends.Length] = Instantiate(emptyObject, transform);
		controlPositions[level.ends.Length].localPosition = new Vector2(level.start.x, level.start.y) + Vector2.one / 2;


		CreateLaserPositions();
	}

	private void CreateLaserPositions()
	{
		if (effects != null)
			for (int i = 0; i < effects.Count; i++)
				if (effects[i] != null)
					Destroy(effects[i].gameObject);

		effects = new List<LineAnimator>();

		var startMove =
			level.start.x == level.size ? new Vector2Int(-1, 0) :
			level.start.x < 0 ? new Vector2Int(1, 0) :
			level.start.y == level.size ? new Vector2Int(0, -1) :
			level.start.y < 0 ? new Vector2Int(0, 1) :
			new Vector2Int(0, 0);

		used = new bool[level.size, level.size];

		if (isActive == null)
			isActive = new bool[level.ends.Length];

		var oldActive = new bool[level.ends.Length];
		for (int i = 0; i < isActive.Length; i++)
		{
			oldActive[i] = isActive[i];
			isActive[i] = false;
		}
		NextLinePos(level.start, startMove, 0);
		for (int i = 0; i < isActive.Length; i++)
		{
			if (isActive[i] != oldActive[i])
			{
				if (isActive[i])
				{
					Debug.Log("Activated: " + i);
					onActivated?.Invoke(i);
				}
				else
				{
					Debug.Log("Deactivated: " + i);
					onDeactivated?.Invoke(i);
				}
			}
		}
	}

	private Transform GetTransform(Vector2Int pos)
	{
		if (pos == level.start) return controlPositions[controlPositions.Length - 1];
		for (int i = 0; i < level.ends.Length; i++)
		{
			if (pos == level.ends[i])
			{
				return controlPositions[i];
			}
		}

		if (pos.x < 0 || pos.x == level.size || pos.y < 0 || pos.y == level.size) return null;
		return laserCards[pos.x, pos.y].transform;
	}

	private void NextLinePos(Vector2Int lastPos, Vector2Int order, float delay)
	{

		var currentPos = lastPos + order;
		var currentTransform = GetTransform(lastPos + order);

		var line = Instantiate(effectLine, transform);
		effects.Add(line);

		line.SetData(
			GetTransform(lastPos),
			currentTransform,
			order,
			currentTransform == null ? false : controlPositions.Contains(currentTransform) ? true : laserCards[currentPos.x, currentPos.y].HasSignal(-order),
			delay
		);

		if (currentTransform == null)
		{
			return;
		}

		if (controlPositions.Contains(currentTransform))
		{
			isActive[Array.IndexOf(controlPositions, currentTransform)] = true;
			return;
		}

		if (!laserCards[currentPos.x, currentPos.y].HasSignal(-order))
		{
			return;
		}

		if (currentPos.y > -1 && currentPos.x > -1 && currentPos.x < level.size && currentPos.y < level.size && used[currentPos.x, currentPos.y])
		{
			return;
		}

		if (currentPos.y > -1 && currentPos.x > -1 && currentPos.x < level.size && currentPos.y < level.size)
		{
			used[currentPos.x, currentPos.y] = true;
		}

		if (laserCards[currentPos.x, currentPos.y].HasSignal(order))
		{
			NextLinePos(currentPos, order, delay + 0.3f);
		}

		order = new Vector2Int(order.y, order.x);

		if (laserCards[currentPos.x, currentPos.y].HasSignal(order))
		{
			NextLinePos(currentPos, order, delay + 0.3f);
		}

		order *= -1;

		if (laserCards[currentPos.x, currentPos.y].HasSignal(order))
		{
			NextLinePos(currentPos, order, delay + 0.3f);
		}
	}
}
