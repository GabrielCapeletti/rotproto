using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	/*----------------------------------------------------------------------------------------------------------------------
	 * 														VARIABLES
	----------------------------------------------------------------------------------------------------------------------*/

	// State Machine
	enum STATES{
		IDLE,
		WALKING,
		ATTACK,
		ON_AIR
	};
	STATES currentState = STATES.IDLE;

	bool grounded = false;
	public Transform groundCheck;
	float groundRadius = 0.2f;
	public LayerMask groundLayer;
	public float jumpForce = 10;


	// Animation Variables
	Animator animator;


	// Speed Variables
	private float speed;
	public float maxSpeed = 10f;


	// Other Variables
	bool facingRight= true;
	public GameObject attackCollider;



	/*----------------------------------------------------------------------------------------------------------------------
	 * 													UPDATE FUNCTIONS
	----------------------------------------------------------------------------------------------------------------------*/

	void Start () {
		this.animator = this.GetComponent<Animator> ();
	}

	void Update () {
		this.grounded = Physics2D.OverlapCircle (this.groundCheck.position, groundRadius, groundLayer);
		animator.SetBool ("grounded", this.grounded);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		this.speed = Input.GetAxisRaw ("Horizontal") * this.maxSpeed;

		animator.SetFloat ("vSpeed", this.GetComponent<Rigidbody2D>().velocity.y);

		switch (currentState) {
		case STATES.IDLE:
			
			this.animator.SetBool ("idle", true);

			if (Input.GetKeyDown (KeyCode.Z)) {
				this.currentState = STATES.ATTACK;
			} else if (Input.GetAxisRaw ("Horizontal") != 0) {
				this.currentState = STATES.WALKING;
			}
			if (!grounded) {
				//this.animator.SetBool ("grounded", false);
				this.currentState = STATES.ON_AIR;
			}
			if (Input.GetKeyDown (KeyCode.X)) {
				this.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpForce));
				//this.animator.SetBool ("grounded", false);
				this.currentState = STATES.ON_AIR;
			}

			break;


		case STATES.WALKING:

			this.animator.SetBool ("idle", false);

			if (Input.GetKeyDown (KeyCode.Z)) {
				this.currentState = STATES.ATTACK;
			} else if (Input.GetAxisRaw ("Horizontal") == 0) {
				this.currentState = STATES.IDLE;
			}
			if (!grounded) {
				//this.animator.SetBool ("grounded", false);
				this.currentState = STATES.ON_AIR;
			}
			if (Input.GetKeyDown (KeyCode.X)) {
				this.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (0, jumpForce));
				//this.animator.SetBool ("grounded", false);
				this.currentState = STATES.ON_AIR;
			}
				
			break;


		case STATES.ATTACK:

			if (!this.animator.GetCurrentAnimatorStateInfo (0).IsName ("player-meelee")) {
				this.animator.SetTrigger ("attack");
			}
			this.ApplyDamage (0.6f, 0.65f);
			if (this.animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 0.9) {
				this.currentState = STATES.IDLE;
			}
			//animator.Play ("player-meelee");
			break;

		case STATES.ON_AIR:
			if (grounded) {
				//this.animator.SetBool ("grounded", true);
				currentState = STATES.IDLE;
			}
			break;

		default:
			break;
		}

		this.GetComponent<Rigidbody2D>().velocity = new Vector2 (speed, this.GetComponent<Rigidbody2D>().velocity.y);
		this.FixScaleX ();

	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag ("Enemy")) {
			Debug.Log ("Enemy Hit");
		}
	}


	/*----------------------------------------------------------------------------------------------------------------------
	 * 													OTHER FUNCTIONS
	----------------------------------------------------------------------------------------------------------------------*/


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
