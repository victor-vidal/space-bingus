using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class Player : MonoBehaviour 
{
	private Animator anim;
	private CharacterController controller;
	private Vector3 moveDirection = Vector3.zero;
	
	private Vector3 playerVelocity;
	private float jumpHeight = 1.0f;
	private float gravityValue = -9.81f;
	private float playerLife = 100;
	private GameObject healthBar;
	private GameObject deathText;
	
	public Text winText;
	public Text Counter;
	public float speed = 600.0f;
	public float turnSpeed = 400.0f;
	public float gravity = 20.0f;
	public GameObject playerObj;

	// MiningStations
	private float deactivatedMiningStations = 0;
	private Vector3 miningStation01Location;
	public GameObject miningStation01;
	public GameObject deactivatedMiningStation01;
	public GameObject particles01;
	private Vector3 miningStation02Location;
	public GameObject miningStation02;
	public GameObject deactivatedMiningStation02;
	public GameObject particles02;
	private Vector3 miningStation03Location;
	public GameObject miningStation03;
	public GameObject deactivatedMiningStation03;
	public GameObject particles03;

	// TextBox
	public GameObject textBox;

	private void SetActivation(GameObject auxObject, bool state)
	{
		auxObject.gameObject.SetActive(state);
	}

	private void TakeDamage(float damage)
	{
		playerLife = playerLife - damage;
		healthBar.GetComponent<HealthBar>().UpdateLife(playerLife);
		CheckDeath();
	}

	private void Death()
	{
		SetActivation(deathText, true);
		Time.timeScale = 0;
	}

	private void CheckDeath()
	{
		if (playerLife < 0)
		{
			Death();
		}
	}

	private void AnimationTrigger()
	{
		if (Input.GetKey("w")) 
		{
			anim.SetInteger("AnimationPar", 1);
		} else 
		{
			anim.SetInteger("AnimationPar", 0);
		}
	}

	private void CheckWin() {
		if (deactivatedMiningStations == 3) {
			SetActivation(winText.gameObject, true);
			Time.timeScale = 0;
		}
	}

	private void DeactivateMiningStation(
		GameObject miningStation, 
		GameObject deactivatedMiningStation,
		GameObject particles 
		)
	{
		SetActivation(miningStation, false);
		SetActivation(deactivatedMiningStation, true);
		SetActivation(particles, false);

		deactivatedMiningStations += 1;
		CheckWin();
	}

	private void Movement()
	{
		if(controller.isGrounded)
		{
			moveDirection = transform.forward * Input.GetAxis("Vertical") * speed;
		}

		if (controller.isGrounded && moveDirection.y < 0)
        {
            moveDirection.y = 0f;
        }

        if (Input.GetButtonDown("Jump") && controller.isGrounded)
        {
            moveDirection.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }
        moveDirection.y += gravityValue * Time.deltaTime;

		float turn = Input.GetAxis("Horizontal");
		transform.Rotate(0, turn * turnSpeed * Time.deltaTime, 0);
		controller.Move(moveDirection * Time.deltaTime);
	}

	private Vector3 getObjectLocation(GameObject auxObject)
	{
		return new Vector3(auxObject.transform.position.x, 
                           auxObject.transform.position.y,
                           auxObject.transform.position.z);
	}

	private void OnCollisionEnter(Collision c)
	{
		if (c.gameObject.tag == "enemy")
		{
			TakeDamage(5f);
		} else if (c.gameObject.tag == "enemy2") {
			TakeDamage(10f);
		} else if (c.gameObject.tag == "enemy3") {
			TakeDamage(15f);
		}
	}

	private void GetHealthBar()
	{
		healthBar = GameObject.Find("HealthBarInner");
	}

	private void GetDeathText()
	{
		deathText = GameObject.Find("GameOver");
	}

	void Start() 
	{
		controller = GetComponent<CharacterController>();
		anim = GetComponentInChildren<Animator>();
		miningStation01Location = getObjectLocation(miningStation01);
		miningStation02Location = getObjectLocation(miningStation02);
		miningStation03Location = getObjectLocation(miningStation03);
		SetActivation(miningStation01.gameObject, true);
		SetActivation(deactivatedMiningStation01.gameObject, false);
		SetActivation(particles01.gameObject, true);
		SetActivation(miningStation02.gameObject, true);
		SetActivation(deactivatedMiningStation02.gameObject, false);
		SetActivation(particles02.gameObject, true);
		SetActivation(miningStation03.gameObject, true);
		SetActivation(deactivatedMiningStation03.gameObject, false);
		SetActivation(particles03.gameObject, true);
		GetHealthBar();
		GetDeathText();
		SetActivation(deathText, false);
	}

	void UpdateCounter() {
		Counter.text = "" + deactivatedMiningStations;
	}

	void Update()
	{
		Vector3 playerLocation = getObjectLocation(playerObj);

		UpdateCounter();

		AnimationTrigger();
		Movement();

		if (Input.GetKey("z") && (textBox.activeSelf)) 
		{
			SetActivation(textBox, false);
		}
		if ((Vector3.Distance(playerLocation, miningStation01Location) <= 2) && (miningStation01.activeSelf))
		{
			if (Input.GetKey("z")) 
			{
				DeactivateMiningStation(miningStation01, deactivatedMiningStation01, particles01);
			}
		} else if ((Vector3.Distance(playerLocation, miningStation02Location) <= 2) && (miningStation02.activeSelf)) {	
			
			if (Input.GetKey("z")) 
			{
				DeactivateMiningStation(miningStation02, deactivatedMiningStation02, particles02);
			}
		} else if ((Vector3.Distance(playerLocation, miningStation03Location) <= 2) && (miningStation03.activeSelf)) {
			if (Input.GetKey("z")) 
			{
				DeactivateMiningStation(miningStation03, deactivatedMiningStation03, particles03);
			}
		}
	}
}