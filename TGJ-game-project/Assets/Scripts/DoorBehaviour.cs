using System;
using Sirenix.OdinInspector;
using UnityEngine;

public class DoorBehaviour : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }
    
    [Button]
    public void OpenDoor()
    {
        _animator.SetBool("IsOpen", true);
    }
    
    [Button]
    public void CloseDoor()
    {
        _animator.SetBool("IsOpen", false);
    }
    
    [Button]
    public void ToggleDoor()
    {
        bool isOpen = _animator.GetBool("IsOpen");
        _animator.SetBool("IsOpen", !isOpen);
    }
}
