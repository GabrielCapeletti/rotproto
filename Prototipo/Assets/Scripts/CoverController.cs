using UnityEngine;
using System.Collections;

public class CoverController : MonoBehaviour
{
	public CoverController nextUp;
	public CoverController next;
	public CoverController nextDown;
	public CoverController previousUp;
	public CoverController previous;
	public CoverController previousDown;
	public bool destructible;
	public GameObject hitCollider;
	public int initiaLife;
	public Sprite[] sprites;

	private bool dead = false;
	private int currentLife;
	private Vector2 positionOne;
	private Vector2 positionTwo;
	private SpriteRenderer renderer;
	private bool highlighted = false;

	void Start ()
	{
		renderer = GetComponent<SpriteRenderer> ();
		currentLife = initiaLife;
		positionOne = transform.position;
		positionOne.x -= 0.8f;
		positionTwo = transform.position;
		positionTwo.x += 0.8f;

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

	public void OnCover ()
	{
		hitCollider.SetActive (true);
	}

	public void OffCover ()
	{
		hitCollider.SetActive (false);
	}

	void Update ()
	{	
		if (highlighted) {
			renderer.color = Color.gray;
		} else {
			renderer.color = Color.white;
		}
		highlighted = false;
	}

	public bool Dead {
		get { return dead; }
	}

	public void HighLight ()
	{
		highlighted = true;
	}

	public void TakeDamage ()
	{
		if (destructible) {
			currentLife--;
			if ((initiaLife / 2) > currentLife) {
				this.renderer.sprite = sprites [1];
				if (currentLife <= 0) {
					dead = true;
					this.renderer.sprite = sprites [2];
				}
			}
		}
	}

}
