using Cysharp.Threading.Tasks;
using DefaultNamespace;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class CutSceneAnimator : MonoBehaviour
{
	[SerializeField]
	private Transform[] stoneCoins;

	[SerializeField]
	private ParticleSystem[] particleSystems;

	[SerializeField]
	private Camera cameraForCutScene;

	[SerializeField]
	private Transform endPosition;

	[SerializeField]
	private Renderer[] stars;
	[SerializeField]
	private ParticleSystem[] starsPart;

	[SerializeField]
	private TrailRenderer trailRenderer;

	private bool[] isAnimated;
	private bool[] isHave;

	[Button]
	private void SetHave(int id)
	{
		/*if (id < 0 || id >= isHave.Length)
		{
			Debug.LogError("Invalid ID: " + id);
			return;
		}
		isHave[id] = true;*/

		//all true
		for (int i = 0; i < isHave.Length; i++)
		{
			isHave[i] = true;
		}
	}

	void Start()
	{
		foreach (var stoneCoin in stoneCoins)
		{
			stoneCoin.localScale = Vector3.zero;
		}
		isHave = new bool[stoneCoins.Length];
		isAnimated = new bool[stoneCoins.Length];
		//stars black
		for (int i = 0; i < stars.Length; i++)
		{
			stars[i].material.color = Color.black;
		}
	}


	[Button]
	public void StartAnimation()
	{
		var rang = 0;
		for (int i = 0; i < stoneCoins.Length; i++)
		{
			if (isHave[i] && !isAnimated[i])
			{
				isAnimated[i] = true;
				stoneCoins[i].localScale = Vector3.zero;
				var position = stoneCoins[i].position;
				stoneCoins[i].position = Camera.main.transform.position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
				stoneCoins[i].gameObject.SetActive(true);

				var j = i;
				stoneCoins[i].DOScale(0.15f, 1.5f).SetDelay(0.3f * rang).SetEase(Ease.Linear);
				stoneCoins[i].DOMove(position, 1.5f).SetDelay(0.3f * rang).SetEase(Ease.Linear).OnComplete(() =>
				{
					particleSystems[j].Play();
				});
				//rotate x from -360 to 0
				DOVirtual.Float(-360, 0, 1.5f, (x) =>
				{
					stoneCoins[j].localRotation = Quaternion.Euler(x, 0, 0);
				}).SetDelay(0.3f * rang).SetEase(Ease.Linear);
				rang++;
			}
		}

		//if all aimated is true -> do delay call range * 0.3f + 1.5f start cut scene
		bool allAnimated = true;
		for (int i = 0; i < isAnimated.Length; i++)
		{
			if (!isAnimated[i])
			{
				allAnimated = false;
				break;
			}
		}
		if (allAnimated)
		{
			DOVirtual.DelayedCall(rang * 0.3f + 1.5f, () =>
			{
				StartCutScene().Forgor();
			});
		}
	}

	[Button]
	//Start cut scene
	public async UniTask StartCutScene()
	{
		var pos = cameraForCutScene.transform.position;
		var rot = cameraForCutScene.transform.rotation;

		var mainCamera = Camera.main.transform;

		cameraForCutScene.transform.position = mainCamera.position;
		cameraForCutScene.transform.rotation = mainCamera.rotation;

		cameraForCutScene.gameObject.SetActive(true);
		mainCamera.gameObject.SetActive(false);

		DOVirtual.Vector3(cameraForCutScene.transform.position, pos, 8f, (x) =>
		{
			cameraForCutScene.transform.position = x;
		}).SetEase(Ease.Linear);
		await DOVirtual.Float(0, 1, 8f, (x) =>
		{
			cameraForCutScene.transform.rotation = Quaternion.Lerp(cameraForCutScene.transform.rotation, rot, x);
		}).SetEase(Ease.Linear).AsyncWaitForCompletion();

		//go to end position
		DOVirtual.Vector3(cameraForCutScene.transform.position, endPosition.position, 8f, (x) =>
		{
			cameraForCutScene.transform.position = x;
		}).SetEase(Ease.Linear);
		DOVirtual.Float(0, 1, 8f, (x) =>
		{
			cameraForCutScene.transform.rotation = Quaternion.Lerp(cameraForCutScene.transform.rotation, endPosition.rotation, x);
		}).SetEase(Ease.Linear);

		await trailRenderer.transform.DOMove(stars[0].transform.position, 8f).SetEase(Ease.Linear).AsyncWaitForCompletion();
		stars[0].material.color = Color.white;
		starsPart[0].Play();

		for (int i = 1; i < stars.Length; i++)
		{
			var j = i;
			await trailRenderer.transform.DOMove(stars[j].transform.position, 0.7f).SetEase(Ease.Linear).AsyncWaitForCompletion();
			stars[j].material.color = Color.white;
			starsPart[j].Play();
		}
	}
}
