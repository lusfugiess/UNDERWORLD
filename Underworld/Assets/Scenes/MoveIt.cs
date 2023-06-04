using UnityEngine;

public class MoveIt : MonoBehaviour
{
    public Transform startMarker;  // Start position marker
    public Transform endMarker;    // End position marker
    public float speed = 1.0f;     // Speed of interpolation

    private float startTime;       // Time when the movement started
    private float journeyLength;   // Total distance between the markers

    private void Start()
    {
        // Calculate the total distance between the markers
        journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
        
        // Start the movement
        startTime = Time.time;
    }

    private void Update()
    {
        // Calculate the fraction of the journey completed
        float distanceCovered = (Time.time - startTime) * speed;
        float fractionOfJourney = distanceCovered / journeyLength;

        // Lerp the object's position between the start and end markers
        transform.position = Vector3.Lerp(startMarker.position, endMarker.position, fractionOfJourney);
    }
}
