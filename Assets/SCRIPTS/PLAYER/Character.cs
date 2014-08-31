using UnityEngine;
 
public class Character : MonoBehaviour
{
 
    // Require a character controller to be attached to the same game object
    [System.Serializable]
    public class PlatformerControllerMovement
    {
        // The speed when running
        public float runSpeed = 10.0f;
 
        public float inAirControlAcceleration = 1.0f;
 
        // The gravity for the character
        public float gravity = 60.0f;
        public float maxFallSpeed = 20.0f;
 
        // How fast does the character change speeds?  Higher is faster.
        public float speedSmoothing = 20.0f;
 
        // The current move direction in x-y.  This will always been (1,0,0) or (-1,0,0)
        // The next line, @System.NonSerialized , tells Unity to not serialize the variable or show it in the inspector view.  Very handy for organization!
        [System.NonSerialized]
        public Vector3 direction = Vector3.zero;
 
        // The current vertical speed
        [System.NonSerialized]
        public float verticalSpeed = 0.0f;
 
        // The current movement speed.  This gets smoothed by speedSmoothing.
        [System.NonSerialized]
        public float speed = 0.0f;
 
        // Is the user pressing the left or right movement keys?
        [System.NonSerialized]
        public bool isMoving = false;
 
        // The last collision flags returned from controller.Move
        [System.NonSerialized]
        public CollisionFlags collisionFlags;
 
        // We will keep track of an approximation of the character's current velocity, so that we return it from GetVelocity () for our camera to use for prediction.
        [System.NonSerialized]
        public Vector3 velocity;
 
        // This keeps track of our current velocity while we're not grounded?
        [System.NonSerialized]
        public Vector3 inAirVelocity = Vector3.zero;
 
    }
 
    [System.Serializable]
    // We will contain all the jumping related variables in one helper class for clarity.
    public class PlatformerControllerJumping
    {
        // Can the character jump?
        public bool enabled = true;
 
        // How high do we jump when pressing jump and letting go immediately
        public float height = 1.0f;
        // We add extraHeight units (meters) on top when holding the button down longer while jumping
        public float extraHeightTime = 0.5f;

        public float currentExtraHeightTime = 0.0f;
 
        // This prevents inordinarily too quick jumping
        // The next line, @System.NonSerialized , tells Unity to not serialize the variable or show it in the inspector view.  Very handy for organization!
        [System.NonSerialized]
        public float repeatTime = 0.05f;
 
        [System.NonSerialized]
        public float timeout = 0.15f;
 
        // Are we jumping? (Initiated with jump button and not grounded yet)
        [System.NonSerialized]
        public bool jumping = false;
 
        [System.NonSerialized]
        public bool reachedApex = false;
 
        // Last time the jump button was clicked down
        [System.NonSerialized]
        public float lastButtonTime = -10.0f;
 
        // Last time we performed a jump
        [System.NonSerialized]
        public float lastTime = -1.0f;
 
        // the height we jumped from (Used to determine for how long to apply extra jump power after jumping.)
        [System.NonSerialized]
        public float lastStartHeight = 0.0f;
    }

    [System.Serializable]
    public class PlayerThrowback
    {
        public float throwBackStrength = 10.0f;
        public float feedbackStrength = 2.0f;

        public float throwbackDeaccelerationValue = 0.95f;

        public Vector3 currentThrowback = new Vector3(0.0f, 0.0f, 0.0f);
    }

    // Does this script currently respond to Input?
    public bool canControl = true;
 
    public PlatformerControllerMovement movement;
 
 	protected PlayerController playerController; // input controller
 
    public PlatformerControllerJumping jump;

    public PlayerThrowback throwback;
 
    protected CharacterController controller;
 
    // Moving platform support.
    protected Transform activePlatform;
    protected Vector3 activeLocalPlatformPoint;
    protected Vector3 activeGlobalPlatformPoint;
    protected Vector3 lastPlatformVelocity;

    protected EPlayerID playerID;

    protected virtual void CharacterAwake()
    {
        movement = new PlatformerControllerMovement();
        jump = new PlatformerControllerJumping();
        movement.direction = transform.TransformDirection(Vector3.forward);
        controller = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
    }
 
    protected virtual void Awake()
    {
    	CharacterAwake();
    }

