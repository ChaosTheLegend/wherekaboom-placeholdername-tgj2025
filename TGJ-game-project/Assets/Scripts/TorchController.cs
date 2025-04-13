using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class TorchController : MonoBehaviour
{
    public UnityEvent onAllTorchesActivated; // Event to trigger when all torches are deactivated
    
    [SerializeField]
    private List<TorchAnimator> torches; // List of torch objects to be controlled
    
    private List<bool> torchStates; // List to keep track of the state of each torch

    [SerializeField]
    private float activationDelay = 0.5f; // Delay before activating the next torch
    private void Awake()
    {
        torchStates = new List<bool>();
        foreach (var t in torches)
        {
            torchStates.Add(false); // Initialize all torches to inactive state
            t.DeactivateTorch();
        }
    }

    public void DeactivateTorch(int index)
    {
        torchStates[index] = false; // Update the state to false
        if (index >= 0 && index < torches.Count)
        {
            torches[index].DeactivateTorch(); // Deactivate the torch
        }
        else
        {
            Debug.LogWarning("Torch index out of range: " + index);
        }
    }
    
    public void ActivateTorch(int index)
    {
        torchStates[index] = true;
        if (index >= 0 && index < torches.Count)
        {
            ActivateTorchesWithDelay(index).Forgor();
        }
        else
        {
            Debug.LogWarning("Torch index out of range: " + index);
        }
    }
    
    private async UniTaskVoid ActivateTorchesWithDelay(int index)
    {
        await UniTask.Delay(TimeSpan.FromSeconds(activationDelay)); // Wait for the specified delay
        torches[index].ActivateTorch(); // Activate the torch
        CheckAllTorchesAcivated(); // Check if all torches are activated
    }
    
    public void CheckAllTorchesAcivated()
    {
        if (torchStates.Any(state => !state))
        {
            return; // If any torch is still active, exit the method
        }

        onAllTorchesActivated?.Invoke(); // Trigger the event if all torches are deactivated
    }
    
    
}
