using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour 
{
    private Animator anim;
    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;
    private Vector3 miningStationLocation;
    private Vector3 playerVelocity;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;
    private List<Rigidbody> enemies = new List<Rigidbody>();
    private Rigidbody playerRigidBody;
    private GameObject healthBar;
    private GameObject deathText;

    public float playerLife = 100;  
    public Text winText;
    public float speed = 600.0f;
    public float jumpForce = 200;
    public float turnSpeed = 400.0f;
    public float gravity = 20.0f;
    public GameObject miningStation;
    public GameObject deactivatedMiningStation;
    public GameObject playerObj;

    private void SetActivation(GameObject auxObject, bool state)
    {
        auxObject.gameObject.SetActive(state);
    }

    private void MoveBackwards()
    {
        
    }

    private void TakeDamage()
    {
        playerLife = playerLife - 0.5f;
        healthBar.GetComponent<HealthBar>().UpdateLife(playerLife);
        MoveBackwards();
    }

    private void Death()
    {
        SetActivation(deathText, true);
        Time.timeScale = 0;
    }

    private void CheckDeath()
    {
        if (playerLife == 0)
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

    private void DeactivateMiningStation()
    {
        SetActivation(miningStation, false);
        SetActivation(deactivatedMiningStation, true);
        SetActivation(winText.gameObject, true);
        Time.timeScale = 0;
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

    private void GetEnemiesRigidbodies(IEnumerable<GameObject> enemiesObjects)
    {
        foreach (GameObject go in enemiesObjects)
        {
            enemies.Add(go.GetComponent<Rigidbody>());
        } 
    }

    private void FindAllEnemies()
    {
        var enemiesObjects = Resources.FindObjectsOfTypeAll<GameObject>().Where(obj => Regex.IsMatch(obj.name, 
                                                                                                     "lizardman", 
                                                                                                     RegexOptions.IgnoreCase));
        GetEnemiesRigidbodies(enemiesObjects);
    }

    private float CheckDistance(Vector3 playerLocation, Rigidbody enemy)
    {
        return Vector2.Distance(new Vector2(enemy.position.x, 
                                            enemy.position.z), 
                                new Vector2(playerLocation.x,
                                            playerLocation.z));
    }

    private void CheckEnemiesProximity(Vector3 playerLocation)
    {
        foreach (Rigidbody enemy in enemies)
        {
            if (CheckDistance(playerLocation, enemy) < 0.5f && playerLife > 0)
            {
                // TakeDamage();
            }
        }
    }

    private void OnCollisionEnter(Collision c)
    {
        if (c.gameObject.tag == "enemy")
        {
            TakeDamage();
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
        playerRigidBody = GetComponent<Rigidbody>();
        miningStationLocation = getObjectLocation(miningStation);
        SetActivation(miningStation.gameObject, true);
        SetActivation(deactivatedMiningStation.gameObject, false);
        FindAllEnemies();
        GetHealthBar();
        GetDeathText();
        SetActivation(deathText, false);
    }

    void Update()
    {
        Vector3 playerLocation = getObjectLocation(playerObj);

        AnimationTrigger();
        Movement();
        CheckEnemiesProximity(playerLocation);
        CheckDeath();

        if (Vector3.Distance(playerLocation, miningStationLocation) <= 2)
        {
            if (Input.GetKey("z")) 
            {
                DeactivateMiningStation();
            }
        }
    }
}