using UnityEngine;

public class CameraFollowTarget : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    [SerializeField] private Vector3 directionFromTargetToCamera;
    [SerializeField] private float distanceBetweenTargetAndCamera;
    [SerializeField] private float smoothedOutLerp;
    private Camera sceneMainCamera;

    private Vector3 targetToCamera;
    private Vector3 targetToCameraOffset;

    private void Awake()
    {
        directionFromTargetToCamera.Normalize();
        targetToCamera = distanceBetweenTargetAndCamera * directionFromTargetToCamera;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        // Do once a rotation for camera to look at camera without doing "LookAt"

        sceneMainCamera.transform.localRotation =
            Quaternion.Euler(distanceBetweenTargetAndCamera * Mathf.Acos(directionFromTargetToCamera.z), 0f, 0f);
        targetToCameraOffset = Vector3.zero;
    }

    // Update is called once per frame
    private void Update()
    {
        Vector3 desiredCameraPosition = targetTransform.position + targetToCamera + targetToCameraOffset;
        Vector3 smoothedCameraPosition =
            Vector3.Lerp(sceneMainCamera.transform.position, desiredCameraPosition, smoothedOutLerp);
        Vector3 toTranslate = smoothedCameraPosition - sceneMainCamera.transform.position;
        sceneMainCamera.transform.Translate(toTranslate);
    }

    private void OnEnable()
    {
        sceneMainCamera = Camera.main;
    }

    private void OnDisable()
    {
        sceneMainCamera = null;
    }

    public void SetTargetTransformPosition(Vector3 vector3)
    {
        targetToCameraOffset = vector3;
    }

    public void SetTargetTransformPosition(float x, float y, float z)
    {
        SetTargetTransformPosition(new Vector3(x, y, z));
    }

    public Vector3 GetTargetTransformPosition()
    {
        return new Vector3(targetTransform.position.x, targetTransform.position.y, targetTransform.position.z);
    }
}
