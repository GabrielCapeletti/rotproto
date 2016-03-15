using UnityEngine;
using System.Collections;
using UnityEditor.SceneManagement;

public class ConverHitCollider : MonoBehaviour
{
	private CoverController parentCover;
	private Collider2D colliderComp;

	void Awake ()
	{
		
		parentCover = GetComponentInParent<CoverController> ();
		colliderComp = GetComponent<BoxCollider2D> ();
	}

	public void TakeDamage ()
	{
		parentCover.TakeDamage ();
		if (parentCover.Dead) {
			colliderComp.enabled = false;
		}
	}
}
