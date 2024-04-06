using UnityEngine;

public class RotateBySeconds : MonoBehaviour
{
    // The speed of rotation in degrees per second
    public float rotationSpeed = 45.0f;

    // Update is called once per frame
    void Update()
    {
        // Calculate the rotation amount based on time
        float rotationAmount = rotationSpeed * Time.deltaTime;

        // Rotate the GameObject around its up axis
        transform.Rotate(Vector3.up, rotationAmount);
    }
}
