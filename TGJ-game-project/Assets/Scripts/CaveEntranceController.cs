using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

public class CaveEntranceController : MonoBehaviour
{
    [SerializeField]
    private List<Rigidbody> rocks;
   
    private bool isOpen = false; // Flag to check if the cave entrance is open
    
    [Button]
    public void OpenCaveEntrance()
    {
        if(isOpen) return;
        
        isOpen = true;
        foreach (var rock in rocks)
        {
            rock.isKinematic = false; // Make the rock non-kinematic to allow physics to affect it
            var randomDirection = Random.insideUnitSphere; // Get a random direction
            rock.AddForce(randomDirection * 5f, ForceMode.Impulse); // Apply an upward force to simulate an explosion
        }
    }
}
