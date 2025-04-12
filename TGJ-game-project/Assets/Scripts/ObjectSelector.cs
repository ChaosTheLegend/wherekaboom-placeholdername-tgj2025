using UnityEngine;

public class ObjectSelector : MonoBehaviour
{
    [SerializeField]
    private LayerMask selectableObjectMask;
    [SerializeField]
    private Transform cameraTransform;
    
    [SerializeField]
    private float selectionDistance = 5f;
    
    // Update is called once per frame
    private void Update()
    {
        //raycast to check if the player is looking at a selectable object
        RaycastHit hit;
        var forward = cameraTransform.TransformDirection(Vector3.forward);
        var origin = cameraTransform.position;
        if (Physics.Raycast(origin, forward, out hit, selectionDistance, selectableObjectMask))
        {
            SelectableObject selectableObject = hit.collider.GetComponent<SelectableObject>();
            if (selectableObject != null)
            {
                selectableObject.OnLookAt();
                if (Input.GetMouseButtonDown(0))
                {
                    selectableObject.OnClick();
                }
            }
        }
    }
}
