using UnityEngine;
using System.Collections;

public class KaneShooterController : MonoBehaviour
{
	public CoverController cover;
	public GameObject gun;
	public GameObject tommyGun;
	public GameObject gunTip;
	public GameObject bullet;
	public GameObject hitCollider;
	public CameraController camera;
	[Tooltip ("balas por segundo")]
	public float fireRate;
	public float recoil;
	public float reloadTime;

	private LineRenderer gunTipenderer;
	private Vector2 target;
	private Animator animator;
	private Rigidbody2D rigidBody;
	private Vector2 teste = Vector2.zero;
	private float aimAng = 0;
	private float lastBulletTime = 10;
	private GameObject particles;
	private bool invertedAim = false;
	private int currentAmmo;

	private float currentReloadTime;

	private enum State
	{
		UNDER_COVER,
		SHOOTING,
		TRANSITIONING
	}

	private State currentState;

	public void start ()
	{
		this.Start ();
	}


	void Start ()
	{
		animator = GetComponent<Animator> ();
		currentAmmo = GameManager.instance.initialAmmo;
		gunTipenderer = gunTip.GetComponent<LineRenderer> ();
		particles = tommyGun.transform.GetChild (0).gameObject;
		currentReloadTime = 0;
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
		case State.TRANSITIONING:
			Transitioning ();
			break;
		default:
			break;
		}
	}

	void UnderCover ()
	{
		if (currentAmmo <= 0) {
			currentReloadTime += Time.deltaTime;
			if (currentReloadTime > reloadTime) {
				currentAmmo = GameManager.instance.Reload ();
				currentReloadTime = 0;
			}
			return;
		}
		
		if (Input.GetKey (KeyCode.RightArrow)) {
			Quaternion quart = new Quaternion (0, 0, 0, 0);
			this.transform.localRotation = quart;
			invertedAim = false;
			if (Input.GetKey (KeyCode.UpArrow)) {
				cover.nextUp.HighLight ();
				if (Input.GetKey (KeyCode.Space)) {
					NewCover (cover.nextUp);
				}
			} else {
				cover.next.HighLight ();
				if (Input.GetKey (KeyCode.Space)) {
					NewCover (cover.next);
				}
			}
		} else if (Input.GetKey (KeyCode.LeftArrow)) {
			cover.previous.HighLight ();
			Quaternion quart = new Quaternion (0, 180, 0, 0);
			this.transform.localRotation = quart;
			invertedAim = true;
			if (Input.GetKey (KeyCode.Space)) {
				NewCover (cover.previous);
			}
		} else if (Input.GetKey (KeyCode.LeftShift)) {			
			this.GoToShooting ();
		}
	}

	public void GoToShooting ()
	{
		gunTipenderer.SetPosition (0, new Vector3 (transform.position.x, transform.position.y, 0));
		gun.SetActive (true);
		hitCollider.SetActive (true);
		this.currentState = State.SHOOTING;		
	}

	public void AimUp ()
	{
		aimAng += 1f;
		if (aimAng >= 85) {
			aimAng = 84;
		} else {
			tommyGun.transform.Rotate (new Vector3 (0, 0, 1));
		}
	}

	public void AimDown ()
	{
		aimAng -= 1f;
		if (aimAng <= -85) {
			aimAng = -84;
		} else {
			tommyGun.transform.Rotate (new Vector3 (0, 0, -1));
		}
	}

	public void ShootingProjectile ()
	{
		particles.SetActive (true);
		if (lastBulletTime > 1 / fireRate && currentAmmo > 0) {
			currentAmmo = GameManager.instance.DecreaseAmmo ();
			GameObject go = (GameObject)Instantiate (bullet, transform.position, Quaternion.identity);
			go.GetComponent<BulletController> ().SetAng ((invertedAim ? 180 - aimAng : aimAng));
			lastBulletTime = 0;
			camera.ShakeCamera ();
			aimAng += recoil;
			if (aimAng >= 85) {
				aimAng = 84;
			} else {
				tommyGun.transform.Rotate (new Vector3 (0, 0, recoil));
			}
		}
	}

	public void Shooting ()
	{		
		if (Input.GetKey (KeyCode.LeftShift)) {			
			if (Input.GetKey (KeyCode.UpArrow)) {
				this.AimUp ();
			} else if (Input.GetKey (KeyCode.DownArrow)) {
				this.AimDown ();
			}

			if (Input.GetKey (KeyCode.Space)) {	
				this.ShootingProjectile ();
			} else {
				particles.SetActive (false);
			}

			if (invertedAim) {
				gunTipenderer.SetPosition (1, new Vector3 (transform.position.x + Mathf.Cos (Mathf.Deg2Rad * (180 - aimAng)) * 10,
					transform.position.y + Mathf.Sin (Mathf.Deg2Rad * (180 - aimAng)) * 10, 0));
			} else {
				gunTipenderer.SetPosition (1, new Vector3 (transform.position.x + Mathf.Cos (Mathf.Deg2Rad * (aimAng)) * 10,
					transform.position.y + Mathf.Sin (Mathf.Deg2Rad * (aimAng)) * 10, 0));
			}
		} else {						
			this.GoUndercover ();
		}

		if (lastBulletTime < 10) {
			lastBulletTime += Time.deltaTime;
		}
	}

	public void GoUndercover ()
	{
		particles.SetActive (false);
		hitCollider.SetActive (false);
		this.currentState = State.UNDER_COVER;
		gun.SetActive (false);
	}

	public void Transitioning ()
	{		
		transform.position = Vector2.Lerp (transform.position, this.target, 0.1f);
		animator.Play ("rolling");
		if (Vector2.Distance (transform.position, target) < 0.1f) {
			this.currentState = State.UNDER_COVER;
			hitCollider.SetActive (false);
			this.rigidBody.gravityScale = 1;
		}
	}

	public void NewCover (CoverController _cover)
	{
		if (this.rigidBody == null) {
			this.rigidBody = GetComponent<Rigidbody2D> ();
		}
		this.rigidBody.gravityScale = 0;
		
		this.cover = _cover;
		this.currentState = State.TRANSITIONING;
		hitCollider.SetActive (true);
		this.target = new Vector2 (cover.transform.position.x, cover.transform.position.y);

		rigidBody = GetComponent<Rigidbody2D> ();
		rigidBody.velocity = Vector2.zero;
		animator.Play ("idle");
	}

}
