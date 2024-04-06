using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PredictionManager : MonoBehaviour
{
    public Text currentDayText;
    public Text yearChangesText;

    public GameObject earth;
    public GameObject borders;

    public PeriodManager periodManager;

    [SerializeField]
    private float secondsForDay = 1.0f;

    private int[] daysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

    private int daysElapsed;
    private Coroutine predictionCoroutine;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Predict()
    {
        Debug.Log("Prediction started...");
        daysElapsed = 0;
        if (predictionCoroutine != null)
        {
            StopCoroutine(predictionCoroutine);
        }
        predictionCoroutine = StartCoroutine(ChangeYearsCounter());
    }

    private void ChangeBorderColors()
    {
        // Loop through all children of the 'borders' GameObject
        foreach (Transform child in borders.transform)
        {
            // Get all MeshRenderers of the current child
            MeshRenderer[] childRenderers = child.GetComponentsInChildren<MeshRenderer>();

            // Change the color of each child's mesh renderers to random colors
            foreach (MeshRenderer renderer in childRenderers)
            {
                Color newColor = GetColor(child.name);
                renderer.material.color = newColor;
            }
        }
    }

    private Color GetColor(string countryName)
    {
        return new Color(Random.value, Random.value, Random.value);
    }
    private float GetValue()
    {
        return Random.Range(-1f, 1f);
    }
    private void UpdateChangesYear()
    {
        float changesValue = GetValue();
        string formattedValue = changesValue.ToString("F2");
        yearChangesText.text = formattedValue;

        if(changesValue < 0)
        {
            yearChangesText.color = Color.blue;
        }
        else
        {
            yearChangesText.color = Color.red;
        }
    }


    IEnumerator ChangeYearsCounter()
    {

        for (int i = 0; i < daysInMonth[periodManager.currentMonthIndex]; i++)
        {
            daysElapsed++;
            currentDayText.text = "Days: " + daysElapsed;

            UpdateChangesYear();
            ChangeBorderColors();

            yield return new WaitForSeconds(secondsForDay);
        }

        predictionCoroutine = null;
    }
}
