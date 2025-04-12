using UnityEngine;
using UnityEngine.Events;

public class SelectableObject : MonoBehaviour
{
    [SerializeField]
    private UnityEvent onClick;
    [SerializeField]
    private UnityEvent onLookAt;
    
    public virtual void OnLookAt()
    {
        onLookAt?.Invoke();
    }
    
    public virtual void OnClick()
    {
        onClick?.Invoke();
    }
}
