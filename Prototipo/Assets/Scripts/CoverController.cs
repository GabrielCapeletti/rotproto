using UnityEngine;
using System.Collections;

public class CoverController : MonoBehaviour
{
	public CoverController nextUp;
	public CoverController next;
	public CoverController nextDown;
	public CoverController previous;

	private SpriteRenderer renderer;

	private bool highlighted = false;

	void Start ()
	{
		renderer = GetComponent<SpriteRenderer> ();

		if (nextUp == null)
			nextUp = this;


		if (next == null)
			next = this;

		if (nextDown == null)
			nextDown = this;
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
