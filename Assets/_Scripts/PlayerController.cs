using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    #region Data Member
    public float walkspeed = 7;
    //float slidespeed = 3;
    public float jumpForce = 600;
    Rigidbody2D rigid;
    Vector3 startingPosition; // If we are too far underwater we will teleport player to starting position.
    public GameObject playerObject;
    public AudioClip[] sounds;
    public AudioSource instrument;
    public bool isDoubleJump = true;
    bool isOnGround = false;


    #endregion
    #region Getter Setter
    #endregion
    #region Built - in Method
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>(); // Get the rigidbody component added to the script and store it in rb

    }
    void Update()
    {
        Walk();
        //jump (press twice to double jump) with sound effect
        Jump();

        if (rigid.velocity.y > 0)
        {
            Debug.Log("pass through");
            //note: ground tiles labelled PassThru are the only ones that can be jumped through with below code
            Physics2D.IgnoreLayerCollision(11, 12, true);
        }
        //if player falls from jumping, player can land on the platform instead of falling through
        else
        {
            Physics2D.IgnoreLayerCollision(11, 12, false);
        }

        //slide (duck down and move left or right simultaneously) with momentum


        //walk with sound effect
        if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            playsound(0);
        }

        // check if your character fell off the platform
        if( transform.position.y < -20) {
            Vector3 temp = transform.position; // copy to an auxiliary variable...
            temp.x = -1.64f; // modify the component you want in the variable...
            temp.y = 1.04f; 
            transform.position = temp; // and save the modified value
            // transform.position.x = -1.64f; // set the x axis that the player will spawn to
            // transform.position.y = 1.04f; //set the y axis that the player will spawn to
            // transform.position.z = 0; // set the z axis that the player will spawn to
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Platform") || collision.gameObject.CompareTag("PassThru"))
        {
            isOnGround = true;
        }

        //when player lands on a platform designating the end of a level
        if (collision.gameObject.CompareTag("LevelEnd"))
        {
            //show new sprite "Level Complete" text on screen
            LevelManager._instance.ReachDestination(collision.gameObject.name); //just need tagged name of the gameObject
            isOnGround = true;
            Debug.Log("You reached the end of the level");
            playsound(2);
        }
    }
    #endregion
    #region Public Method
    #endregion
    #region Private Method
    private void Walk()
    {
        float movement = Input.GetAxis("Horizontal") * walkspeed;
        rigid.velocity = new Vector3(movement, rigid.velocity.y, 0);

    }
    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (isOnGround)
            {
                isOnGround = false;
                rigid.AddForce(new Vector3(0, jumpForce, 0)); // Adds 100 force straight up, might need tweaking on that number
                playsound(1);
            }
            {
                if (isDoubleJump)
                {
                    rigid.AddForce(new Vector3(0, jumpForce, 0)); // Adds 100 force straight up, might need tweaking on that number
                    playsound(1);
                }
            }
        }
    }

    private void playsound(int index)
    {
        instrument.clip = sounds[index];
        instrument.Play();
    }
    #endregion







}