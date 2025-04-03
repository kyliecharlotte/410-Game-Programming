using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private float movementX;
    private int count;
    private int jumps = 0;
    private bool grounded;
    private float movementY;
    public float speed = 0;
    public TextMeshProUGUI countText;

    public GameObject winTextObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        count = 0;
        SetCountText();
        winTextObject.SetActive(false);
        
    }

    private void FixedUpdate() {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);
        rb.AddForce(movement*speed);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            this.Jump();
        }
    }

    private void Jump() {
        Debug.Log("x" + jumps);
        if (grounded == true) {
            Debug.Log(jumps);
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3 (0, 5f, 1f), ForceMode.Impulse);
            grounded = false;
            jumps++;
        } else if (jumps == 1) {
            gameObject.GetComponent<Rigidbody>().AddForce(new Vector3 (0, 5f, 2f), ForceMode.Impulse);
            jumps++;
            return;
        } else {
            jumps = 0;
            return;
        }
    }

    void OnTriggerEnter(Collider other) {
        other.gameObject.SetActive(false);

        if (other.gameObject.CompareTag("PickUp")){
            other.gameObject.SetActive(false);
            count = count + 1;
            SetCountText();
        }

    }

    void OnMove(InputValue movement) {
        Vector2 move_vector = movement.Get<Vector2>();
        movementX = move_vector.x;
        movementY = move_vector.y;

    }

    void SetCountText() {
        countText.text = "Count: " + count.ToString();
        if (count >= 6) {
            Destroy(GameObject.FindGameObjectWithTag("Enemy"));
            winTextObject.SetActive(true);
        }
    }

    private void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Enemy")) {
            Destroy(gameObject);
            winTextObject.gameObject.SetActive(true);
            winTextObject.GetComponent<TextMeshProUGUI>().text = "You Lost :(";
        }
        if (collision.gameObject.CompareTag("Ground")) {
            grounded = true;
            jumps = 0;
        }
    }
}
