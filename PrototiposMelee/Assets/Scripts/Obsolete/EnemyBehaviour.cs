using UnityEngine;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour {

	public Transform PlayerTransform;

	public float playerDetector = 1.6f;

	public float maxSpeed = 1f;
	float speed = 1f;
		
	public GameObject attackCollider;

	Animator animator;

	enum STATES{
		IDLE,
		WALKING,
		ATTACK,
		ON_AIR
	};
	STATES currentState = STATES.IDLE;

	// Use this for initialization
	void Start () {
		this.animator = GetComponent<Animator> ();
		speed = maxSpeed;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		switch (this.currentState) {
		case STATES.IDLE:
			animator.Play ("enemy-idle");

			if (Mathf.Abs (PlayerTransform.position.x - this.GetComponent<Transform> ().position.x) < playerDetector) {
				animator.SetTrigger ("attack");
				currentState = STATES.ATTACK;
			} else if (Mathf.Abs (speed) > 0) {
				this.currentState = STATES.WALKING;
			}

			if (PlayerTransform.position.x > this.GetComponent<Transform> ().position.x && speed <= 0) {
				speed = maxSpeed;
			} else if (PlayerTransform.position.x < this.GetComponent<Transform> ().position.x && speed >= 0){
				speed = -maxSpeed;
			}

			break;
		case STATES.WALKING:
			animator.Play ("enemy-walking");
			if (Mathf.Abs (PlayerTransform.position.x - this.GetComponent<Transform> ().position.x) < playerDetector) {
				animator.SetTrigger ("attack");
				currentState = STATES.ATTACK;
			} else if (Mathf.Abs (speed) == 0) {
				this.currentState = STATES.IDLE;
			}

			if (PlayerTransform.position.x > this.GetComponent<Transform> ().position.x && speed <= 0) {
				speed = maxSpeed;
			} else if (PlayerTransform.position.x < this.GetComponent<Transform> ().position.x && speed >= 0){
				speed = -maxSpeed;
			}

			break;
		case STATES.ATTACK:
			speed = 0;
			this.ApplyDamage (0.3f, 0.7f);
			if (this.animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= 0.9) {
				this.currentState = STATES.IDLE;
			}
			break;
		}

		this.FixEnemyScaleX ();

		this.GetComponent<Rigidbody2D>().velocity = new Vector2 (speed, this.GetComponent<Rigidbody2D>().velocity.y);

	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.CompareTag ("Player")) {
			other.gameObject.SendMessage ("GetDamage", 1);
			//Debug.Log ("Player Hit");
		}
	}

	void FixEnemyScaleX () {
		if (speed > 0 && this.transform.localScale.x < 0) {
			this.transform.localScale = new Vector2 (-this.transform.localScale.x, this.transform.localScale.y);
		} else if (speed < 0 && this.transform.localScale.x > 0){
			this.transform.localScale = new Vector2 (-this.transform.localScale.x, this.transform.localScale.y);
		}
	}

	void ApplyDamage(float start, float end) {
		if (this.animator.GetCurrentAnimatorStateInfo (0).normalizedTime >= start && this.animator.GetCurrentAnimatorStateInfo (0).normalizedTime <= end) {
			this.attackCollider.SetActive (true);
		} else {
			this.attackCollider.SetActive(false);
		}
	}

}
