using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;

public class CoinGenerator : MonoBehaviour
{
    [SerializeField]
    private Vector3 coinEndPosition = new Vector3(0, 0, 0);
    
    [SerializeField]
    private Transform coin;
    
    [Button]
    private void RecordEndPosition()
    {
        // Record the start position of the coin
        coinEndPosition = coin.position;
    }
    
    
    
    [Button]
    public void MoveCoin()
    {
        coin.DOMove(coinEndPosition, 1f)
            .SetEase(Ease.OutBack);
    }
}
