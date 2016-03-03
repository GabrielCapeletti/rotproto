using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class KaneShooterController : MonoBehaviour
{
	public CoverController cover;
	public GameObject gun;
	public GameObject tommyGun;
	public GameObject gunTip;
	public GameObject bullet;
	public GameObject hitCollider;
	public HUDAmmo hudAmmo;
	public HUDLife hudLife;
	public CameraController camera;
	public int initialAmmo;
	[Tooltip ("balas por segundo")]
	public float fireRate;
	public float recoil;
	public float reloadTime;
	public int playerNumber;

	private int initialLife;
	private int currentLife;

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
	private int lookDirection;
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
		lookDirection = ((this.GetComponent<SpriteRenderer> ().flipX) ? 1 : -1);
		initialLife = GameManager.instance.initialLife;
		currentLife = initialLife;
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
				currentAmmo = Reload ();
				currentReloadTime = 0;
			}
			return;
		}
		if (Input.GetAxisRaw ("Horizontal" + playerNumber) > 0) {
			Quaternion quart = new Quaternion (0, 0, 0, 0);
			this.transform.localRotation = quart;
			invertedAim = false;
			if (Input.GetAxisRaw ("Vertical" + playerNumber) > 0) {
				cover.nextUp.HighLight ();
				if (Input.GetButtonDown ("Jump" + playerNumber)) {
					NewCover (cover.nextUp);
				}
			} else {
				cover.next.HighLight ();
				if (Input.GetButtonDown ("Jump" + playerNumber)) {
					NewCover (cover.next);
				}
			}
		} else if (Input.GetAxisRaw ("Horizontal" + playerNumber) < 0) {
			cover.previous.HighLight ();
			Quaternion quart = new Quaternion (0, lookDirection * 180, 0, 0);
			this.transform.localRotation = quart;
			invertedAim = true;
			if (Input.GetButtonDown ("Jump" + playerNumber)) {
				NewCover (cover.previous);
			}

		} else if (Input.GetAxisRaw ("Aim" + playerNumber) != 0) {			
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
			currentAmmo = DecreaseAmmo ();
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
		if (Input.GetAxisRaw ("Aim" + playerNumber) != 0) {			
			if (Input.GetAxisRaw ("Vertical" + playerNumber) > 0) {
				this.AimUp ();
			} else if (Input.GetAxisRaw ("Vertical" + playerNumber) < 0) {
				this.AimDown ();
			}

			if (Input.GetAxisRaw ("Fire1" + playerNumber) > 0) {	
				this.ShootingProjectile ();
			} else {
				particles.SetActive (false);
			}

			if (invertedAim) {
				gunTipenderer.SetPosition (0, new Vector3 (transform.position.x, transform.position.y, 0));
				gunTipenderer.SetPosition (1, new Vector3 (transform.position.x + Mathf.Cos (Mathf.Deg2Rad * (180 - aimAng)) * 10,
					transform.position.y + Mathf.Sin (Mathf.Deg2Rad * (180 - aimAng)) * 10, 0));
			} else {
				gunTipenderer.SetPosition (0, new Vector3 (transform.position.x, transform.position.y, 0));
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

	public int DecreaseLife ()
	{
		currentLife--;
		hudLife.RemoveLife ();

		if (currentLife < 0)
			SceneManager.LoadScene (0);

		return currentLife;
	}


	public void Transitioning ()
	{		
		transform.position = Vector2.Lerp (transform.position, this.target, Time.deltaTime * 6f);
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
		this.target = _cover.PositionOne;

		rigidBody = GetComponent<Rigidbody2D> ();
		rigidBody.velocity = Vector2.zero;
		animator.Play ("idle");
	}

}
