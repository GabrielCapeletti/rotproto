using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
	public HUDAmmo hudAmmo;
	public HUDLife hudLife;

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
		hudLife.RemoveLife ();
		if (currentLife < 0)
			SceneManager.LoadScene (0);
		
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
