using UnityEngine;
using UnityEngine.UI;

public class RotationManager : MonoBehaviour
{
    // The speed of rotation in degrees per second
    public float rotationSpeed = 45.0f;
    public bool IsRotating;
    public GameObject RotatingObject;
    public Slider speedSlider;

    private void Start()
    {
        UpdateSpeed();
    }
    // Update is called once per frame
    void Update()
    {
        if (IsRotating)
        {
            // Calculate the rotation amount based on time
            float rotationAmount = rotationSpeed * Time.deltaTime;

            // Rotate the GameObject around its up axis
            RotatingObject.transform.Rotate(Vector3.up, rotationAmount);
        }
    }

    public void RotatingEnable()
    {
        IsRotating = !IsRotating;
    }

    public void UpdateSpeed()
    {
        rotationSpeed = speedSlider.value;
    }
}
