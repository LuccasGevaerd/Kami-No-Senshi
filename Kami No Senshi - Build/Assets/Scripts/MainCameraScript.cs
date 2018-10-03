using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCameraScript : MonoBehaviour {

    public List<Transform> targets;
    public Vector3 offset;
    public float smoothTime = .5f;
    public float shakeTime;
    public float shakeAmount = 0.1f;

    public float minZoom, maxZoom;
    public float zoomLimiter = 50;

    private Vector3 velocity;

    Camera cam;

    private void Start()
    {
        cam = GetComponent<Camera>();
        
    }

    private void LateUpdate()
    {
        if (shakeTime > 0)
        {
            Vector2 ShakePos = Random.insideUnitCircle * shakeAmount;

            transform.position = new Vector3(transform.position.x + ShakePos.x,
                                             transform.position.y + ShakePos.y,
                                             transform.position.z);
            shakeTime -= Time.deltaTime;
        }
        Move();
        Zoom();
    }
    void Move()
    {
        if (targets.Count == 0) return;
        Vector3 centerPoint = GetCenterPoint();
        Vector3 newPosition = centerPoint + offset;
        transform.position = Vector3.SmoothDamp(transform.position,newPosition, ref velocity, smoothTime);
    }
    void Zoom()
    {
        float newZoom = Mathf.Lerp(maxZoom, minZoom, GetCenterX() / zoomLimiter);
        cam.fieldOfView = Mathf.Lerp(cam.fieldOfView, newZoom, Time.deltaTime);
    }
    Vector3 GetCenterPoint()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.center;
    }
    float GetCenterX()
    {
        var bounds = new Bounds(targets[0].position, Vector3.zero);
        for (int i = 0; i < targets.Count; i++)
        {
            bounds.Encapsulate(targets[i].position);
        }
        return bounds.size.x;
    }
    void GetTarget1(Transform obj)
    {
        targets[0] = obj;
    }
    void GetTarget2(Transform obj)
    {
        targets[1] = obj;
    }
    void ShakeCamera(float shakeTime2)
    {
        shakeTime = 0;
        shakeTime = shakeTime2;
    }
}
