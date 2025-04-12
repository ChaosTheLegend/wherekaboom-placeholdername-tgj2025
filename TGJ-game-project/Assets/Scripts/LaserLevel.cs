using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "ScriptableObjects/LaserLevel", order = 1)]
public class LaserLevel : ScriptableObject
{
	public Vector2Int start;
	public Vector2Int[] ends;

	[HideInInspector]
	public int size = 3;

	[HideInInspector]
	[SerializeField]
	private LaserCardComponent[] laserCards = new LaserCardComponent[9];

	[ShowInInspector]
	private int Size
	{
		get => size;
		set
		{
			if (value < 1)
			{
				return;
			}
			size = value;
			laserCards = new LaserCardComponent[size * size];
		}
	}

	[ShowInInspector]
	public LaserCardComponent[,]
	LaserCards
	{
		get
		{
			LaserCardComponent[,] cards = new LaserCardComponent[size, size];
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					cards[i, j] = laserCards[i * size + j];
				}
			}
			return cards;
		}

		set
		{
			for (int i = 0; i < size; i++)
			{
				for (int j = 0; j < size; j++)
				{
					laserCards[i * size + j] = value[i, j];
				}
			}
		}
	}
}
