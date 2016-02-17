using UnityEngine;
using System.Collections;

public class ShooterDetector : MonoBehaviour
{
	public GameObject firstCover;

	void Start ()
	{
	
	}

	void Update ()
	{
	
	}

	void OnTriggerEnter2D (Collider2D col)
	{
		if (col.gameObject.tag == "Kane") {
			bool[] v = { false, true };
			col.gameObject.GetComponent<PlayerController> ().ChangeMode (v);
			col.gameObject.GetComponent<KaneShooterController> ().NewCover (firstCover.GetComponent<CoverController> ());
		}
	}
}
