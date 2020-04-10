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

    // Private properties.
    private bool isDead = false;
    private bool isPlayingDeathAnimation = false;
    private bool isJumping;
    private int currentIndex;
    private float jumpOffsetX;    
    private float midwayPointX;
    private float initialPosition;
    private float liveMeshSizeScale;
    private float deadMeshSizeScale;
    private Vector3 jumpTargetLocation;

    // Start is called before the first frame update.
    void Start() {
        // Set initial values.
        isDead = false;
        isPlayingDeathAnimation = false;
        isJumping = false;
        currentIndex = -1;
        jumpOffsetX = 1.5f;
        liveMeshSizeScale = 0.7f;
        deadMeshSizeScale = 0.02f;

        initialPosition = transform.position.y;
    }

    // Update is called once per frame.
    void Update() {
        // If player is dead we should stop the game.
        if (isDead) {
            return;
        }

        // Calculate next jump position.
        if (Input.GetMouseButtonDown(0) && !isJumping) {
            calculateNextJumpLocation();
        }

        // Smooth jumping calculation.
        if (isJumping) {
            if (transform.position.x > jumpTargetLocation.x) {
                // Animate towards final x position.
                float newXPosition = transform.position.x - (movingSpeed * Time.deltaTime);
                float newYPosition = newXPosition > midwayPointX ? transform.position.y + jumpHeightIncrement * Time.deltaTime : transform.position.y - jumpHeightIncrement * Time.deltaTime;
                transform.position = new Vector3(newXPosition, Mathf.Max(newYPosition, initialPosition), transform.position.z);
            } else {
                // Jumping animation reached final position. Stop animation.
                isJumping = false;
            }            
        }

        // Death animation.
        if (isPlayingDeathAnimation) {
            UpdateDeathAnimation();
        }
    }

    // Jump logic.
    private void calculateNextJumpLocation() {
        // Set isJumping flag to true so we prevent multiple jumping.
        isJumping = true;

        // Increment current index.
        currentIndex += 1;

        // Check index out of bound.
       if (strips.Count <= currentIndex) {
            print("Index is out of bound. Current selected Index: " + currentIndex);
            isJumping = false; // Reset isJumping flag.
            return;
        }
 
        // Get strip at the current index.
        GameObject nextStrip = strips[currentIndex] as GameObject;

        // Get x position of next strip.
        jumpTargetLocation = new Vector3(nextStrip.transform.position.x - jumpOffsetX, nextStrip.transform.position.y, transform.position.z);
        midwayPointX = jumpTargetLocation.x + ((transform.position.x - jumpTargetLocation.x) * 0.5f);

        // Instantiate new strip right after the last strip.
        SpawnNewStrip();
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
}
