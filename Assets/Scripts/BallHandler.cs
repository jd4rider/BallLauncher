using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallHandler : MonoBehaviour
{

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Rigidbody2D pivot;
    [SerializeField] private float detachDelay = 0.15f;
    [SerializeField] private float respawnDelay = 1f;
    
    private Rigidbody2D ball;
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
        if(ball == null){
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
        ball.bodyType = RigidbodyType2D.Kinematic;

        Vector2 touchpos = Touchscreen.current.primaryTouch.position.ReadValue();

        Vector3 worldPos = mainCamera.ScreenToWorldPoint(touchpos);

        ball.position = worldPos;

    }

    private void SpawnNewBall() {
        GameObject ballInstance = Instantiate(ballPrefab, pivot.position, Quaternion.identity);

        ball = ballInstance.GetComponent<Rigidbody2D>();
        hook = ballInstance.GetComponent<SpringJoint2D>();

        hook.connectedBody = pivot;
    }

    private void LaunchBall()
    {
        ball.bodyType = RigidbodyType2D.Dynamic;
        ball = null;

        Invoke(nameof(DetachBall), detachDelay);
        Invoke(nameof(SpawnNewBall), respawnDelay);

    }

    private void DetachBall()
    {
        hook.enabled = false;
        hook = null;
    }
}
