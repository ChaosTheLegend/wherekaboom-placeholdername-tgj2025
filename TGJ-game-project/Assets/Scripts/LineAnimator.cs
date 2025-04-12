using DG.Tweening;
using UnityEngine;

public class LineAnimator : MonoBehaviour
{
	private Transform startPoint;
	private Transform endPoint;
	private Vector2 order;
	private bool isShort;
	private bool isFinishable;
	[SerializeField] private LineRenderer line;

	public void SetData(Transform start, Transform end, Vector2Int order, bool finishable, float delay)
	{
		if (start == null) { Debug.Log(order); }

		startPoint = start;
		endPoint = end;
		this.order = order;
		isFinishable = finishable;
		isShort = endPoint == null;



		if (isShort)
			line.positionCount = 2;
		else if (finishable)
			line.positionCount = 4;
		else
			line.positionCount = 3;
		line.material.SetFloat("_Float", 0);
		line.material.DOFloat(1, "_Float", 0.5f).SetDelay(delay).OnComplete(() => { line.material.SetFloat("_Float", 10); });
	}

	private void Start()
	{
		line.SetPosition(0, startPoint.localPosition);

		line.SetPosition(1, startPoint.localPosition + (Vector3)order * 0.4f);

		if (!isShort)
			line.SetPosition(2, endPoint.localPosition - (Vector3)order * 0.4f);

		if (!isShort && isFinishable)
			line.SetPosition(3, endPoint.localPosition);
	}

	private void Update()
	{
		line.SetPosition(0, startPoint.localPosition);

		line.SetPosition(1, startPoint.localPosition + (Vector3)order * 0.4f);

		if (!isShort)
			line.SetPosition(2, endPoint.localPosition - (Vector3)order * 0.4f);

		if (!isShort && isFinishable)
			line.SetPosition(3, endPoint.localPosition);
	}
}
