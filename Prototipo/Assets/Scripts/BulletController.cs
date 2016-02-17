using UnityEngine;
using System.Collections;

public class BulletController : MonoBehaviour
{
	public float lifeTime;
	public float spread;
	public float modularSpeed;

	private Rigidbody2D rigidBody;
	private float currentLifeTime;
	private Vector3 speed;


	void Start ()
	{
		currentLifeTime = 0;
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
		
		//	rigidBody = GetComponent<Rigidbody2D> ();
		//	rigidBody.velocity = _speed;
	}

	public void SetAng (float ang)
	{		
		ang += (spread / 2) - (Random.Range (0, 1f) * spread);
		this.speed = new Vector2 (modularSpeed * Mathf.Cos (Mathf.Deg2Rad * ang), 
			modularSpeed * Mathf.Sin (Mathf.Deg2Rad * ang));

	}

	void OnTriggerEnter2D (Collider2D coll)
	{

	}
}
