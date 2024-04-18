using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Rendering.LookDev;
using UnityEngine;
using UnityEngine.InputSystem;

public class Character : MonoBehaviour
{
    /* CharacterController - Allows you to easily do movement constrained by collisions without having to deal with a rigidbody.
     * Doc:
     * https://docs.unity3d.com/ScriptReference/CharacterController.html
     */
    private CharacterController characterController;
    private Camera characterCamera;
    [SerializeField] private int moveSpeed = 5;
    [SerializeField] private int jumpSpeed = 5;
    [SerializeField] private int characterMass = 1;
    private Vector3 velocity;
    private Vector3 fieldOfView;
    [SerializeField] private Interactable interactableTarget;

    void Awake() {
        characterController = GetComponent<CharacterController>(); // Set up CharacterController
        characterCamera = GameObject.Find("Main Camera").GetComponent<Camera>(); // Set the main camera of the scene as the characterCamera

        //playerInputActions = new PlayerInputActions();
        //playerInputActions.Player.Enable();
        //playerInputActions.Player.Use.performed += PlayerInteractionListener; // Link event listener to the defined player action "use"
    }

    void Start() {
	}

	void Update() {
        CharacterMovement();
        ViewControl();
        PlayerInteraction();
    }

    void CharacterMovement() {
        float x = Input.GetAxis("Horizontal"); // Get the value of x axis
        float y = Input.GetAxis("Vertical"); // Get the value of y axis

        /* Vector3.ClampMagnitude - Returns a copy of vector with its magnitude clamped to maxLength.
         * Doc:
         * public static Vector3 ClampMagnitude(Vector3 vector, float maxLength);
         * 
         */
        // Build a Vector3 with x, y, z(0) values & maxLength that will be the player coords
        Vector3 input = new Vector3();
        input += transform.forward * y; // Add value of y axis to vector
        input += transform.right * x; // Add value of x axis to vector
        input = Vector3.ClampMagnitude(input, 1f);

        characterController.Move((input * moveSpeed + velocity) * Time.deltaTime); // Deplacement du personnage via le vecteur de deplacement dans le characterController

        /* Input - KeyCode maps to physical keys
         * Doc:
         * https://docs.unity3d.com/ScriptReference/Input.html
         */
        if(Input.GetButtonDown("Jump") && characterController.isGrounded){
            velocity.y += jumpSpeed; // Add jump speed to vector
        }

    }

    void ViewControl() {
        fieldOfView.x += Input.GetAxis("Horizontal"); // Récupère la valeur de l'axe horizontal de la souris
        fieldOfView.y += Input.GetAxis("Vertical"); // Récupère la valeur de l'axe vertical de la souris
        fieldOfView.y = Mathf.Clamp(fieldOfView.y, -80f, 80f); // Limite la valeur de l'axe vertical de la souris

        characterCamera.transform.localRotation = Quaternion.Euler(-fieldOfView.y * 3, 0.0f, 0.0f); // Rotation de la camera (verticale)
        transform.localRotation = Quaternion.Euler(0.0f, fieldOfView.x * 3, 0.0f); // Rotation du personnage (horizontale)
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
         // UPDATE: changed Raycast bu CapsuleCast because Raycast emits a ray only at y 0 which is problematic if player is not correctly aligned with object.
         if(Physics.Raycast(transform.position, playerDirection, out RaycastHit hitObj, 1f, LayerMask.GetMask("Usable"))){
            Debug.Log(hitObj);
            if(hitObj.transform.TryGetComponent<Interactable>(out Interactable interactableObj)){
                Debug.Log("interactable obj");
                interactableTarget = interactableObj;
            } 
            if(hitObj.transform.TryGetComponent<Collectable>(out Collectable collectableObj)){
                Debug.Log("collectable obj");
                collectableObj.CollectObject();
            }
         } else {
            interactableTarget = null;
        }
    }

    void PlayerInteractionListener(InputAction.CallbackContext context) {
        Debug.Log("listener interacté");
        interactableTarget?.Interact();
        //playerAnimator.SetTrigger("is_interacting");
    }

}