    public void SetPlayerID(EPlayerID id)
    {
        playerID = id;
    }

    protected GameObject GetEnemy()
    {
        if(playerID == EPlayerID.PlayerOne)
        {
            return GameObject.FindWithTag("PLAYER_TWO");
        }
        else
        {
            return GameObject.FindWithTag("PLAYER_ONE");
        }
    }
 
    public void Spawn(Vector3 position)
    {
        // reset the character's speed
        movement.verticalSpeed = 0.0f;
        movement.speed = 0.0f;
 
        // reset the character's position to the spawnPoint
        transform.position = position;
    }
 
    protected virtual void UpdateSmoothedMovementDirection()
    {
        float h = 0.0f;
        if(playerController.goingLeft) 	h -= 1.0f;
        if(playerController.goingRight) h += 1.0f;
 
        if (!canControl)
            h = 0.0f;
 
        movement.isMoving = Mathf.Abs(h) > 0.1;
 
        if (movement.isMoving)
            movement.direction = new Vector3(h, 0, 0);
 
        // Smooth the speed based on the current target direction
        float curSmooth = movement.speedSmoothing * Time.deltaTime;

        // Choose target speed

        float targetSpeed = Mathf.Min(Mathf.Abs(h), 1.0f);

        targetSpeed *= movement.runSpeed;

        movement.speed = Mathf.Lerp(movement.speed, targetSpeed, curSmooth);

        // HACK: no smoothing
        movement.speed = targetSpeed;
    }

    protected virtual void CharacterFixedUpdate()
    {
        // Make sure we are absolutely always in the 2D plane.
        transform.position = new Vector3(transform.position.x, transform.position.y, 0.0f);
    }
 
    void FixedUpdate()
    {
    	CharacterFixedUpdate();
    }
 
