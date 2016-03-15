using UnityEngine;
using System.Collections;

public class ExplosiveController : MonoBehaviour
{
	public void TakeDamage ()
	{
		transform.GetChild (0).gameObject.SetActive (true);
	}
}
