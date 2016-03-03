using UnityEngine;
using System.Collections;

public class NewPlayerController : MonoBehaviour {

	/*----------------------------------------------------------------------------------------------------------------------
	 * 														VARIABLES
	----------------------------------------------------------------------------------------------------------------------*/

	// State Machine variables-------------
	enum STATES{
		IDLE,
		WALKING,
		ATTACK,
		ON_AIR,
		DAMAGE
	};

	STATES currentState = STATES.IDLE;
	STATES nextState = STATES.IDLE;
	// State Machine variables-------------


	// Grounded variables------------------
	bool grounded = false; 			// Main variable
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask groundLayer;
	// Grounded variables------------------


	public float jumpForce = 1000f;


	// Animation Variables
	Animator animator;


	// InputAxis variables-----------------
	private float inputAxis;
	public float maxSpeed = 10f;
	// InputAxis variables-----------------


	// Damage variables--------------------
	float damageRecoil = 1000f;
	// Damage variables--------------------


	// Other Variables
	bool facingRight= true;
	public GameObject attackCollider;


	/*----------------------------------------------------------------------------------------------------------------------
	 * 													UPDATE FUNCTIONS
	----------------------------------------------------------------------------------------------------------------------*/

	void Start () {
		this.animator = this.GetComponent<Animator> ();
	}

	void FixedUpdate () {

		this.UpdateInputAxis ();
		this.UpdateGrounded ();

		switch (currentState) {
		case STATES.IDLE:
			
			this.animator.SetBool ("idle", true);

			if (this.IsAttacking ()) { 				// Is it attacking?
				this.nextState = STATES.ATTACK;
				this.attackCollider.SetActive (true);
			} else if (this.IsJumping ()) {			// Is it trying to jump?
				this.Jump ();
				this.nextState = STATES.ON_AIR;		
			} else if (!this.grounded) {			// Is it falling?
				this.nextState = STATES.ON_AIR;	
			} else if (this.inputAxis != 0) {		// Is it trying to move?
				this.nextState = STATES.WALKING;
			} else {
				this.nextState = STATES.IDLE;
			}

			//Debug.Log ("IDLE");

			break;

		case STATES.WALKING:
			
			this.animator.SetBool ("idle", false);

			if (this.IsAttacking ()) {				// Is it attacking?
				this.nextState = STATES.ATTACK;
			} else if (this.IsJumping ()) {			// Is it trying to jump?
				this.Jump ();
				this.nextState = STATES.ON_AIR;
			} else if (!this.grounded) {			// Is it falling?
				this.nextState = STATES.ON_AIR;
			} else if (this.inputAxis == 0) {		// Did it stop moving?
				this.nextState = STATES.IDLE;
			} else {
				this.nextState = STATES.WALKING;
			}

			//Debug.Log ("WALKING");

			break;
		
		case STATES.ATTACK:

			if (!this.animator.GetCurrentAnimatorStateInfo (0).IsName ("player-meelee")) {
				this.animator.SetTrigger ("attack");
			}


			Debug.Log (this.attackCollider.activeSelf);
			//this.ApplyDamage (0.3f, 0.7f);

			if (this.animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 0.9) {
				this.nextState = STATES.IDLE;
				this.attackCollider.SetActive(false);
			}

			//Debug.Log ("ATTACK");

			break;

		case STATES.ON_AIR:

			animator.SetFloat ("vSpeed", this.GetComponent<Rigidbody2D>().velocity.y);

			if (this.grounded) {
				//this.animator.SetBool ("grounded", true);
				nextState = STATES.IDLE;
			}

			//Debug.Log ("ON AIR");

			break;

		case STATES.DAMAGE:

			this.nextState = STATES.IDLE;
			animator.Play ("player-damage");
			Debug.Log ("Lost Life");
			break;

		default:
			break;
		}

		this.GetComponent<Rigidbody2D>().velocity = new Vector2 (inputAxis, this.GetComponent<Rigidbody2D>().velocity.y);
		this.FixScaleX ();

		currentState = nextState;
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag ("Enemy")) {
			Debug.Log ("Enemy Hit");
			this.attackCollider.SetActive(false);
		}
	}


	/*----------------------------------------------------------------------------------------------------------------------
	 * 													OTHER FUNCTIONS
	----------------------------------------------------------------------------------------------------------------------*/

	public void GetDamage (int dir) {
		this.currentState = STATES.DAMAGE;
		this.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (dir * this.damageRecoil, 100));
	}

	void UpdateInputAxis () {
		this.inputAxis = Input.GetAxisRaw ("Horizontal") * this.maxSpeed;
	}

	bool IsAttacking () {
		return Input.GetKeyDown (KeyCode.Z);
	}

	bool IsJumping () {
		return Input.GetKeyDown (KeyCode.X);
	}

	void Jump () {
		this.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpForce));
	}

	void UpdateGrounded () {
		this.grounded = Physics2D.OverlapCircle (this.groundCheck.position, groundRadius, groundLayer);
		animator.SetBool ("grounded", this.grounded);
	}

	void FixScaleX(){

		if (this.transform.localScale.x > 0) {
			this.facingRight = true;
		} else {
			this.facingRight = false;
		}

		if (Input.GetAxisRaw ("Horizontal") == 1 && facingRight == false) { // if Player facing left & moving right, flip
			this.transform.localScale = new Vector2 (-this.transform.localScale.x, this.transform.localScale.y);
		} else if (Input.GetAxisRaw ("Horizontal") == -1 && facingRight == true) { // if Player facing right & moving left, flip
			this.transform.localScale = new Vector2 (-this.transform.localScale.x, this.transform.localScale.y);
		}
	}

	// Applies damage between given start and end time
	// start and end are the percentage of the attack animaton's length
	// i.e.: start = 0.5 & end = 0.6 => applies damage between 50% and 60% of the animation's length.
	void ApplyDamage(float start, float end) {
		if (this.animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= start && this.animator.GetCurrentAnimatorStateInfo (0).normalizedTime <= end) {
			this.attackCollider.SetActive (true);
		} else {
			this.attackCollider.SetActive(false);
		}
	}
}
