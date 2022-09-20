using System;
using System.Collections;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Player")] 
    [SerializeField] private bool isP1;
	
	[Header("Movement")]
    [SerializeField, Range(0f, 100f)]
 	float maxSpeed = 10f;
    [SerializeField, Range(0f, 100f)]
 	float maxAcceleration = 10f;
    private Vector2 aim;

    private Vector2 playerMoveInput;
    Rigidbody rb;
    public Vector3 velocity, desiredVelocity;
    Vector3 contactNormal;
	public int teamID;
	
	//gamepad
	[SerializeField] private float controllerDeadzone = 0.1f;
	[SerializeField] private float gamepadRotateSmoothing = 500f;
	[SerializeField] private bool isGamepad;
	private PlayerInput playerInput;
	

    [Header("Dash")]
    public float moveSpeed = 5f;
    public float moveAcceleration = 70f;
    public float dashSpeed = 10f; 
    public int dashCount;
    public float dashAcceleration = 100f;
    public float dashLength = .5f;
    public float dashCooldown = 1f;
    public float dashCounter, dashCoolCounter;
    public List<Transform> dashInHand;

	[Header("Asset")]
	private AudioSource audioSource;
	public AudioClip dashSFX;
	private Animator animator;
	[SerializeField] private MMFeedbacks hitFeedack;
	[SerializeField] private TextMeshProUGUI hitCountText;
	private GameManager gameManager;
	
	[Header("GameSession")]
	public int hitCount;

	void Awake () {
		playerInput = GetComponent<PlayerInput>();
		rb = GetComponent<Rigidbody>();
		audioSource = GetComponent<AudioSource>();
		animator = GetComponent<Animator>();
		gameManager = FindObjectOfType<GameManager>();

	}
	void Update ()
    {
	    HandleDash();
	    HandleRotation();
	    isGamepad = playerInput.currentControlScheme.Equals("Gamepad") ? true : false;
	    
	    SetTeamID();
	    ChangeHitText();
    }
	void FixedUpdate () 
    {
	    AdjustVelocity();
	    rb.velocity = velocity;
	    var mass = rb.mass;
	    rb.AddForce(Physics.gravity, ForceMode.Acceleration);;
	    ClearState();
    }
    public void OnMove(InputAction.CallbackContext ctx)
    {
	    playerMoveInput = ctx.ReadValue<Vector2>();
	    playerMoveInput = Vector2.ClampMagnitude(playerMoveInput, 1f);
	    desiredVelocity = new Vector3(playerMoveInput.x, 0f, playerMoveInput.y) * maxSpeed;
    }
    public void OnAttack(InputAction.CallbackContext ctx)
    {
	    if (ctx.performed)
	    {
		    //animator.SetTrigger("ATTACK");
	    }
    }

    public void OnDash(InputAction.CallbackContext ctx)
    {
	    if (dashCount > 0f && dashCoolCounter <= 0f && dashCounter <= 0f)
	    {
		    maxSpeed = dashSpeed;
		    maxAcceleration = dashAcceleration;
		    dashCounter = dashLength;

		    dashCount--;

		    Destroy(dashInHand[dashInHand.Count - 1].gameObject);
		    dashInHand.Remove(dashInHand[dashInHand.Count - 1]);
		    audioSource.clip = dashSFX;
		    audioSource.Play(0);
	    }
    }

    public void OnAim(InputAction.CallbackContext ctx)
    {
	    aim = ctx.ReadValue<Vector2>();
    }
	void ClearState () {
	    contactNormal = Vector3.zero;
 	}
    void AdjustVelocity () {
 		Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
 		Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;
 
 		float currentX = Vector3.Dot(velocity, xAxis);
 		float currentZ = Vector3.Dot(velocity, zAxis);
 
 		float acceleration = maxAcceleration;
 		float maxSpeedChange = acceleration * Time.deltaTime;
 
 		float newX =
 			Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
 		float newZ =
 			Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);
 
 		velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
 	}
    Vector3 ProjectOnContactPlane (Vector3 vector) {
 		return vector - contactNormal * Vector3.Dot(vector, contactNormal);
 	}

    void HandleDash()
    {
	    if (isP1)
	    {
		    if (Input.GetKeyDown(KeyCode.Space) && dashCount > 0f )
		    {
			    if (dashCoolCounter <= 0f && dashCounter <= 0f)	
			    {
				    maxSpeed = dashSpeed;
				    maxAcceleration = dashAcceleration;
				    dashCounter = dashLength;
			    
				    dashCount--;
			    
				    Destroy(dashInHand[dashInHand.Count-1].gameObject);
				    dashInHand.Remove(dashInHand[dashInHand.Count - 1]);
					audioSource.clip = dashSFX;
					audioSource.Play(0);
				}
		    }
	    }
	    else
	    {
		    if (Input.GetKeyDown(KeyCode.RightControl) && dashCount > 0f )
		    {
			    if (dashCoolCounter <= 0f && dashCounter <= 0f)	
			    {
				    maxSpeed = dashSpeed;
				    maxAcceleration = dashAcceleration;
				    dashCounter = dashLength;
			    
				    dashCount--;
			    
				    Destroy(dashInHand[dashInHand.Count-1].gameObject);
				    dashInHand.Remove(dashInHand[dashInHand.Count - 1]);

					audioSource.clip = dashSFX;
					audioSource.Play(0);
				}
		    }
	    }

	   
	    
	    if (dashCounter > 0f)
	    {
		    dashCounter -= Time.deltaTime;
		    if (dashCounter <= 0f)
		    {
			    maxSpeed = moveSpeed;
			    maxAcceleration = moveAcceleration;
			    dashCoolCounter = dashCooldown;
		    }
	    }
	    if (dashCoolCounter > 0f)
	    {
		    dashCoolCounter -= Time.deltaTime;
	    }
    }
    
    private void HandleRotation()
    {
	  
	    if (isGamepad)
	    {
		    if (Mathf.Abs(aim.x) > controllerDeadzone || Mathf.Abs(aim.y) > controllerDeadzone)
		    {
			    Vector3 playerDirection = Vector3.right * aim.x + Vector3.forward * aim.y;

			    if (playerDirection.sqrMagnitude > 0.0f)
			    {
				    Quaternion newrotation = Quaternion.LookRotation(playerDirection, Vector3.up);
				    transform.rotation = Quaternion.RotateTowards(transform.rotation, newrotation, gamepadRotateSmoothing * Time.deltaTime);
			    }
		    }
		    
	    }
	    else
	    {
		    if (Camera.main is not null)
		    {
			    Ray ray = Camera.main.ScreenPointToRay(aim);
			    Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
			    float rayDistance;

			    if (groundPlane.Raycast(ray, out rayDistance))
			    {
				    Vector3 point = ray.GetPoint(rayDistance);
				    LookAt(point);
			    }
		    }

	    }
    }
    private void LookAt(Vector3 lookPoint)
    {
	    Vector3 heightCorrectedPoint = new Vector3(lookPoint.x, transform.position.y, lookPoint.z);
	    transform.LookAt(heightCorrectedPoint);
    }

    private void OnCollisionEnter(Collision other)
    {
	    if (other.gameObject.tag == "Ball")
	    {
		    hitCount--;
		    audioSource.Play();
		    hitFeedack.PlayFeedbacks();
	    }
    }

    private void ChangeHitText()
    {
	    if (!gameManager.gameOver)
	    {
		    hitCountText.text = hitCount.ToString();
	    }

    }

    private void SetTeamID()
    {
	    if (gameObject.name == "Player1")
	    {
		    isP1 = true;
		    teamID = 1;
	    }
	    else if(gameObject.name == "Player2")

	    {
		    isP1 = false;
		    teamID = 2;
	    }

    }
    
}
