using UnityEngine;
using System.Collections;

public class EnemyController : MonoBehaviour {


	// State Machine variables-------------
	enum STATES{
		IDLE,
		WALKING,
		DAMAGE
	};
	STATES currentState = STATES.IDLE;
	STATES nextState = STATES.IDLE;
	// State Machine variables-------------

	Animator animator;


	// Use this for initialization
	void Start () {
		this.animator = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		switch (this.currentState) {
		case STATES.IDLE:
			break;
		case STATES.WALKING:
			break;
		case STATES.DAMAGE:
			break;

		}

	}

	void RecieveDamage (int type) {
		switch (type) {
		case 1:
			this.animator.Play ("subzero-hurt-1");
			break;
		case 2:
			this.animator.Play ("subzero-hurt-2");
			break;
		case 3:
			this.animator.Play ("subzero-hurt-3");
			break;
		case 4:
			this.animator.Play ("subzero-hurt-3");
			break;
		default:
			this.animator.Play ("subzero-hurt-4");
			break;
		}
	}
}
