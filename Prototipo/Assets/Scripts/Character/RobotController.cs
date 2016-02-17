using UnityEngine;
using System.Collections;

public class RobotController : MonoBehaviour
{
	private enum State
	{
		RUN,
		IDLE,
		COVER
	}

	public float speedX;

	private Animator animator;
	private State currentState;
	private Vector2 movementVector;
	private Rigidbody2D rigidBody;

	void Start ()
	{
		currentState = State.IDLE;
		animator = GetComponent<Animator> ();
		movementVector = Vector2.zero;
		rigidBody = GetComponent<Rigidbody2D> ();
	}

	void Update ()
	{
	
		switch (currentState) {
		case State.IDLE:
			Idle ();
			break;
		case State.RUN:
			Run ();
			break;
		case State.COVER:
			break;	
		}

		movementVector.y = rigidBody.velocity.y;
		rigidBody.velocity = movementVector;		
	}

	void Idle ()
	{
		movementVector = Vector2.zero;
		if (Input.GetKey (KeyCode.RightArrow) || Input.GetKey (KeyCode.LeftArrow) || Input.GetKey (KeyCode.Space)) {			
			currentState = State.RUN;
			animator.PlayInFixedTime ("running");
		}
	}

	void Run ()
	{
		if (Input.GetKey (KeyCode.RightArrow)) {
			movementVector = new Vector2 (speedX, 0);
			transform.localScale = new Vector2 (1, 1);
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			movementVector = new Vector2 (-speedX, 0);
			transform.localScale = new Vector2 (-1, 1);
		} else if (Input.GetKey (KeyCode.Space)) {
			rigidBody.AddForce (new Vector2 (0, 10));
		} else {
			currentState = State.IDLE;
			animator.PlayInFixedTime ("idle");
		}
	}
}
