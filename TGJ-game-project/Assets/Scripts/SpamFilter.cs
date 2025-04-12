using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

public class SpamFilter : MonoBehaviour
{
    [SerializeField]
    private float spamDelay = 0.5f;
    [SerializeField]
    private bool singleUse = false;
    [SerializeField]
    private float activationDelay = 0.5f;
    
    [SerializeField]
    private UnityEvent filterEvent;
    
    private bool isActive;
    
    public void FilterSpam()
    {
        if(isActive) return;
        
        isActive = true;
        
        ActivateSpamFilter().Forget();
        
        if(singleUse) return;
        ResetSpamFilter().Forget();
    }
    
    private async UniTaskVoid ActivateSpamFilter()
    {
        await UniTask.Delay((int)(activationDelay * 1000));
        filterEvent?.Invoke();
    }
    
    private async UniTaskVoid ResetSpamFilter()
    {
        await UniTask.Delay((int)(spamDelay * 1000));
        isActive = false;
    }
}
