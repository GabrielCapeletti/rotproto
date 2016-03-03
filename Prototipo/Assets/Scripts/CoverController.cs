using UnityEngine;
using System.Collections;

public class CoverController : MonoBehaviour
{
	public CoverController nextUp;
	public CoverController next;
	public CoverController nextDown;
	public CoverController previous;

	private Vector2 positionOne;
	private Vector2 positionTwo;

	private SpriteRenderer renderer;

	private bool highlighted = false;

	void Start ()
	{
		renderer = GetComponent<SpriteRenderer> ();

		positionOne = transform.position;
		positionOne.x -= 0.5f;
		positionTwo = transform.position;
		positionTwo.x -= 0.5f;

		if (nextUp == null)
			nextUp = this;


		if (next == null)
			next = this;

		if (nextDown == null)
			nextDown = this;
	}

	public Vector2 PositionOne {
		get {
			return positionOne;
		}
	}

	public Vector2 PositionTwo {
		get {
			return positionTwo;
		}
	}

	void Update ()
	{	
		if (highlighted) {
			renderer.color = Color.gray;
		} else {
			renderer.color = Color.black;
		}
		highlighted = false;
	}

	public void HighLight ()
	{
		highlighted = true;
	}

}
