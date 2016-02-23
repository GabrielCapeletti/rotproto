using UnityEngine;
using System.Collections;

public class EnemyHitController : MonoBehaviour
{

	void Start ()
	{
	
	}

	void Update ()
	{
	
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		if (col.gameObject.tag == "KaneBullet") {
			//Destroy (transform.parent.gameObject);
		}
	}
}
