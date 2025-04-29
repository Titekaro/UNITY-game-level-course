/*************************************************************
 *************************************************************
 
 - SCRIPT REFERENCE -

 This script links the camera to the player displacement
 from scratch without using the characterController system.

 This script has to be placed on the main camera and works with 
 the "(from scratch)player" script that has to be referenced as 
 the "target" variable of this script.

 This script got replaced with the character script.

 *************************************************************
*************************************************************/

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraToFollowPlayer : MonoBehaviour {
	[SerializeField] private GameObject target;
	[SerializeField] private float cameraDistance = 4;

	// Start is called before the first frame update
	void Start() {
		MatchPosition();
	}

	// Update is called once per frame
	void Update() {
		MatchPosition();
	}

	void MatchPosition() {
		/* Vector3 Constructor - Creates a new vector with given x, y, z components.
		 * Doc:
		 * public Vector3(float x, float y, float z);
		 * https://docs.unity3d.com/ScriptReference/Vector3-ctor.html
		 */
		transform.position = new Vector3(target.transform.position.x, cameraDistance, target.transform.position.z);
	}

}