using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class BulletController : MonoBehaviour
{
	public float lifeTime;
	public float spread;
	public float modularSpeed;
	public GameObject explosion;

	private SpriteRenderer renderer;
	private Rigidbody2D rigidBody;
	private float currentLifeTime;
	private Vector3 speed;

	void Start ()
	{
		currentLifeTime = 0;
		renderer = GetComponent<SpriteRenderer> ();
	}

	void Update ()
	{
		currentLifeTime += Time.deltaTime;
		if (currentLifeTime > lifeTime) {
			GameObject.Destroy (this.gameObject);
		}
	
	}

	void FixedUpdate ()
	{
		transform.position += this.speed * Time.deltaTime;	
	}

	public void SetSpeed (Vector2 _speed)
	{		
		this.speed = _speed * Random.Range (0, 1f) + _speed;
	}

	public void SetAng (float ang)
	{		
		ang += (spread / 2) - (Random.Range (0, 1f) * spread);
		this.speed = new Vector2 (modularSpeed * Mathf.Cos (Mathf.Deg2Rad * ang), 
			modularSpeed * Mathf.Sin (Mathf.Deg2Rad * ang));

	}

	public void OnCollisionEnter2D (Collision2D coll)
	{		
		if (coll.gameObject.tag == "Cover") {			
			coll.collider.SendMessage ("TakeDamage");
		}
		renderer.enabled = false;
		this.speed = Vector2.zero;
		explosion.SetActive (true);
	}
}
