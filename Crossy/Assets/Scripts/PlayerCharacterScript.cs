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
        currentIndex = 0;
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
       /* if (strips.count <= currentIndex) {
            print("Index is out of bound. Current selected Index: " + currentIndex);
            isJumping = false; // Reset isJumping flag.
            return;
        }
        */
        // Get strip at the current index.
        GameObject nextStrip = strips[currentIndex] as GameObject;

        // Get x position of next strip.
        jumpTargetLocation = new Vector3(nextStrip.transform.position.x - jumpOffsetX, nextStrip.transform.position.y, transform.position.z);
        midwayPointX = jumpTargetLocation.x + ((transform.position.x - jumpTargetLocation.x) * 0.5f);
    }
}
