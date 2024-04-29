using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class Character : MonoBehaviour
{
    /* CharacterController - Allows you to easily do movement constrained by collisions without having to deal with a rigidbody.
     * Doc:
     * https://docs.unity3d.com/ScriptReference/CharacterController.html
     */
    private CharacterController characterController;
    private Animator characterAnimator;
    private Camera characterCamera;
    private float cameraHeight = 1.5f;
    private int cameraDistance = 2;
    private Vector3 velocity;
    private Vector3 gravity;
    private (Vector3, quaternion) characterInitialPosition;
    private bool isCharacterWalking = false;
    private GameManager gameManager;

    [SerializeField] private int fallLimit = -50;
    [SerializeField] private int moveSpeed = 5;
    [SerializeField] private int jumpSpeed = 5;
    [SerializeField] private int rotationSpeed = 10; 
    [SerializeField] private int characterMass = 1;
    [SerializeField] private Interactable interactableTarget;

    private PlayerInput characterInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private InputAction interactAction;

    void Awake() {
        characterController = GetComponent<CharacterController>(); // Set up CharacterController
        characterAnimator = GameObject.Find("Visual (empty)").GetComponent<Animator>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        characterCamera = GameObject.Find("Main Camera").GetComponent<Camera>(); // Set the main camera of the scene as the characterCamera
        // Set up the camera position at right distance of character position
        characterCamera.transform.position = new Vector3(characterCamera.transform.position.x, characterCamera.transform.position.y + cameraHeight, characterCamera.transform.position.z - cameraDistance);
        characterInitialPosition = (transform.position, transform.rotation);

        // Get the character inputs
        characterInput = GetComponent<PlayerInput>();
        moveAction = characterInput.actions["Move"];
        jumpAction = characterInput.actions["Jump"];
        interactAction = characterInput.actions["Interact"];
    }

    void Start() {
	}

	void Update() {
        CharacterGravity();
        CharacterMovement();
        CheckCharacterPosition();

        CharacterInteraction();
    }

    void CharacterGravity() {
        gravity = Physics.gravity * characterMass * Time.deltaTime;
        velocity.y = characterController.isGrounded ? -1 : velocity.y + gravity.y;
    }

    void CharacterMovement() {
        // Set up the character displacement according to axis values returned by the keyboard
        Vector2 axisInput = moveAction.ReadValue<Vector2>();

        /* Vector3.ClampMagnitude - Returns a copy of vector with its magnitude clamped to maxLength.
         * Doc:
         * public static Vector3 ClampMagnitude(Vector3 vector, float maxLength);
         * 
         */
        // Build a Vector3 with x, y, z values that will be the player coords at move
        Vector3 moveCoords = new Vector3(axisInput.x, 0, axisInput.y);
        //moveCoords = Vector3.ClampMagnitude(moveCoords, 1f); // MaxLength

        // Launch walking animation if character is moving
        if(moveCoords != new Vector3(0,0,0)) {
            isCharacterWalking = true;
        } else {
            isCharacterWalking = false;
        }
        characterAnimator.SetBool("is_walking", isCharacterWalking);

        /* Input - KeyCode maps to physical keys
         * Doc:
         * https://docs.unity3d.com/ScriptReference/Input.html
         */
        // With Input.GetButtonDown("Jump"), detect the jump movement set up in Edit > Project Settings > Input Manager > Jump
        float jumpInput = jumpAction.ReadValue<float>();
        if(jumpInput > 0 && characterController.isGrounded){
            velocity.y += jumpSpeed; // Add jump speed to vector
        }

        // Move the character according to the moveCoords by using the Move() property of characterController
        characterController.Move((moveCoords * moveSpeed + velocity) * Time.deltaTime);

        // Make character rotate according to the move coords
        transform.forward = Vector3.Slerp(transform.forward, moveCoords, Time.deltaTime * rotationSpeed);

    }

    void CheckCharacterPosition() {
         if(transform.position.y < fallLimit) {
            (transform.position, transform.rotation) = characterInitialPosition;
            gameManager.UpdateLife();
            ResetCharacterPosition(transform.position, transform.rotation);
        }
    }

    void ResetCharacterPosition(Vector3 position, Quaternion rotation) {
        transform.position = position;
        transform.rotation = rotation;
        Physics.SyncTransforms();
        velocity = Vector3.zero;
    }

    void ViewControl() {
        // Set up the ability to look around. First step, get the axis values returned by the keyboard.
        //fieldOfView.x = Input.GetAxis("Horizontal"); // Récupère la valeur de l'axe horizontal
        //fieldOfView.y += Input.GetAxis("Vertical"); // Récupère la valeur de l'axe vertical
        //fieldOfView.y = Mathf.Clamp(fieldOfView.y, -80f, 80f); // Limite la valeur de l'axe vertical de la souris

        //characterCamera.transform.rotation = Quaternion.Euler(0, fieldOfView.x, 0);

        // Set up the camera rotation according to the character rotation
        //characterCamera.transform.rotation = Quaternion.Euler(characterController.transform.rotation.x, characterController.transform.rotation.y, characterController.transform.rotation.z);
        //characterCamera.transform.Rotate(new Vector3(0, fieldOfView.x, 0));
        //characterCamera.transform.localRotation = Quaternion.Euler(-fieldOfView.y * 3, 0.0f, 0.0f); // Rotation de la camera (verticale)
        //transform.localRotation = Quaternion.Euler(0.0f, fieldOfView.x * 3, 0.0f); // Rotation du personnage (horizontale)
    }

    void CharacterInteraction() {
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
         // @TODO: change Raycast bu CapsuleCast because Raycast emits a ray only at y 0 which is problematic if player is not correctly aligned with object.
         if(Physics.Raycast(transform.position, playerDirection, out RaycastHit hitObj, 1f, LayerMask.GetMask("Usable"))){
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

        OnInteractInput();
    }

    void OnInteractInput() {
        float interactInput = interactAction.ReadValue<float>();
        Debug.Log(interactAction);
        if(interactInput > 0 && interactableTarget) {
            Debug.Log("interacté");
            interactableTarget.Interact();
            //playerAnimator.SetTrigger("is_interacting");
        }
    }

}
