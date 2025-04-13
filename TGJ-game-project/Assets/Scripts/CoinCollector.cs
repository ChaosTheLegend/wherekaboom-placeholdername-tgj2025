using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [SerializeField]
    private List<bool> coinStates; // List to keep track of the state of each coin
    [SerializeField]
    private CutSceneAnimator cutSceneAnimator;

    [Button]
    private void Sync()
    {
        for (var i = 0; i < coinStates.Count; i++)
        {
            var coin = coinStates[i];
            if(!coin) continue;
            cutSceneAnimator.SetHave(i);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            //get the name of the object and extract the number
            string name = other.gameObject.name;
            int index = int.Parse(name.Substring(name.Length - 1)); // Assuming the last character is the index
            
            // Check if the index is within the bounds of the coinStates list
            if (index >= 0 && index < coinStates.Count)
            {
                coinStates[index] = true; // Update the state to true
                Destroy(other.gameObject); // Destroy the coin object
                cutSceneAnimator.SetHave(index);
            }
            else
            {
                Debug.LogWarning("Coin index out of range: " + index);
            }
        }
    }
}
