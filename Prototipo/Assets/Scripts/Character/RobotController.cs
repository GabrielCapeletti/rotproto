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
	public int playerNumber;

	private Animator animator;
	private State currentState;
	private Vector2 movementVector;
	private Rigidbody2D rigidBody;
	private SpriteRenderer renderers;
	private int lookDirection;

	void Start ()
	{
		currentState = State.IDLE;
		animator = GetComponent<Animator> ();
		movementVector = Vector2.zero;
		rigidBody = GetComponent<Rigidbody2D> ();
		renderers = GetComponent<SpriteRenderer> ();
		lookDirection = ((renderers.flipX) ? -1 : 1);
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
		if (Input.GetAxisRaw ("Horizontal" + playerNumber) != 0 || Input.GetButtonDown ("Jump" + playerNumber)) {			
			currentState = State.RUN;
			animator.PlayInFixedTime ("running");
		}
	}

	void Run ()
	{
		if (Input.GetAxisRaw ("Horizontal" + playerNumber) > 0) {
			movementVector = new Vector2 (speedX, 0);
			Quaternion quart = new Quaternion (0, 0, 0, 0);
			this.transform.localRotation = quart;
			//transform.localScale = new Vector2 (lookDirection * 1, 1);
		} else if (Input.GetAxisRaw ("Horizontal" + playerNumber) < 0) {
			movementVector = new Vector2 (-speedX, 0);
			Quaternion quart = new Quaternion (0, 180, 0, 0);
			this.transform.localRotation = quart;
			//transform.localScale = new Vector2 (-1 * lookDirection, 1);
		} else if (Input.GetButtonDown ("Jump" + playerNumber)) {
			rigidBody.AddForce (new Vector2 (0, 10));
		} else {
			currentState = State.IDLE;
			animator.PlayInFixedTime ("idle");
		}
	}
}