    protected virtual void ApplyJumping()
    {
        // Prevent jumping too fast after each other
        if (jump.lastTime + jump.repeatTime > Time.time)
            return;
 
        if (controller.isGrounded)
        {
            // Jump
            // - Only when pressing the button down
            // - With a timeout so you can press the button slightly before landing
            if (jump.enabled && Time.time < jump.lastButtonTime + jump.timeout)
            {
                movement.verticalSpeed = CalculateJumpVerticalSpeed(jump.height);
                movement.inAirVelocity = lastPlatformVelocity;
                SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);
            }
        }
    }
 
   protected virtual void ApplyGravity()
    {
        // Apply gravity
        bool jumpButton = playerController.jumping;

        if (!canControl)
            jumpButton = false;
 
        // When we reach the apex of the jump we send out a message
        if (jump.jumping && !jump.reachedApex && movement.verticalSpeed <= 0.0)
        {
            jump.reachedApex = true;
            SendMessage("DidJumpReachApex", SendMessageOptions.DontRequireReceiver);
        }
        // * When jumping up we don't apply gravity for some time when the user is holding the jump button
        //   This gives more control over jump height by pressing the button longer
        bool extraPowerJump = jump.jumping && jumpButton && jump.currentExtraHeightTime < jump.extraHeightTime && !IsTouchingCeiling();
 
        if (extraPowerJump)
            return;
        else if (controller.isGrounded)
            movement.verticalSpeed = -movement.gravity * Time.deltaTime;
        else
            movement.verticalSpeed -= movement.gravity * Time.deltaTime;
 
        // Make sure we don't fall any faster than maxFallSpeed.  This gives our character a terminal velocity.
        movement.verticalSpeed = Mathf.Max(movement.verticalSpeed, -movement.maxFallSpeed);
    }
 
    protected virtual float CalculateJumpVerticalSpeed(float targetJumpHeight)
    {
        // From the jump height and gravity we deduce the upwards speed
        // for the character to reach at the apex.
        return Mathf.Sqrt(2 * targetJumpHeight * movement.gravity);
    }
 
    protected virtual void DidJump()
    {
        jump.jumping = true;
        jump.reachedApex = false;
        jump.lastTime = Time.time;
        jump.lastStartHeight = transform.position.y;
        jump.lastButtonTime = -10;
        jump.currentExtraHeightTime = 0.0f;
    }

    protected virtual void CharacterUpdate()
    {
    	if (playerController.jumping && canControl)
        {
            jump.lastButtonTime = Time.time;
            jump.lastStartHeight = transform.position.y;
        }

        if(jump.currentExtraHeightTime < jump.extraHeightTime)
        jump.currentExtraHeightTime += Time.deltaTime;
 
        UpdateSmoothedMovementDirection();
 
        // Apply gravity
        // - extra power jump modifies gravity
        ApplyGravity();
 
        // Apply jumping logic
        ApplyJumping();
 
        // Moving platform support
        if (activePlatform != null)
        {
            Vector3 newGlobalPlatformPoint = activePlatform.TransformPoint(activeLocalPlatformPoint);
            Vector3 moveDistance = (newGlobalPlatformPoint - activeGlobalPlatformPoint);
            transform.position = transform.position + moveDistance;
            lastPlatformVelocity = (newGlobalPlatformPoint - activeGlobalPlatformPoint) / Time.deltaTime;
        }
        else
        {
            lastPlatformVelocity = Vector3.zero;
        }
 
        activePlatform = null;
 
        // Save lastPosition for velocity calculation.
        Vector3 lastPosition = transform.position;
 
        // Calculate actual motion
        Vector3 currentMovementOffset = movement.direction * movement.speed + new Vector3(0, movement.verticalSpeed, 0) + movement.inAirVelocity;
        currentMovementOffset += throwback.currentThrowback;
 
        // We always want the movement to be framerate independent.  Multiplying by Time.deltaTime does this.
        currentMovementOffset *= Time.deltaTime;
 
        // Move our character!
        movement.collisionFlags = controller.Move(currentMovementOffset);
 
        // Calculate the velocity based on the current and previous position.
        // This means our velocity will only be the amount the character actually moved as a result of collisions.
        movement.velocity = (transform.position - lastPosition) / Time.deltaTime;
 
        // Moving platforms support
        if (activePlatform != null)
        {
            activeGlobalPlatformPoint = transform.position;
            activeLocalPlatformPoint = activePlatform.InverseTransformPoint(transform.position);
        }
 
        // Set rotation to the move direction  
        /*if (movement.direction.sqrMagnitude > 0.01)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.direction), Time.deltaTime * movement.rotationSmoothing);
        else transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement.direction), Time.deltaTime * 100);*/

        // TODO: flip character based on direction
 
        // We are in jump mode but just became grounded
        if (controller.isGrounded)
        {
            movement.inAirVelocity = Vector3.zero;
 
            if (jump.jumping)
            {
                jump.jumping = false;
 
                SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);
 
                Vector3 jumpMoveDirection = movement.direction * movement.speed + movement.inAirVelocity;
                if (jumpMoveDirection.sqrMagnitude > 0.01)
                    movement.direction = jumpMoveDirection.normalized;
            }
        }

        throwback.currentThrowback *= throwback.throwbackDeaccelerationValue;
    }
 
    protected virtual void Update()
    {
        CharacterUpdate();
    }
 
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.moveDirection.y > 0.01f)
            return;
 
        // Make sure we are really standing on a straight platform
        // Not on the underside of one and not falling down from it either!
        if (hit.moveDirection.y < -0.9f && hit.normal.y > 0.9f)
        {
            activePlatform = hit.collider.transform;
        }
    }
 
    // Various helper functions below:
    protected virtual float GetSpeed()
    {
        return movement.speed;
    }
 
    protected virtual Vector3 GetVelocity()
    {
        return movement.velocity;
    }
 
 
    protected virtual bool IsMoving()
    {
        return movement.isMoving;
    }
 
    protected virtual bool IsJumping()
    {
        return jump.jumping;
    }
 
    protected virtual bool IsTouchingCeiling()
    {
        return (movement.collisionFlags & CollisionFlags.CollidedAbove) != 0;
    }
 
    protected virtual Vector3 GetDirection()
    {
        return movement.direction;
    }
 
    protected virtual void SetControllable(bool controllable)
    {
        canControl = controllable;
    }

    public virtual void onAttacked(Vector3 dir)
    {
        Debug.Log("Kill!");
    }

    public virtual void onFeedback(Vector3 dir)
    {
        Debug.Log("Shouldn't happen!");
    }
}
 