using UnityEngine;
using System.Collections;

public class Weapon : MonoBehaviour
{

	public static Weapon Pistol = new Weapon (5f, 2, 6, 1);
	public static Weapon TommyGun = new Weapon (10f, 0, 20, 2);

	private float fireRate;
	private float recoil;
	private int magSize;
	private int reloadTime;

	private Weapon (float _fireRate, float _recoil, int _magSize, int _reloadTime)
	{

		this.fireRate = _fireRate;
		this.recoil = _recoil;
		this.magSize = _magSize;
		this.reloadTime = _reloadTime;
	}

	public float FireRate {
		get{ return fireRate; }
	}

	public float Recoil {
		get{ return recoil; }
	}

	public int MagSize {
		get{ return magSize; }
	}

	public float ReloadTime {
		get{ return reloadTime; }
	}

}
