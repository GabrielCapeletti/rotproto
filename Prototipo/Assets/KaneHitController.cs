using UnityEngine;
using System.Collections;

public class KaneHitController : MonoBehaviour
{
	KaneShooterController shooter;
	string bulletTag;

	void Start ()
	{
		shooter = GetComponentInParent<KaneShooterController> ();
		bulletTag = shooter.bullet.tag;
	}

	void Update ()
	{
	
	}

	public void TakeDamage ()
	{
		shooter.DecreaseLife ();
	}

	void OnCollisionEnter2D (Collision2D col)
	{		
		if (col.gameObject.tag == "Bullet") {		
			shooter.DecreaseLife ();
		}
	}
}
