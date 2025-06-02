using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    #region Camera
    [Header("Camera Input")]

    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform cameraPosTransform;

    [SerializeField] private float mouseSensitivity;

    public float rotateHorizontal, rotateVertical, rotateOtherly;
    #endregion

    [Header("Fov")]
    public bool useFluentFov;
    public MovementScript movementScript;
    public Rigidbody rigidBody;
    public Camera cam;

    public float defaultFov;
    public float currentFov;

    public float minMovementSpeed;
    public float maxMovementSpeed;
    public float minFov;
    public float maxFov;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSensitivity;

        rotateVertical += mouseX;

        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSensitivity;

        rotateHorizontal -= mouseY;
        rotateHorizontal = Mathf.Clamp(rotateHorizontal, -90f, 90f);

        transform.rotation = Quaternion.Euler(rotateHorizontal, rotateVertical, rotateOtherly);
        playerTransform.rotation = Quaternion.Euler(0f, rotateVertical, 0f);
        cameraPosTransform.rotation = Quaternion.Euler(rotateHorizontal, rotateVertical, 0f);

        if (useFluentFov) 
        { 
            HandleFov(); 
        }
    }

    private void HandleFov()
    {
        float moveSpeedDif = maxMovementSpeed - minMovementSpeed;
        float fovDif = maxFov - minFov;

        float rbFlatVel = new Vector3(rigidBody.velocity.x, 0f, rigidBody.velocity.z).magnitude;
        float currMoveSpeedOvershoot = rbFlatVel - minMovementSpeed;
        float currMoveSpeedProgress = currMoveSpeedOvershoot / moveSpeedDif;

        float fov = (currMoveSpeedProgress * fovDif) + minFov;

        float currFov = cam.fieldOfView;
        float lerpedFov = Mathf.Lerp(fov, currFov, Time.deltaTime * 200);

        cam.fieldOfView = lerpedFov;
    }

    public void DoFov(float endValue)
    {
        cam.DOFieldOfView(endValue, 0.25f);
    }

    public void DoTilt(float zTilt)
    {
        transform.DOLocalRotate(new Vector3(0, 0, zTilt), 0.25f);
    }
}
