using UnityEngine;
using System.Collections;

public class KaneHitController : MonoBehaviour
{
	KaneShooterController shooter;

	void Start ()
	{
		shooter = GetComponentInParent<KaneShooterController> ();
	}

	void Update ()
	{
	
	}

	void OnCollisionEnter2D (Collision2D col)
	{		
		if (col.gameObject.tag == "EnemyBullet") {			
			//	shooter.DecreaseLife ();
		}
	}
}
