using System;
using System.Collections.Generic;
using UnityEngine;

public class CoinCollector : MonoBehaviour
{
    [SerializeField]
    private List<bool> coinStates; // List to keep track of the state of each coin

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
            }
            else
            {
                Debug.LogWarning("Coin index out of range: " + index);
            }
        }
    }
}
