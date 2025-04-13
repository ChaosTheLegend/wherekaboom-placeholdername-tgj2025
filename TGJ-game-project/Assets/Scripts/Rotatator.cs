using System;
using DG.Tweening;
using UnityEngine;

public class Rotatator : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 10f;

    

    private void Start()
    {
        //dorotate
        transform.DORotate(new Vector3(0, 360, 0), rotationSpeed, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);
    }
}
