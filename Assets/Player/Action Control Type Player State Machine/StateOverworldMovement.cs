using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

/// <summary>
///     In this script the following functionality is coded:
///     - Overworld Movement
///     - Detect Interactable, Inspectable and Conversable Objects
/// </summary>
public class StateOverworldMovement : MonoBehaviour, IControlTypeState
{
    private static readonly int IsMoving = Animator.StringToHash("isMoving");
    private static readonly int IsGrounded = Animator.StringToHash("isGrounded");

    [SerializeField] private InputActionReference inputWalking;
    [SerializeField] private InputActionReference inputJumping;
    [SerializeField] private InputActionReference inputInteracting;
    [SerializeField] private InputActionReference inputInspecting;
    [SerializeField] private InputActionReference inputListening;
    [SerializeField] private InputActionReference inputTalking;
    [SerializeField] private InputActionReference inputToCrafting;
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float forceOfInitialJump;
    [SerializeField] private float rateAtWhichForceOfJumpDiminishes;
    [SerializeField] private float coyoteTime;
    [SerializeField] private float jumpBufferTime;
    [SerializeField] private Animator playerCharAnimator;

    private CharacterController character;
    private float coyoteTimer;
    private IConversable currentConversable;
    private IInspectable currentInspectable;
    private IInspectable[] currentInspectables;

    private IInteractable currentInteractable;
    private float jumpBufferTimer;
    private float jumpForce;
    private bool jumpQueued;

    

    private UIController uiController;

    private void Awake()
    {
        if (!TryGetComponent(out character))
            throw new NullReferenceException();
        jumpForce = 0;
        jumpQueued = false;
    }

    private void Start()
    {
        uiController = UIController.instance;
        currentInspectables = new[] { (IInspectable)null };
        playerCharAnimator.SetBool(IsGrounded, true);
    }

    private void Update()
    {
        coyoteTimer = character.isGrounded ? coyoteTime : Mathf.Max(0f, coyoteTimer - Time.deltaTime);

        jumpBufferTimer = Mathf.Max(0f, jumpBufferTimer - Time.deltaTime);

        if (jumpQueued && jumpBufferTimer > 0f && coyoteTimer > 0f)
        {
            jumpForce = forceOfInitialJump;
            jumpQueued = false;
            jumpBufferTimer = 0f;
            coyoteTimer = 0f;
            playerCharAnimator.SetBool(IsGrounded, false);
        }

        if (!character.isGrounded)
        {
            jumpForce -= rateAtWhichForceOfJumpDiminishes * Time.deltaTime;
        }
        else if (jumpForce < 0)
        {
            jumpForce = 0;
            playerCharAnimator.SetBool(IsGrounded, true);
        }

        Vector2 inputDirectionVector = inputWalking.action.ReadValue<Vector2>().normalized * walkingSpeed;
        Vector3 movementVector = new(inputDirectionVector.x, jumpForce, inputDirectionVector.y);

        playerCharAnimator.SetBool(IsMoving, inputWalking.action.ReadValue<Vector2>().magnitude > 0f);
        character.Move(movementVector * Time.deltaTime);
    }

    private void OnEnable()
    {
        inputWalking.action.Enable();
        inputJumping.action.Enable();
        inputInteracting.action.Enable();
        inputInspecting.action.Enable();
        inputListening.action.Enable();
        inputTalking.action.Enable();
        inputToCrafting.action.Enable();

        inputJumping.action.performed += OnInputActionPerformedInputJumping;
        inputInteracting.action.performed += OnInputActionPerformedInputInteracting;
        inputInspecting.action.performed += OnInputActionPerformedInputInspecting;
        inputListening.action.performed += OnInputActionPerformedInputListening;
        inputTalking.action.performed += OnInputActionPerformedInputTalking;
        inputToCrafting.action.performed += OnInputActionPerformedChangeToCraftingState;

        if (!uiController.IsUnityNull())
            uiController.EditUIHighlighters(true);

    }

    private void OnDisable()
    {
        inputJumping.action.performed -= OnInputActionPerformedInputJumping;
        inputInteracting.action.performed -= OnInputActionPerformedInputInteracting;
        inputInspecting.action.performed -= OnInputActionPerformedInputInspecting;
        inputListening.action.performed -= OnInputActionPerformedInputListening;
        inputTalking.action.performed -= OnInputActionPerformedInputTalking;
        inputToCrafting.action.performed -= OnInputActionPerformedChangeToCraftingState;

        inputWalking.action.Disable();
        inputJumping.action.Disable();
        inputInteracting.action.Disable();
        inputInspecting.action.Disable();
        inputListening.action.Disable();
        inputTalking.action.Disable();
        inputToCrafting.action.Disable();

        if (!uiController.IsUnityNull())
            uiController.EditUIHighlighters(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if (other.TryGetComponent(out IInspectable inspectable))
        {
            // >>>> set a bool check to true and show UI element to communicate to player
            uiController.ShowUIElementInspect();
            currentInspectable = inspectable;

            currentInspectables = other.GetComponents<IInspectable>();
        }

        if (other.TryGetComponent(out IInteractable interactable))
        {
            // >>>> set a bool check to true and show UI element to communicate to player
            uiController.ShowUIElementInteract();
            currentInteractable = interactable;
        }

        if (other.TryGetComponent(out IConversable conversable))
        {
            // >>>> set a bool check to true and show UI element to communicate to player
            uiController.ShowUIElementListenOrTalk();
            currentConversable = conversable;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IInspectable>(out _))
        {
            uiController.HideUIElementInspect();
            currentInspectable = null;

            currentInspectables = new[] { (IInspectable)null };
        }

        if (other.TryGetComponent<IInteractable>(out _))
        {
            uiController.HideUIElementInteract();
            currentInteractable = null;
        }

        if (other.TryGetComponent<IConversable>(out _))
        {
            uiController.HideUIElementListenOrTalk();
            currentConversable = null;
        }
    }

    public void ExitState()
    {
        enabled = false;
    }

    public void EnterState()
    {
        enabled = true;
    }

    private void OnInputActionPerformedChangeToCraftingState(InputAction.CallbackContext context)
    {
    }

    private void OnInputActionPerformedInputJumping(InputAction.CallbackContext context)
    {
        jumpQueued = true;
        jumpBufferTimer = jumpBufferTime;
    }

    private void OnInputActionPerformedInputInteracting(InputAction.CallbackContext context)
    {
        if (currentInteractable.IsUnityNull()) return;
        currentInteractable.Interact();
    }

    private void OnInputActionPerformedInputInspecting(InputAction.CallbackContext context)
    {
        if (currentInspectable.IsUnityNull()) return;

        if (currentInspectables.Length > 1)
        {
            Debug.Log(currentInspectables.Length);
            foreach (IInspectable inspectable in currentInspectables)
                if (!inspectable.IsUnityNull())
                    inspectable.Inspect();
        }
        else
        {
            currentInspectable.Inspect();
        }
    }

    private void OnInputActionPerformedInputListening(InputAction.CallbackContext context)
    {
        if (currentConversable.IsUnityNull()) return;

        if (!currentConversable.StartChitChat())
            currentConversable.StartResponseIsCorrectChitChat();

        // TODO Delegate a function that shows all the correct UI Elements

        ActorControlTypeStateMachine.PushStateToListening(currentConversable);
    }

    private void OnInputActionPerformedInputTalking(InputAction.CallbackContext context)
    {
        if (currentConversable.IsUnityNull()) return;

        if (!currentConversable.StartTalkPrompt()) return;


        ActorControlTypeStateMachine.PushStateToTalking(currentConversable);
    }
}
