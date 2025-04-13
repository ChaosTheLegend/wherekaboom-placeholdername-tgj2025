using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GraphtyEnabler : MonoBehaviour
{
    [SerializeField]
    private DecalProjector projector;
    
    [SerializeField]
    private float maxEmission = 15f;

    private void Start()
    {
        projector.material.SetFloat("_Power", 0f);
    }

    [Button]
    public void LightUpGraphty()
    {
        var material = projector.material;

        material.DOFloat(maxEmission, "_Power", 2f)
            .SetEase(Ease.InQuad);
    }
}
