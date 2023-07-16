using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField]
    Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;
    public bool canLook = true;
    Vector2 velocity;
    Vector2 frameVelocity;
    private readonly Vector2 cameraRotationOffset = new Vector2(-60, 30);


    void Reset()
    {
        // Get the character from the FirstPersonMovement in parents.
        character = GetComponentInParent<FirstPersonMovement>().transform;
    }

    void Start()
    {
        // Lock the mouse cursor to the game screen.
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        if(canLook){
            // Get smooth velocity.
            Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
            Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
            frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
            velocity += frameVelocity;
            velocity.y = Mathf.Clamp(velocity.y, -90, 90);

            // Rotate camera up-down and controller left-right from velocity.
            transform.localRotation = Quaternion.AngleAxis(-velocity.y + cameraRotationOffset.y, Vector3.right);
            character.localRotation = Quaternion.AngleAxis(velocity.x + cameraRotationOffset.x, Vector3.up);
        }
    }
    public void LockCamera() {
        canLook = false;
    }
    public void UnlockCamera() {
        canLook = true;
    }
}
