using UnityEngine;
using System.Collections;

public class ExplosionController : MonoBehaviour
{
	CircleCollider2D collider;

	void Start ()
	{
	}

	public void GiveDamage ()
	{
	}

	public void Die ()
	{
		GameObject.Destroy (this.transform.parent.gameObject);
	}

}
