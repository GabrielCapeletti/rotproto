using UnityEngine;
using System.Collections;

public class BigExplosionController : MonoBehaviour
{
	CircleCollider2D collider;

	void Start ()
	{
	}

	public void GiveDamage ()
	{
		collider = GetComponent<CircleCollider2D> ();
		collider.enabled = true;
	}

	public void Die ()
	{
		GameObject.Destroy (this.transform.parent.gameObject);
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.tag == "PlayerCollider") {	
			Debug.Log (coll.gameObject.tag);
			coll.SendMessage ("TakeDamage");
		}
	}
}
