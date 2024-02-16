using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercontrolor : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private Camera followCamera;

    [SerializeField] private float rotationSpeed = 10f;
    
    private Vector3 playerVelocity;
    [SerializeField] private float gravityValue = -13f;

    public bool groundPlayer;
    [SerializeField] private float jumpHeight = 2.5f;

    public Animator animator;

    public bool isdead;

    public ParticleSystem damageParticle;
    public ParticleSystem deadParticle;
    public void Awake()
    {
        instance = this;
    }


    public static playercontrolor instance;
    // Start is called before the first frame update
    void Start()
    {
        damageParticle.Stop();
        deadParticle.Stop();
        characterController = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (winner.instance.isWinner|| isdead)
        {
            case true:
                animator.SetBool("victory", winner.instance.isWinner);
                animator.SetBool("dead", true);
                fixGravityWhenPlayDead();
                break;
            case false:
                break;
        }
        Movment();
    } 
    void Movment()
    {
        groundPlayer = characterController.isGrounded;
        if(characterController.isGrounded && playerVelocity.y < -2)
        {
            playerVelocity.y = -1f;
        }

        float horizentalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movemetInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) 
                              * new Vector3(horizentalInput, 0, verticalInput);
        Vector3 movementDirection = movemetInput.normalized;

        characterController.Move(movementDirection*playerSpeed*Time.deltaTime);

        if(movementDirection != Vector3.zero)
        {
            Quaternion desiredRotatiom = Quaternion.LookRotation(movementDirection, Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation,desiredRotatiom, rotationSpeed*Time.deltaTime);
        }

        if(Input.GetButtonDown("Jump") && groundPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
            animator.SetTrigger("jumping");
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        animator.SetFloat("speed", Mathf.Abs(movementDirection.x) + Mathf.Abs(movementDirection.z));
        animator.SetBool("ground", characterController.isGrounded);

    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("BoxDamage"))
        {
            ShowDamgeParticles();
            isdead = true;
        }
    }

    public void ShowDamgeParticles()
    {
        TogglesSlowMotion();
        damageParticle.Play();
        deadParticle.Play();
        StartCoroutine(delaySlow());
    }

    void TogglesSlowMotion()
    {
        Time.timeScale = 0.5f;
    }

    IEnumerator delaySlow()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 1;
    }
    void fixGravityWhenPlayDead()
    {

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);
    }
} 