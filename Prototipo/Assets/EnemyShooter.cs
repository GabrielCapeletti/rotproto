using UnityEngine;
using System.Collections;

public class EnemyShooter : MonoBehaviour
{
	public GameObject gun;
	public GameObject tommyGun;
	public GameObject gunTip;
	public GameObject bullet;
	public float fireRate;
	public int totalBullets;
	public float reloadingTime;

	private LineRenderer gunTipenderer;
	private GameObject kane;
	private float aimAng = 0;
	private bool invertedAim = false;
	private float lastBulletTime = 10;
	private int currentBullet;
	private float currentReloadingTime;


	private enum State
	{
		UNDER_COVER,
		SHOOTING
	}

	private State currentState;

	void Start ()
	{
		currentState = State.UNDER_COVER;
		gunTipenderer = gunTip.GetComponent<LineRenderer> ();
		kane = GameObject.Find ("Robot");
		currentBullet = totalBullets;
	}

	void Update ()
	{
		switch (currentState) {
		case State.UNDER_COVER:
			UnderCover ();
			break;
		case State.SHOOTING:
			Shooting ();
			break;
		default:
			break;
		}
	}

	void UnderCover ()
	{	
		currentReloadingTime += Time.deltaTime;

		if (currentReloadingTime > reloadingTime) {
			currentReloadingTime = 0;
			this.currentState = State.SHOOTING;
			gunTipenderer.SetPosition (0, new Vector3 (transform.position.x, transform.position.y, 0));
			gun.SetActive (true);
		}

	}

	public void Shooting ()
	{
		invertedAim = !(kane.transform.position.x > transform.position.x);

		this.transform.localScale = new Vector3 ((invertedAim ? -1 : 1), 1, 1);

		aimAng = Mathf.Rad2Deg * Mathf.Atan2 (transform.position.y - kane.transform.position.y, 
			transform.position.x - kane.transform.position.x);

		Debug.Log (aimAng);

		gunTipenderer.SetPosition (1, new Vector3 (transform.position.x + Mathf.Cos (Mathf.Deg2Rad * (180 + aimAng)) * 20,
			transform.position.y + Mathf.Sin (Mathf.Deg2Rad * (aimAng + 180)) * 20, 0));

		lastBulletTime += Time.deltaTime;
		if (lastBulletTime > 1 / fireRate) {					
			GameObject go = (GameObject)Instantiate (bullet, transform.position, Quaternion.identity);
			go.GetComponent<BulletController> ().SetAng (aimAng + 180);
			lastBulletTime = 0;
			currentBullet--;
			if (currentBullet <= 0) {
				currentBullet = totalBullets;
				this.currentState = State.UNDER_COVER;
				gun.SetActive (false);
			}
		}
		
	}

}
