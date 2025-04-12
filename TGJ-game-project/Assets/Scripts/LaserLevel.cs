using Sirenix.OdinInspector;
using UnityEngine;

[CreateAssetMenu(fileName = "NewLevel", menuName = "ScriptableObjects/LaserLevel", order = 1)]
public class LaserLevel : ScriptableObject
{
	public Vector2Int start;
	public Texture2D startIcon;
	public Vector2Int[] ends;
	public Texture2D[] endIcons;

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


	[Button]
	private void Shuffle()
	{
		for (int i = 0; i < laserCards.Length; i++)
		{
			int randomIndex = Random.Range(0, laserCards.Length);
			(laserCards[i], laserCards[randomIndex]) = (laserCards[randomIndex], laserCards[i]);
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
