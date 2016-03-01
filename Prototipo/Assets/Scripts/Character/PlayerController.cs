using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
	public bool[] starts;

	public enum Mode
	{
		NORMAL = 0,
		SHOOTER = 1
	}

	KaneShooterController shooterMode;
	RobotController robotMode;

	void Start ()
	{
		this.shooterMode = this.GetComponent<KaneShooterController> ();
		this.robotMode = this.GetComponent<RobotController> ();

		this.robotMode.enabled = this.starts [0];
		this.shooterMode.enabled = this.starts [1];

		this.shooterMode.start ();
	}

	public void ChangeMode (bool[] v)
	{
		this.robotMode.enabled = v [0];
		this.shooterMode.enabled = v [1];
	}

	void Update ()
	{
		
	}
}
