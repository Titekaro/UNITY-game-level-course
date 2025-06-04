using Unity.XR.CoreUtils;
using UnityEngine;

public class BodyPocket : MonoBehaviour
{
    [SerializeField]
    [Tooltip("The camera that the Gameobject will follow")] Camera Camera;

    private Vector3 currentPosition;
    private Quaternion currentRotation;

    void Update()
    {
        currentPosition = Camera.transform.position;
        currentRotation = Camera.transform.rotation;

        if(currentPosition != this.transform.position) {
            UpdateBodyPocketPosition();
        }
    
    }

    private void UpdateBodyPocketPosition()
    {
        this.transform.position = new Vector3(currentPosition.x, currentPosition.y, currentPosition.z);
        //this.transform.rotation = new Quaternion(currentRotation.x, currentRotation.y, currentRotation.z, currentRotation.w);
        
        Debug.Log(currentPosition + " " + this.transform.position);
    }
    
}
