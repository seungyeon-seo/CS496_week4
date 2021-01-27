using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]

public class MyCharacterControls : MonoBehaviour
{
	// externel components
	public GameObject cam;
	private Animator anim;
	private CapsuleCollider coll;
	private Rigidbody rb;

	// parameters
	public float gravity = 10.0f;
	public float moveSpeed = 10.0f;
	public float rotateSpeed = 25f; //Speed the player rotate
	public float fallSpeed = 0.0f;
	public float airSpeed = 8f;
	public float maxFallSpeed = 100000.0f;
	public float jumpHeight = 10.0f;
	public float maxVelocityChange = 10.0f;
	public int flipCount = 1;

	// variables related to controlling movement
	public Vector3 moveVelocity;
	public Vector3 jumpVelocity;
	public Vector3 fallVelocity;

	private Vector3 moveDir = Vector3.zero;
	private bool isMove;
	private bool isJump;
	private bool isWalk;
	private bool isFlip;
	private bool isGrounded;

	// Externel Force
	[SerializeField]
	private float distToGround;
	private bool canMove = true; //If player is not hitted
	private bool isStuned = false;
	private bool wasStuned = false; //If player was stunned before get stunned another time
	private float pushForce;
	private Vector3 pushDir;
	private bool slide = false;

	// Checkpoint
	public Vector3 checkPoint;

