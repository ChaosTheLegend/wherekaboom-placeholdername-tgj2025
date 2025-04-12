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
    private List<GameObject> torchObjects; // List of torch objects to be controlled
    
    private List<bool> torchStates; // List to keep track of the state of each torch

    [SerializeField]
    private float activationDelay = 0.5f; // Delay before activating the next torch
    private void Awake()
    {
        torchStates = new List<bool>();
        for (int i = 0; i < torchObjects.Count; i++)
        {
            torchStates.Add(false); // Initialize all torches to inactive state
            torchObjects[i].SetActive(false); // Ensure all torches are initially deactivated
        }
        
        
    }

    public void DeactivateTorch(int index)
    {
        torchStates[index] = false; // Update the state to false
        if (index >= 0 && index < torchObjects.Count)
        {
            torchObjects[index].SetActive(false);
        }
        else
        {
            Debug.LogWarning("Torch index out of range: " + index);
        }
    }
    
    public void ActivateTorch(int index)
    {
        torchStates[index] = true;
        if (index >= 0 && index < torchObjects.Count)
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
        torchObjects[index].SetActive(true);
    }
    
    public void CheckAllTorchesDeactivated()
    {
        if (torchStates.Any(state => !state))
        {
            return; // If any torch is still active, exit the method
        }

        onAllTorchesActivated?.Invoke(); // Trigger the event if all torches are deactivated
    }
    
    
}
