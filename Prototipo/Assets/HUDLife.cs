using UnityEngine;
using System.Collections;

public class HUDLife : MonoBehaviour
{

	public GameObject hudLife;

	private GameObject[] lifes;
	private int currentLife;
	private Vector2 initialPos;

	void Start ()
	{
		initialPos = transform.position;
		lifes = new GameObject[GameManager.instance.initialLife];
		currentLife = lifes.Length - 1;
		for (int i = 0; i < lifes.Length; i++) {
			Vector3 pos = initialPos;
			pos.x += (0.4f * (i));
			pos.z = 10;
			GameObject go = (GameObject)GameObject.Instantiate (hudLife, pos, Quaternion.identity);
			go.transform.parent = transform;
			lifes [i] = go;
		}
	}

	public void RemoveLife ()
	{
		lifes [currentLife].SetActive (false);
		if (currentLife <= 0)
			return;

		currentLife--;
	}

	void Update ()
	{

	}
}
