using System;
using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TorchAnimator : MonoBehaviour
{
    [SerializeField]
    private Light light;
    [SerializeField]
    private ParticleSystem particles;
    [SerializeField] 
    private bool hasDecals;
    [ShowIf("hasDecals")]
    [SerializeField]
    private DecalProjector projector;

    private void Awake()
    {
        //set light to 0
        light.intensity = 0f;
        //set particles to 0
        particles.Stop();
        //set projector to 0
        if (!hasDecals) return;
        projector.material.SetFloat("_Power", 0f);
    }
    
    public void ActivateTorch()
    {
        //light up
        light.DOIntensity(15f, 2f)
            .SetEase(Ease.InQuad);
        particles.Play();
        if (!hasDecals) return;
        projector.material.DOFloat(15f, "_Power", 2f)
            .SetEase(Ease.InQuad);
    }
    
    public void DeactivateTorch()
    {
        //light down
        light.DOIntensity(0f, 2f)
            .SetEase(Ease.InQuad);
        particles.Stop();
        if (!hasDecals) return;
        projector.material.DOFloat(0f, "_Power", 2f)
            .SetEase(Ease.InQuad);
    }
    
}
