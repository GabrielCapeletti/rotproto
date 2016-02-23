using UnityEngine;
using System.Collections;

public class KaneHitController : MonoBehaviour
{
	void Start ()
	{
	
	}

	void Update ()
	{
	
	}

	void OnCollisionEnter2D (Collision2D col)
	{
		Debug.Log (col.gameObject.tag);
		if (col.gameObject.tag == "EnemyBullet") {
			GameManager.instance.DecreaseLife ();
		}
	}
}
