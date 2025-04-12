using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class InteractiveButton : SelectableObject
{
    [SerializeField]
    private UnityEvent onButtonPress;
    
    [SerializeField]
    private bool oneTimePress = false;
    
    [SerializeField]
    private float pressDelay = 0.5f;
    
    [SerializeField]
    private bool interactable = true;
    
    public override void OnClick()
    {
        if(!interactable) return;
        
        interactable = false;
        if(!oneTimePress) ResetInteractable().Forget();
        base.OnClick();
        onButtonPress?.Invoke();
    }
    
    private async UniTaskVoid ResetInteractable()
    {
        await UniTask.Delay((int)(pressDelay * 1000));
        interactable = true;
    }
}
