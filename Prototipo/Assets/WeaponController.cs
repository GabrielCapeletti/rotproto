using UnityEngine;
using System.Collections;

public class WeaponController : MonoBehaviour
{
	public enum WeaponType
	{
		Pistol,
		TommyGun
	}

	public WeaponType weapon;

	private Weapon weaponPonter;

	void Start ()
	{
		switch (weapon) {
		case WeaponType.Pistol:
			weaponPonter = Weapon.Pistol;
			break;
		case WeaponType.TommyGun:
			weaponPonter = Weapon.TommyGun;
			break;
		default:
			break;
		}
	}

	public Weapon GetWeapon ()
	{
		return weaponPonter;
	}
}