	void Start()
	{
		coll = gameObject.GetComponent<CapsuleCollider>();
		anim = gameObject.GetComponentInChildren<Animator>();
		cam = GameObject.Find("Main Camera");
	}

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
		rb.freezeRotation = true;
		rb.useGravity = true;
		checkPoint = transform.position;
		Cursor.visible = true;
	}

	private void FixedUpdate()
	{
		Debug.Log("GROUND : " + isGrounded.ToString() + distToGround.ToString());
		// GRAVITY
		rb.AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0));

		// CANNOT MOVE
		if (!canMove)
		{
			//Debug.Log("CANNOT MOVE");
			rb.velocity = pushDir * pushForce;
		}
		// CAN MOVE
		else
		{
			//Debug.Log("CAN MOVE");

			// Rotation
			if (isMove)
			{
				Vector3 targetDir = moveDir; //Direction of the character

				targetDir.y = 0;
				if (targetDir == Vector3.zero)
				{
					targetDir = transform.forward;
				}
				Quaternion tr = Quaternion.LookRotation(targetDir); //Rotation of the character to where it moves
				Quaternion targetRotation = Quaternion.Slerp(transform.rotation, tr, Time.deltaTime * rotateSpeed); //Rotate the character little by little
				transform.rotation = targetRotation;
			}

			// Ground - RUN
			if (isGrounded)
			{
				//Debug.Log("RUN");
				// Calculate how fast we should be moving
				Vector3 targetVelocity = moveDir;
				targetVelocity *= moveSpeed;
				if (isWalk)
				{
					targetVelocity *= 0.3f;
				}

				// Apply a force that attempts to reach our target velocity
				Vector3 velocity = rb.velocity;
				if (targetVelocity.magnitude < velocity.magnitude) //If I'm slowing down the character
				{
					targetVelocity = velocity;
					rb.velocity /= 1.1f;
				}
				Vector3 velocityChange = (targetVelocity - velocity);
				velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
				velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
				velocityChange.y = 0;

				// Floor Slippery
				if (!slide)
				{
					if (Mathf.Abs(rb.velocity.magnitude) < moveSpeed * 1.0f)
						rb.AddForce(velocityChange, ForceMode.VelocityChange);
				}
				else if (Mathf.Abs(rb.velocity.magnitude) < moveSpeed * 1.0f)
				{
					rb.AddForce(moveDir * 0.15f, ForceMode.VelocityChange);
					//Debug.Log(rb.velocity.magnitude);
				}

				// Jump
				if (isJump)
				{
					//Debug.Log("JUMP");
					rb.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
				}

				// Reset Flip Count
				flipCount = 1;
			}
			else
			{
				//Debug.Log("NOT RUN");
				if (isFlip && flipCount > 0)
				{
					flipCount -= 1;
					rb.velocity = new Vector3(rb.velocity.x, CalculateJumpVerticalSpeed(), rb.velocity.z);
				}
				else if (!slide)
				{
					Vector3 targetVelocity = new Vector3(moveDir.x * airSpeed, rb.velocity.y, moveDir.z * airSpeed);
					Vector3 velocity = rb.velocity;
					Vector3 velocityChange = (targetVelocity - velocity);
					velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
					velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
					rb.AddForce(velocityChange, ForceMode.VelocityChange);
					if (velocity.y < -maxFallSpeed)
						rb.velocity = new Vector3(velocity.x, -maxFallSpeed, velocity.z);
				}
				else if (Mathf.Abs(rb.velocity.magnitude) < moveSpeed * 1.0f)
				{
					rb.AddForce(moveDir * 0.15f, ForceMode.VelocityChange);
				}
			}
		}
	}


	void Update()
	{
		// moveDir & isJump
		float h = Input.GetAxis("Horizontal");
		float v = Input.GetAxis("Vertical");
		Vector3 v2 = v * cam.transform.forward; //Vertical axis to which I want to move with respect to the camera
		Vector3 h2 = h * cam.transform.right; //Horizontal axis to which I want to move with respect to the camera
		moveDir = (v2 + h2).normalized; //Global position to which I want to move in magnitude 1
		isMove = moveDir.x != 0 || moveDir.z != 0;
		isJump = Input.GetKey(KeyCode.Space);
		isWalk = Input.GetKey(KeyCode.LeftShift);// Input.GetButton("Jump");
		isFlip = Input.GetKey(KeyCode.LeftControl);// Input.GetButton("Jump");
		isGrounded = IsGrounded();

		// Hit Check
		RaycastHit hit;
		if (Physics.Raycast(coll.bounds.center, Vector3.down, out hit, coll.bounds.extents.y + 0.01f))
		{
			if (hit.transform.tag == "Slide")
			{
				slide = true;
			}
			else
			{
				slide = false;
			}
		}

		//Animation Effect
		if (isGrounded)
		{
			// Idle : 0
			// Run : 1
			// Jump : 2
			// Fall : 3
			// Flip : 4
			// Walk : 5
			if (isWalk)
			{
				anim.SetInteger("AnimationPar", 5);
			}
			else if (isFlip && flipCount > 0)
			{
				anim.SetInteger("AnimationPar", 4);
			}
			else if (isJump)
			{
				anim.SetInteger("AnimationPar", 2);
			}
			else if (isMove)
			{
				anim.SetInteger("AnimationPar", 1);
			}
			else
			{
				anim.SetInteger("AnimationPar", 0);
			}
		}
		else
		{
			if (isFlip)
			{
				anim.SetInteger("AnimationPar", 4);
			}
			else
			{
				anim.SetInteger("AnimationPar", 3);
			}
		}
	}

	bool IsGrounded()
	{
		RaycastHit hit;
		Physics.Raycast(coll.bounds.center, Vector3.down, out hit, coll.bounds.extents.y + .01f);
		if (hit.collider != null)
		{
			return true;
		}
		else
		{
			return false;
		}
	}

	float CalculateJumpVerticalSpeed()
	{
		return Mathf.Sqrt(2 * jumpHeight * gravity);
	}

	public void HitPlayer(Vector3 velocityF, float time)
	{
		Debug.Log("HIT_PLAYER");
		rb.velocity = velocityF;

		pushForce = velocityF.magnitude;
		//pushForce = velocityF.magnitude * 500;
		pushDir = Vector3.Normalize(velocityF);
		StartCoroutine(Decrease(velocityF.magnitude, time));
	}

	public void LoadCheckPoint()
	{
		transform.position = checkPoint;
	}

	private IEnumerator Decrease(float value, float duration)
	{
		if (isStuned)
			wasStuned = true;
		isStuned = true;
		canMove = false;

		float delta = 0;
		delta = value / duration;

		for (float t = 0; t < duration; t += Time.deltaTime)
		{
			yield return null;
			if (!slide) //Reduce the force if the ground isnt slide
			{
				pushForce = pushForce - Time.deltaTime * delta;
				pushForce = pushForce < 0 ? 0 : pushForce;
				//Debug.Log(pushForce);
			}
			rb.AddForce(new Vector3(0, -gravity * GetComponent<Rigidbody>().mass, 0)); //Add gravity
		}

		if (wasStuned)
		{
			wasStuned = false;
		}
		else
		{
			isStuned = false;
			canMove = true;
		}
	}
}
