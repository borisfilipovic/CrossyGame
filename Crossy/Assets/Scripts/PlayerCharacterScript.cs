using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterScript : MonoBehaviour {

    // Public properties.
    public GameObject strip1;
    public GameObject strip2;
    public GameObject strip3;
    public GameObject strip4;
    public GameObject strip5;
    public GameObject strip6;
    public GameObject strip7;
    public GameObject strip8;
    public GameObject strip9;
    public GameObject strip10;
    public GameObject strip11;
    public GameObject strip12;
    public GameObject strip13;
    public GameObject strip14;
    public GameObject strip15;
    public List<GameObject> strips;
    public float movingSpeed = 10.0f;
    public float jumpHeightIncrement = 0.0f;
    public GameObject[] poolOfStripsPrefabs;
    public GameObject mesh;
    public GameObject bounaryLeft;
    public GameObject bounaryRight;

    // Private properties.
    private bool isDead = false;
    private bool isPlayingDeathAnimation = false;
    private bool isJumpingUp;
    private bool isJumpingDown;
    private bool isJumpingLeft;
    private bool isJumpingRight;
    private int currentIndex;
    private float jumpOffsetX;
    private float jumpOffsetZ;
    private float midwayPointX;
    private float initialPosition;
    private float initialZPosition;
    private float liveMeshSizeScale;
    private float deadMeshSizeScale;
    private Vector3 jumpTargetLocation;
    private enum Direction { up, down, left, right};

    // Start is called before the first frame update.
    void Start() {
        // Set initial values.
        isDead = false;
        isPlayingDeathAnimation = false;
        isJumpingUp = false;
        isJumpingDown = false;
        isJumpingLeft = false;
        isJumpingRight = false;
        currentIndex = -1;
        jumpOffsetX = 1.5f;
        jumpOffsetZ = 7.0f;
        liveMeshSizeScale = 0.7f;
        deadMeshSizeScale = 0.02f;
        initialPosition = transform.position.y;
        initialZPosition = transform.position.z;
    }

    // Update is called once per frame.
    void Update() {
        // If player is dead we should stop the game.
        if (isDead) {
            return;
        }

        // Calculate next jump position.
        /*if (Input.GetMouseButtonDown(0) && !isJumpingUp) {
            JumpUp();
        }*/

        // Smooth jumping calculation.
        if (isJumpingUp)
        {
            if (transform.position.x > jumpTargetLocation.x)
            {
                // Animate towards final x position.
                float newXPosition = transform.position.x - (movingSpeed * Time.deltaTime);
                float newYPosition = newXPosition > midwayPointX ? transform.position.y + jumpHeightIncrement * Time.deltaTime : transform.position.y - jumpHeightIncrement * Time.deltaTime;
                transform.position = new Vector3(newXPosition, Mathf.Max(newYPosition, initialPosition), transform.position.z);
            }
            else
            {
                // Jumping animation reached final position. Stop animation.
                isJumpingUp = false;
            }
        } else if (isJumpingDown) {
            if (transform.position.x < jumpTargetLocation.x)
            {
                // Animate towards final x position.
                float newXPosition = transform.position.x + (movingSpeed * Time.deltaTime);
                float newYPosition = newXPosition < midwayPointX ? transform.position.y + jumpHeightIncrement * Time.deltaTime : transform.position.y - jumpHeightIncrement * Time.deltaTime;
                transform.position = new Vector3(newXPosition, Mathf.Max(newYPosition, initialPosition), transform.position.z);
            }
            else
            {
                // Jumping animation reached final position. Stop animation.
                isJumpingDown = false;
            }
        } else if (isJumpingLeft) {
            if (transform.position.z > jumpTargetLocation.z)
            {
                // Animate towards final x position.
                float newZPosition = transform.position.z - (movingSpeed * Time.deltaTime);
                float newYPosition = newZPosition > midwayPointX ? transform.position.y + jumpHeightIncrement * Time.deltaTime : transform.position.y - jumpHeightIncrement * Time.deltaTime;     
                transform.position = new Vector3(transform.position.x, Mathf.Max(newYPosition, initialPosition), newZPosition);
            }
            else
            {
                // Jumping animation reached final position. Stop animation.
                isJumpingLeft = false;
            }
        }
        else if (isJumpingRight) {
            if (transform.position.z < jumpTargetLocation.z)
            {
                // Animate towards final x position.
                float newZPosition = transform.position.z + (movingSpeed * Time.deltaTime);
                float newYPosition = newZPosition < midwayPointX ? transform.position.y + jumpHeightIncrement * Time.deltaTime : transform.position.y - jumpHeightIncrement * Time.deltaTime;
                transform.position = new Vector3(transform.position.x, Mathf.Max(newYPosition, initialPosition), newZPosition);
            }
            else
            {
                // Jumping animation reached final position. Stop animation.
                isJumpingRight = false;
            }
        }

        // Death animation.
        if (isPlayingDeathAnimation) {
            UpdateDeathAnimation();
        }
    }

    // Instantiate new strip.
    private void SpawnNewStrip() {
        // Create random number.
        int stripsPrefabCount = poolOfStripsPrefabs.Length;
        int randomNumber = Random.Range(0, stripsPrefabCount);

        // Create new strip.
        GameObject item = poolOfStripsPrefabs[randomNumber] as GameObject; // Randomly select type of new strip.
        GameObject lastStrip = strips[strips.Count - 1] as GameObject; // Get last strip so we can calculate location for new strip.
        GameObject newStrip = Instantiate(item, lastStrip.transform.position, lastStrip.transform.rotation); // Instantiate new strip.

        // Set new strip position.
        Transform itemChildTransform = item.transform.GetChild(0) as Transform;
        Transform itemChildOfChildTransform = itemChildTransform.GetChild(0) as Transform;
        float itemWidth = itemChildOfChildTransform.gameObject.GetComponent<Renderer>().bounds.size.x; // Use renderer because it has size properties.
        newStrip.transform.position = new Vector3(newStrip.transform.position.x - itemWidth, newStrip.transform.position.y, newStrip.transform.position.z); // 

        // Add new strip.
        strips.Add(newStrip);
    }

    // Check collisions between player and car. This is default Unity method that is called on collision trigger. Enemy must have Rigidbody component attached. Enemy must also have BodCollider with isTrigger checked.
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Enemy") {
            DeathAnimation();
        }
    }

    // Death animation.
    void DeathAnimation() {
        // Set is playing death animation to true, so whit method will not be called all the time during animation.
        isPlayingDeathAnimation = true;
    }

    void UpdateDeathAnimation() {
        // Animation goal: a. Scale it down, b. Rotate character.
        // A. Scale it down.
        if (mesh.transform.localScale.z > deadMeshSizeScale) {
            mesh.transform.localScale -= new Vector3(0.0f, 0.0f, deadMeshSizeScale);
        } else {
            isPlayingDeathAnimation = false;
            isDead = true;
        }

        // Rotate character.
        if (mesh.transform.rotation.eulerAngles.x == 0 || mesh.transform.rotation.eulerAngles.x > 270) {
            mesh.transform.Rotate(-4.0f, 0.0f, 0.0f);
        }
    }

    // Swipe gestures.
    void SwipeUp() {
        // Move character up.
        if (!isJumpingUp) {
            isJumpingUp = true;
            jump(Direction.up);
        }
        print("Consuming up");
    }

    void SwipeDown() {
        // Move character down.        
        if (!isJumpingDown)
        {
            isJumpingDown = true;
            jump(Direction.down);
        }        
        print("Consuming down");
    }

    void SwipeLeft() {
        // Move character left.
        if (!isJumpingLeft)
        {
            isJumpingLeft = true;
            jump(Direction.left);
        }        
        print("Consuming left");
    }

    void SwipeRight() {
        // Move character right.
        if (!isJumpingRight)
        {
            isJumpingRight = true;
            jump(Direction.right);
        }        
        print("Consuming right");
    }

    // Jump up or down.
    private void jump(Direction direction)
    {
        float playerYEulerAngleRotation = 0.0f;
        switch (direction)
        {
            case Direction.up:
                // Increment current index.
                currentIndex += 1;

                // Check index out of bound.
                if (strips.Count <= currentIndex)
                {
                    print("Index is out of bound. Current selected Index: " + currentIndex);
                    isJumpingUp = false; // Reset isJumping flag.
                    return;
                }

                // Get strip at the current index.
                GameObject nextStrip = strips[currentIndex] as GameObject;

                /// Calculate jump target location.
                jumpTargetLocation = new Vector3(nextStrip.transform.position.x - jumpOffsetX, nextStrip.transform.position.y, transform.position.z);
                midwayPointX = jumpTargetLocation.x + ((transform.position.x - jumpTargetLocation.x) * 0.5f);

                // Move boundary. We need to calculate the distance that the chicken will travel as it jumps.
                float distanceUpX = transform.position.x - jumpTargetLocation.x;
                bounaryLeft.transform.position -= new Vector3(distanceUpX, 0, 0);
                bounaryRight.transform.position -= new Vector3(distanceUpX, 0, 0);

                // Instantiate new strip right after the last strip.
                SpawnNewStrip();
                break;
            case Direction.down:
                // Decrement current index for one step.
                currentIndex -= 1;

                // Check if we are at the begining already. If so, than just return since playor cannot go back.
                if (currentIndex < 0)
                {
                    currentIndex = 0;
                    return;
                }

                // Get strip at the current index.
                GameObject previousStrip = strips[currentIndex] as GameObject;

                /// Calculate jump target location.
                jumpTargetLocation = new Vector3(previousStrip.transform.position.x - jumpOffsetX, previousStrip.transform.position.y, transform.position.z);
                midwayPointX = jumpTargetLocation.x - ((jumpTargetLocation.x - transform.position.x) * 0.5f);

                // Move boundary. We need to calculate the distance that the chicken will travel as it jumps.
                float distanceDownX = jumpTargetLocation.x - transform.position.x;
                bounaryLeft.transform.position += new Vector3(distanceDownX, 0, 0);
                bounaryRight.transform.position += new Vector3(distanceDownX, 0, 0);

                // Rotate player facing down.
                playerYEulerAngleRotation = 180.0f;                
                break;
            case Direction.left:
                /// Calculate jump target location.
                jumpTargetLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z - jumpOffsetZ);
                midwayPointX = jumpTargetLocation.z - ((jumpTargetLocation.z - transform.position.z) * 0.5f);

                // Rotate player facing left.
                playerYEulerAngleRotation = -90.0f;
                break;
            case Direction.right:
                /// Calculate jump target location.
                jumpTargetLocation = new Vector3(transform.position.x, transform.position.y, transform.position.z + jumpOffsetZ);
                midwayPointX = jumpTargetLocation.z + ((transform.position.z - jumpTargetLocation.z) * 0.5f);

                // Rotate player facing left.
                playerYEulerAngleRotation = 90.0f;
                break;
            default:
                print("Un handeled case.");
                break;
        }

        // Rotate player towards moving direction.
        mesh.transform.localEulerAngles = new Vector3(0.0f, playerYEulerAngleRotation, 0.0f);
    }
}
