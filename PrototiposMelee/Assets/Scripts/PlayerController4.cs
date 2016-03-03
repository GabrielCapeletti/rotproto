using UnityEngine;
using System.Collections;

public class PlayerController4 : MonoBehaviour {

	/*----------------------------------------------------------------------------------------------------------------------
	 * 														VARIABLES
	----------------------------------------------------------------------------------------------------------------------*/

	// State Machine variables-------------
	enum STATES{
		IDLE,
		WALKING,
		ATTACK,
		STRANGLE,
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


	// InputAxis variables-----------------
	private float inputAxis;
	public float maxSpeed = 10f;
	// InputAxis variables-----------------


	// Damage variables--------------------
	float damageRecoil = 1000f;
	// Damage variables--------------------


	// Attack variables-------------------
	// Combo Variables
	float comboMaxTime = 1f;
	float comboMinTime = 0.3f;
	float comboTime = 0f;
	int comboCount = 0;

	// Strangle Variables
	float strangleBeginTime = 0.3f;
	float strangleMaxTime = 1f;
	float strangleMinTime = 0.3f;
	float strangleTime = 0;
	// Attack variables-------------------


	// Animation Variables
	Animator animator;


	// Other Variables
	public float jumpForce = 1000f;
	bool facingRight= true;
	GameObject attackCollider;

	/*----------------------------------------------------------------------------------------------------------------------
	 * 													UPDATE FUNCTIONS
	----------------------------------------------------------------------------------------------------------------------*/

	void Start () {
		this.animator = this.GetComponent<Animator> ();
		this.attackCollider = this.transform.Find ("AttackCollider").gameObject;
	}

	void FixedUpdate () {

		this.UpdateInputAxis ();
		this.UpdateGrounded ();

		switch (currentState) {
		case STATES.IDLE:

			this.animator.Play ("player-idle");

			if (this.GetAttack ()) { 				// Is it attacking?
				this.nextState = STATES.ATTACK;
			} else if (this.GetJump ()) {			// Is it trying to jump?
				this.Jump ();
				this.nextState = STATES.ON_AIR;		
			} else if (!this.grounded) {			// Is it falling?
				this.nextState = STATES.ON_AIR;	
			} else if (this.inputAxis != 0) {		// Is it trying to move?
				this.nextState = STATES.WALKING;
			} else {
				this.nextState = STATES.IDLE;
			}

			break;

		case STATES.WALKING:

			this.animator.Play ("player-walking");

			this.GetComponent<Rigidbody2D>().velocity = new Vector2 (inputAxis, this.GetComponent<Rigidbody2D>().velocity.y);

			if (this.GetAttack ()) {				// Is it attacking?
				this.nextState = STATES.ATTACK;
			} else if (this.GetJump ()) {			// Is it trying to jump?
				this.Jump ();
				this.nextState = STATES.ON_AIR;
			} else if (!this.grounded) {			// Is it falling?
				this.nextState = STATES.ON_AIR;
			} else if (this.inputAxis == 0) {		// Did it stop moving?
				this.nextState = STATES.IDLE;
			} else {
				this.nextState = STATES.WALKING;
			}

			break;

		case STATES.ATTACK:

			this.GetComponent<Rigidbody2D>().velocity = new Vector2 (0, this.GetComponent<Rigidbody2D>().velocity.y);

			this.attackCollider.SetActive (true);

			if (this.comboCount == 0) {
				this.comboCount = 1;
				this.animator.Play("player-attack-1");
			}

			this.comboTime += Time.deltaTime;

			if (this.comboTime < this.comboMinTime) {
				
				attackCollider.GetComponent<Renderer>().material.color = Color.white;

			} else if (this.comboTime > this.comboMinTime && this.comboTime < this.comboMaxTime) {
				
				attackCollider.GetComponent<Renderer>().material.color = Color.blue;

				if (this.GetAttack ()) {
					if (Input.GetKey (KeyCode.X) && this.comboCount < 3) {

						this.attackCollider.SetActive (false);
						this.comboTime = 0f;
						this.comboCount = 0;

						this.animator.Play("scorpion-strangle-begin");

						this.nextState = STATES.STRANGLE;

					} else if (Input.GetKey (KeyCode.X) && this.comboCount >= 3 && this.comboCount < 5) {

						this.attackCollider.SetActive (false);
						this.comboCount = 5;
						this.comboTime = 0f;

						this.animator.Play("scorpion-knee-in-the-face");

					} else if (this.comboCount < 4){
						
						this.attackCollider.SetActive (false);
						this.comboCount++;
						this.comboTime = 0f;

						this.animator.Play("player-attack-" + this.comboCount);

					}
				}
					
			} else if (this.comboTime > this.comboMaxTime) {
				
				this.comboTime = 0f;
				this.nextState = STATES.IDLE;
				this.attackCollider.SetActive (false);
				this.comboCount = 0;

			} else {
				this.nextState = STATES.ATTACK;
			}
				
			break;

		case STATES.STRANGLE:

			this.strangleTime += Time.deltaTime;

			if (this.strangleTime < this.strangleMinTime) {

			} else if (this.strangleTime > this.strangleMinTime && this.strangleTime < this.strangleMaxTime) {

				if (this.GetAttack () && Input.GetKey (KeyCode.X)) {

					this.attackCollider.SetActive (false);
					this.strangleTime = 0f;

					this.animator.Play("scorpion-strangle-attack");
				}

			} else if (this.strangleTime > this.strangleMaxTime) {

				this.strangleTime = 0f;
				this.nextState = STATES.IDLE;
			
			}

			break;

		case STATES.ON_AIR:
			
			this.animator.Play("player-jump");

			this.GetComponent<Rigidbody2D>().velocity = new Vector2 (inputAxis, this.GetComponent<Rigidbody2D>().velocity.y);

			if (this.grounded) {
				nextState = STATES.IDLE;
			}

			break;

		case STATES.DAMAGE:

			this.nextState = STATES.IDLE;
			animator.Play ("player-damage");
			break;

		default:
			break;
		}
			
		this.FixScaleX ();

		currentState = nextState;
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag ("Enemy")) {
			other.gameObject.SendMessage ("RecieveDamage", this.comboCount);
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

	bool GetAttack () {
		return Input.GetButtonDown("Fire1");
	}

	bool GetJump () {
		return Input.GetButtonDown("Jump");
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
}
