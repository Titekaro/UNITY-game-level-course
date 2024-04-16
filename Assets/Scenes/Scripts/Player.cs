using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : MonoBehaviour
{
    /* Animator - Interface to control the Mecanim animation system.
     * Doc:
     * https://docs.unity3d.com/ScriptReference/Animator.html
     */
    [SerializeField] private Animator playerAnimator;
    private PlayerInputActions playerInputActions;
    [SerializeField] private float moveSpeed = 0.7f;
    [SerializeField] private float rotationSpeed = 20f;
    private bool isPlayerMoving = false; // Variable used to set walking animation if player is walking

    void Awake() {
        playerInputActions = new PlayerInputActions();
        playerInputActions.Player.Move.Enable();
    }
    void Start() {
	}
	void Update() {
        PlayerMovement();
        PlayerInteraction();
    }

    void PlayerMovement() {
        /* Vector2 constructor - Representation of 2D vectors and points.
         * Doc:
         * public Vector2(float x, float y);
         * https://docs.unity3d.com/ScriptReference/Vector2.html
         */
		Vector2 keyCoords = new Vector2(0, 0); // X Y

        // Fetch configured inputs in playerInputActions asset
        keyCoords = playerInputActions.Player.Move.ReadValue<Vector2>();

        /* Vector3 Constructor - Creates a new vector with given x, y, z components.
		 * Doc:
		 * public Vector3(float x, float y, float z);
		 * https://docs.unity3d.com/ScriptReference/Vector3-ctor.html
		 */
        // Make player move according to keyboard input coords.
        Vector3 playerCoords = new Vector3(keyCoords.x, 0f, keyCoords.y); // /!\ X Z Y because we need to transpose y position to z position for this usecase (3D move spatialisation)

        /* Physics.CapsuleCast - Casts a capsule against all colliders in the Scene and returns detailed information on what was hit.
         * Doc:
         * public static bool CapsuleCast(Vector3 point1, Vector3 point2, float radius, Vector3 direction, out RaycastHit hitInfo, float maxDistance)
         * https://docs.unity3d.com/ScriptReference/Physics.CapsuleCast.html
         */
        // Define the check capsule
        bool canPlayerMove = !Physics.CapsuleCast(
            transform.position,
            transform.position + Vector3.up * 2, // Vector3.up - Shorthand for writing Vector3(0, 1, 0).
            0.6f,
            playerCoords,
            moveSpeed * Time.deltaTime
        );

        // Define what should happen if player can move
        if(canPlayerMove) {
            // Check if player is moving
            isPlayerMoving = (playerCoords != Vector3.zero);

            // Set walking animation: "is_walking" refers to the name of the boolean set in the animation window for the walking animation.
            playerAnimator.SetBool("is_walking", isPlayerMoving);
            transform.position = transform.position + playerCoords * Time.deltaTime * moveSpeed; // Time.deltaTime pour uniformiser la vitesse de déplacement entre les différents supports d'exécution du jeu
        }

        /* Vector3.Slerp - Spherically interpolates between two vectors.
         * Doc:
         * public static Vector3 Slerp(Vector3 a, Vector3 b, float t);
         * https://docs.unity3d.com/ScriptReference/Vector3.Slerp.html
         */
        transform.forward = Vector3.Slerp(transform.forward, playerCoords, Time.deltaTime * rotationSpeed); // Orientation of player
    }

    void PlayerInteraction() {
        // Get player orientation
        Vector3 playerDirection = transform.forward;

        /* Physics.Raycast - Casts a ray, from point origin, in direction direction, of length maxDistance, against all colliders in the Scene.
         * You may optionally provide a LayerMask, to filter out any Colliders you aren't interested in generating collisions with.
         * Doc:
         * public static bool Raycast(Vector3 origin, Vector3 direction, out RaycastHit hitInfo, float maxDistance, int layerMask, QueryTriggerInteraction queryTriggerInteraction);
         * https://docs.unity3d.com/ScriptReference/Physics.Raycast.html
         */
         // Send Raycast to detect interactions and get touched object.
         // If touched object has an interectable component, call his interact method.
         if(Physics.Raycast(transform.position, playerDirection, out RaycastHit hitObj, 1f, LayerMask.GetMask("Interactable"))){
            Debug.Log("touché: " + hitObj);
            if(hitObj.transform.TryGetComponent<Interactable>(out Interactable interactableObj)){
                interactableObj.Interact();
            }
         }
    }

}
