using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float cameraMoveSpeed = 0.3f;
    public Transform player;
    private Vector3 currentTargetPosition;
    private int currentPositionIndex = 0; 
    public List<Vector3> locations;
    float cameraWidth;
    private Camera mainCamera;
    public GameObject turnon;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
        locations = new List<Vector3>();
        locations.Add(new Vector3(-22f, 2.2f, -10f));
        locations.Add(new Vector3(-7.04f, 2.56f, -10f));
        locations.Add(new Vector3(9f, -3f, -10f));
        locations.Add(new Vector3(30.4f, -6.58f, -10f));
        locations.Add(new Vector3(56f, -6.58f, -10f));
        cameraWidth = mainCamera.orthographicSize * mainCamera.aspect;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentPositionIndex == 1) { GetComponent<Camera>().orthographicSize = 5; cameraWidth = mainCamera.orthographicSize * mainCamera.aspect; }
        if (currentPositionIndex == 2) { GetComponent<Camera>().orthographicSize = 6; cameraWidth = mainCamera.orthographicSize * mainCamera.aspect; }
        if (currentPositionIndex == 3) { GetComponent<Camera>().orthographicSize = 6; cameraWidth = mainCamera.orthographicSize * mainCamera.aspect; }
        if (currentPositionIndex == 4) 
        { 
            GetComponent<Camera>().orthographicSize = 9; 
            cameraWidth = mainCamera.orthographicSize * mainCamera.aspect;
            for (int i = 0; i < turnon.transform.childCount; i++)
            {
                turnon.transform.GetChild(i).gameObject.SetActive(true);
            }
        }
        float cameraRightEdge = mainCamera.transform.position.x + cameraWidth;
        float cameraLeftEdge = mainCamera.transform.position.x - cameraWidth;

        if (player.position.x > cameraRightEdge)
        {
            // Player has crossed the trigger on the right edge.
            currentPositionIndex++;
        }
        else if (player.position.x < cameraLeftEdge && currentPositionIndex > 0)
        {
            // Player has crossed the trigger on the left edge and is allowed to go back to the previous location.
            currentPositionIndex--;
        }

        // Update the target position based on the current index.
        currentTargetPosition = locations[currentPositionIndex];

        // Move the camera towards the target position smoothly.
        transform.position = Vector3.Lerp(transform.position, currentTargetPosition, Time.deltaTime * cameraMoveSpeed);
    }
}
