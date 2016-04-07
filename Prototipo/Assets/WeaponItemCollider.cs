using UnityEngine;
using System.Collections;

public class WeaponItemCollider : MonoBehaviour
{
	public Weapon weaponType;

	void Start ()
	{
		WeaponController controller = GetComponentInParent<WeaponController> ();

		weaponType = controller.GetWeapon ();
	}

	void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			//coll.SendMessage ("OnItemRange", this);
			coll.gameObject.GetComponent<KaneShooterController> ().OnItemRange (this);
		}
	}

	void OffTriggerExit2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Player") {
			//coll.SendMessage ("OutOfItemRange", this);
			coll.gameObject.GetComponent<KaneShooterController> ().OutOfItemRange (this);
		}
	}
}
