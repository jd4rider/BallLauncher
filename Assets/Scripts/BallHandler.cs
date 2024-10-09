using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{

    [SerializeField] private GameObject ballPref;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float detachDelay = 0.15f;
    [SerializeField] private float respawnDelay = 1f;
    
    private Rigidbody2D ballPrefab;
    private SpringJoint2D hook;
    private Camera mainCamera;

    private bool isDragging;

    private void Start()
    {
        mainCamera = Camera.main;

        SpawnNewBall();
    }
    private void Update()
    {
        if(ballPrefab == null){
            return;
        }
        if(!Touchscreen.current.primaryTouch.press.isPressed){
            if(isDragging){
                LaunchBall();
            }
            isDragging = false;
            return;
        }
        isDragging = true;
        ballPrefab.bodyType = RigidbodyType2D.Kinematic;

        Vector2 touchpos = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(touchpos);

        ballPrefab.position = worldPos;

    }

    private void SpawnNewBall() {
        GameObject ballInstance = Instantiate(ballPref, pivot.position, Quaternion.identity);

        ballPrefab = ballInstance.GetComponent<Rigidbody2D>();
        hook = ballInstance.GetComponent<SpringJoint2D>();

        hook.connectedBody = pivot;
    }

    private void LaunchBall()
    {
        ballPrefab.bodyType = RigidbodyType2D.Dynamic;
        ballPrefab = null;

        Invoke(nameof(DetachBall), detachDelay);
        Invoke(nameof(SpawnNewBall), respawnDelay);

    }

    private void DetachBall()
    {
        hook.enabled = false;
        hook = null;
    }
}
