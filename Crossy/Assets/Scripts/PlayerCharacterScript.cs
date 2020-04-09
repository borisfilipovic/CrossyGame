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

    // Private properties.
    private bool isJumping;
    private int currentIndex;
    private float jumpOffsetX;    
    private float midwayPointX;
    private float initialPosition;
    private Vector3 jumpTargetLocation;

    // Start is called before the first frame update.
    void Start() {
        // Set initial values.
        isJumping = false;
        currentIndex = -1;
        jumpOffsetX = 1.5f;
        initialPosition = transform.position.y;
    }

    // Update is called once per frame.
    void Update() {
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
}
