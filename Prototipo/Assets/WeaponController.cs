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
	public Sprite[] sprites;

	private Weapon weaponPonter;
	private SpriteRenderer spriteRenderer;

	void Awake ()
	{
		spriteRenderer = GetComponent<SpriteRenderer> ();

		switch (weapon) {
		case WeaponType.Pistol:
			weaponPonter = Weapon.Pistol;
			spriteRenderer.sprite = sprites [0];
			break;
		case WeaponType.TommyGun:
			weaponPonter = Weapon.TommyGun;
			spriteRenderer.sprite = sprites [1];
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
