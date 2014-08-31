using UnityEngine;
 
public class Character : MonoBehaviour
{
    protected enum CharacterStates
    {
        Idle,
        Jump,
        Attack,
        Block,
        Run,
        Ground
    }
 
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
 
        public float doubleJumpHeight = 2.1f;
 
        // Are where double jumping? ( Initiated when jumping or falling after pressing the jump button )
        [System.NonSerialized]
        public bool doubleJumping = false;
 
        // Can we make a double jump ( we can't make two double jump or more at the same jump )
        [System.NonSerialized]
        public bool canDoubleJump = false;

        public bool doubleJumped = false;
 
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

    public AudioClip JumpSound;
 
    public PlatformerControllerMovement movement;
 
 	protected PlayerController playerController; // input controller
 
    public PlatformerControllerJumping jump;

    public PlayerThrowback throwback;

    public GameManager gameManager;

    CharacterStates characterState;

    protected AudioSource audioSource;
 
    protected CharacterController controller;
 
    // Moving platform support.
    protected Transform activePlatform;
    protected Vector3 activeLocalPlatformPoint;
    protected Vector3 activeGlobalPlatformPoint;
    protected Vector3 lastPlatformVelocity;

    public Game game;

    protected EPlayerID playerID;

    protected virtual void CharacterAwake()
    {
        movement = new PlatformerControllerMovement();
        jump = new PlatformerControllerJumping();
        movement.direction = transform.TransformDirection(Vector3.forward);
        controller = GetComponent<CharacterController>();
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
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
        if(!GameManager.IsPaused)
    	   CharacterFixedUpdate();
    }
 
    protected virtual void ApplyJumping()
    {
        if (controller.isGrounded || (jump.doubleJumping && !jump.doubleJumped))
        {
            // Jump
            // - Only when pressing the button down
            // - With a timeout so you can press the button slightly before landing
            if (jump.enabled && Time.time < jump.lastButtonTime + jump.timeout)
            {
                movement.verticalSpeed = CalculateJumpVerticalSpeed(jump.height);
                movement.inAirVelocity = lastPlatformVelocity;
                SendMessage("DidJump", SendMessageOptions.DontRequireReceiver);

				//this.GetComponent<PlayerAnimater>().SetPlayerJumping(true,false);
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

			this.GetComponent<PlayerAnimater>().SetPlayerFlying();
        }

        // * When jumping up we don't apply gravity for some time when the user is holding the jump button
        //   This gives more control over jump height by pressing the button longer
        bool extraPowerJump = jump.jumping && jump.doubleJumping && !jump.doubleJumped && movement.verticalSpeed > 0.0 && jumpButton && !IsTouchingCeiling();
 
        if (extraPowerJump)
        {
            return;
        }
        else if (controller.isGrounded)
        {
            movement.verticalSpeed = -movement.gravity * Time.deltaTime;
            jump.canDoubleJump = false;
        }
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
        SetState(CharacterStates.Jump);
    }

    protected virtual void CharacterUpdate()
    {
        bool jumpButton = playerController.jumping;
        // if we are jumping and we press jump button, we do a double jump or
        // if we are falling, we can do a double jump to
        if ((jump.jumping && jumpButton && !jump.doubleJumping) || (!controller.isGrounded && !jump.jumping && !jump.doubleJumping && movement.verticalSpeed < -12.0))
        {
            jump.canDoubleJump = true;
        }
 
        // if we can do a double jump, and we press the jump button, we do a double jump
        if (jump.canDoubleJump && jumpButton && !IsTouchingCeiling())
        {
            jump.doubleJumping = true;
            movement.verticalSpeed = CalculateJumpVerticalSpeed(jump.doubleJumpHeight);
            jump.canDoubleJump = false;
 
        }
    	if (jumpButton && canControl)
        {
            jump.lastButtonTime = Time.time;
            jump.lastStartHeight = transform.position.y;
        }
 
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
 
            if (jump.jumping || jump.doubleJumping)
            {
                jump.jumping = false;
                jump.doubleJumping = false;
                jump.canDoubleJump = false;
                jump.doubleJumped = false;
 
                SendMessage("DidLand", SendMessageOptions.DontRequireReceiver);

                SetState(CharacterStates.Ground);
 
                Vector3 jumpMoveDirection = movement.direction * movement.speed + movement.inAirVelocity;
                if (jumpMoveDirection.sqrMagnitude > 0.01)
                    movement.direction = jumpMoveDirection.normalized;
            }
        }

        throwback.currentThrowback *= throwback.throwbackDeaccelerationValue;

        if(jump.doubleJumping)
            jump.doubleJumped = true;
    }
 
    protected virtual void Update()
    {
        if(!GameManager.IsPaused)
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

			if (this.GetComponent<PlayerAnimater>().GetInAir()){

				this.GetComponent<PlayerAnimater>().SetPlayerStanding();
			}
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
        gameManager.OnEndGame(playerID == EPlayerID.PlayerOne ? 1 : 2);
		this.GetComponent<PlayerAnimater>().SetPlayerBlocking();
    }

    public virtual void onFeedback(Vector3 dir)
    {
        Debug.Log("Shouldn't happen!");
    }

    protected void OnSausageWarp()
    {
        GameObject level_holder = GameObject.FindGameObjectWithTag("LEVEL_HOLDER");
        level_holder.GetComponent<LevelHolder>().TriggerNinjaWarp();

        Vector3 position = transform.position;
        game.CreateCharacter(playerID, CharacterType.Sausage, position);
    }

    protected void OnNinjaWarp()
    {
        GameObject level_holder = GameObject.FindGameObjectWithTag("LEVEL_HOLDER");
        level_holder.GetComponent<LevelHolder>().TriggerSausageWarp();

        Vector3 position = transform.position;
        game.CreateCharacter(playerID, CharacterType.Ninja, position);
    }

    public virtual void OnTriggerEnter(Collider trigger_collider){

        if (trigger_collider.gameObject.tag == "SAUSAGE_WARP"){

            if (trigger_collider.gameObject.GetComponent<WarpTileObject>().GetWarpOn()){
                OnSausageWarp();
            }
        }
        else if (trigger_collider.gameObject.tag == "NINJA_WARP"){

            if (trigger_collider.gameObject.GetComponent<WarpTileObject>().GetWarpOn()){
                OnNinjaWarp();
            }
        }
    }

    protected virtual void OnCharacterIdle()
    {

    }

    protected virtual void OnCharacterAttack()
    {
        
    }

    protected virtual void OnCharacterJump()
    {
        audioSource.PlayOneShot(JumpSound);
        this.GetComponent<PlayerAnimater>().SetPlayerJumping();
    }

    protected virtual void OnCharacterBlock()
    {
        
    }

    protected virtual void OnCharacterRun()
    {
        
    }

    protected virtual void OnCharacterGround()
    {
        
    }

    protected void SetState(CharacterStates newState)
    {
        if(newState != characterState)
        {
            switch(newState)
            {
                case CharacterStates.Idle:
                    characterState = newState;
                    OnCharacterIdle();
                    break;

                case CharacterStates.Attack:
                    if(characterState != CharacterStates.Block)
                        characterState = newState;
                    OnCharacterAttack();
                    break;

                case CharacterStates.Jump:
                    characterState = newState;
                    OnCharacterJump();
                    break;

                case CharacterStates.Block:
                    characterState = newState;
                    OnCharacterBlock();
                    break;

                case CharacterStates.Run:
                    characterState = newState;
                    OnCharacterRun();
                    break;

                case CharacterStates.Ground:
                    characterState = newState;
                    OnCharacterGround();
                    break;
            }
        }
    }
}
 