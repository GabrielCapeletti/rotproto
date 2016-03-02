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

}
