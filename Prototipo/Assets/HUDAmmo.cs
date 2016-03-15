using UnityEngine;
using System.Collections;

public class HUDAmmo : MonoBehaviour
{

	public GameObject hudBullet;

	private GameObject[] bullets;
	private int currentAmmo;
	private Vector2 initialPos;

	void Start ()
	{
		initialPos = transform.position;
		NewWeapon ();
	}

	public void NewWeapon ()
	{
		bullets = new GameObject[GameManager.instance.initialAmmo];
		currentAmmo = bullets.Length - 1;
		for (int i = 0; i < bullets.Length; i++) {
			Vector3 pos = initialPos;
			pos.x += (0.2f * (i % 10));
			pos.y -= (int)(i / 10) * 0.5f;
			pos.z = 10;
			GameObject go = (GameObject)GameObject.Instantiate (hudBullet, pos, Quaternion.identity);
			go.transform.parent = transform;
			bullets [i] = go;
		}
	}

	public void RemoveBullet ()
	{
		bullets [currentAmmo].SetActive (false);
		if (currentAmmo <= 0)
			return;


		currentAmmo--;
	}

	public void Reload ()
	{
		currentAmmo = bullets.Length - 1;
		for (int i = 0; i < bullets.Length; i++) {
			bullets [i].SetActive (true);
		}
	}

	void Update ()
	{
	
	}
}
