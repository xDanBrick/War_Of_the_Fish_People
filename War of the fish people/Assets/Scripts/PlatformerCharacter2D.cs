using System;
using UnityEngine;

public class PlatformerCharacter2D : MonoBehaviour
{
	private const int attackCounterMax = 50;
    [SerializeField] private float jumpForce = 400f;                  // Amount of force added when the player jumps.
	[SerializeField] private float gridSize = 1.0f;
	[SerializeField] private GameObject bulletAim;
	[SerializeField] private GameObject bullet;
	[SerializeField] private GameObject selectedIcon;
	[SerializeField] private int team = 1;
    private Animator animator;            // Reference to the player's animator component.
    private Rigidbody2D body;
	[SerializeField] private bool facingRight = false;  // For determining which way the player is currently facing.
	private bool isMoving = false;
	private bool isAttacking = false;
	private bool isJumping = false;
	private int attackCounter = 0;
	private float startX = 0.0f;
	private bool isShooting = false;
	private float upMax = 90.0f;
	private float downMax = 0.0f;
	private GameObject aim;
	private State state = State.idle;
	
	[HideInInspector] public bool jump = false;
	[HideInInspector] public enum State{idle, moveRight, moveLeft, attack, die};
	[HideInInspector] public float dir = 1.0f;

    void Start()
    {
        animator = GetComponent<Animator>();
        body = GetComponent<Rigidbody2D>();
	}
	
	public void SelectPlayer(bool flag)
	{
		selectedIcon.SetActive (flag);
	}
	
	public void SelectState(State newState)
	{
		state = newState;
	}
	
	public void AimWeapon(bool flag)
	{
		if(flag)
		{
			aim = (GameObject) Instantiate(bulletAim, new Vector2(transform.position.x + (7.0f * dir), transform.position.y), transform.rotation);
		}
		else
		{
			Destroy(aim);
		}
	}
	
	private void FireBullet()
	{
		GameObject bulletBody = (GameObject) Instantiate(bullet, new Vector2(transform.position.x + (1.0f * dir), transform.position.y), aim.transform.rotation);
		bulletBody.GetComponent<Rigidbody2D>().AddForce ((dir *aim.transform.right) * 500.0f);
		bulletBody.GetComponent<BulletScript>().FireBullet(team, gameObject);
		aim.SetActive(false);
	}
	
	public void AimBullet(float x)
	{
		aim.transform.RotateAround (
			transform.position, new Vector3 (0.0f, 0.0f, 1.0f), -x);
	}
	
	// Update is called once per frame
	void Update () {
		if (state == State.die) {
			animator.SetTrigger("Die");
		}
		MovementUpdate();
		JumpUpdate ();
		AttackUpdate ();
	}

    public void MovementUpdate()
    {
		if (state == State.moveRight || state == State.moveLeft) {
			startX = transform.position.x;
			isMoving = true;
			// The Speed animator parameter is set to the absolute value of the horizontal input.
			animator.SetTrigger("Walking");
		
			// If the input is moving the player right and the player is facing left...
			if (state == State.moveRight && !facingRight) {
				// ... flip the player.
				Flip ();
			}
            // Otherwise if the input is moving the player left and the player is facing right...
			else if (state == State.moveLeft && facingRight) {
				// ... flip the player.
				Flip ();
			}
			// Move the character
			state = State.idle;
		}
		if (isMoving) 
		{
			if(facingRight)
			{
				body.velocity = new Vector2 (1.0f, body.velocity.y);
			}
			else
			{
				body.velocity = new Vector2 (-1.0f, body.velocity.y);
			}
			if(transform.position.x <= (startX - gridSize))
			{
				isMoving = false;
				Statics.isWalking = false;
				animator.SetBool("Walking", false);
			}
			else if(transform.position.x >= (startX + gridSize))
			{
				isMoving = false;
				Statics.isWalking = false;
				animator.SetBool("Walking", false);
			}
		}   
    }

	private void AttackUpdate()
	{
		if (state == State.attack) {
			isAttacking = true;
			FireBullet();
			animator.SetBool ("Attacking", true);
			state = State.idle;
		}
		if (isAttacking) {
			attackCounter++;
			if(attackCounter == attackCounterMax)
			{
				animator.SetBool("Attacking", false);
				isAttacking = false;
				attackCounter = 0;
			}
		}
	}

	private void JumpUpdate()
	{
		// If the player should jump...
		if (jump)
		{
			isJumping = true;
			jump = false;
			// Add a vertical force to the player.
			animator.SetBool("Jumping", true);
			body.AddForce(new Vector2(0.0f, jumpForce));
		}
		if (isJumping) {
			if(body.velocity.y <= 0.0f)
			{
				isJumping = false;
				animator.SetBool("Jumping", false);
			}
		}
	}

    public void Flip()
    {
        // Switch the way the player is labelled as facing.
        facingRight = !facingRight;

        // Multiply the player's x local scale by -1.
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}