using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public HUDAmmo hudAmmo;

	public static GameManager instance;

	public int initialAmmo;
	public int initialLife;

	private int currentAmmo;
	private int currentLife;

	void Start ()
	{
		currentAmmo = initialAmmo;
		currentLife = initialLife;
	}

	void Awake ()
	{
		instance = this;
	}

	public int DecreaseLife ()
	{
		currentLife--;
		return currentLife;
	}

	public int DecreaseAmmo ()
	{
		hudAmmo.RemoveBullet ();
		currentAmmo--;
		return currentAmmo;
	}

	public int Reload ()
	{
		currentAmmo = initialAmmo;
		hudAmmo.Reload ();
		return initialAmmo;
	}

}
