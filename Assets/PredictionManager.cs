using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using System.Drawing;

public class PredictionManager : MonoBehaviour
{
    public Text currentDayText;
    public Text yearChangesText;

    public GameObject earth;
    public GameObject borders;

    public PeriodManager periodManager;

    [SerializeField]
    private float secondsForDay = 1.0f;

    private int daysElapsed;
    private Coroutine predictionCoroutine;

    private Dictionary<string, float> countryPredictionAreas = new Dictionary<string, float>();
    private float maxValue;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(borders.transform.childCount);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Predict()
    {
        Debug.Log("Prediction started...");
        ChangeBorderColors();

        //daysElapsed = 0;
        //if (predictionCoroutine != null)
        //{
        //    StopCoroutine(predictionCoroutine);
        //}

        //predictionCoroutine = StartCoroutine(ChangePeriodCounter());
    }

    private string GetFileName()
    {
        // Get the current date components
        int day = periodManager.get_day();
        int month = periodManager.get_month();
        int year = periodManager.get_year();

        // Create a DateTime object with the current date components
        DateTime currentDate = new DateTime(year, month, day);

        // Format the date as "yyyy-MM-dd"
        string formattedDate = currentDate.ToString("yyyy-MM-dd");

        // Append ".csv" to the formatted date
        string fileName = formattedDate + ".csv";

        return fileName;
    }

    // Load country prediction areas from CSV file
    // Load country prediction areas from CSV file
    private void LoadDataFromCSV()
    {
        string csvFileName = GetFileName(); // Get CSV file name
        string csvFilePath = Path.Combine(Application.dataPath, "data", csvFileName); // Get CSV file path

        // Check if CSV file exists
        if (File.Exists(csvFilePath))
        {
            // Read all lines from the CSV file
            string[] lines = File.ReadAllLines(csvFilePath);

            // Skip the first line (header row) and start parsing from the second line
            for (int i = 1; i < lines.Length; i++)
            {
                // Split the line into columns using the comma as the delimiter
                string[] columns = lines[i].Split(',');

                // Check if the line has the expected number of columns
                if (columns.Length >= 2)
                {
                    // Parse country name
                    string countryName = columns[0];

                    // Parse prediction area value
                    float predictionArea;
                    if (float.TryParse(columns[1], out predictionArea))
                    {
                        // Add country prediction area to dictionary
                        countryPredictionAreas[countryName] = predictionArea;
                    }
                }
            }
        }
        else
        {
            Debug.LogError("CSV file not found: " + csvFilePath);
        }
    }


    // Change border colors based on data from CSV file
    // Change border colors based on data from CSV file
    private void ChangeBorderColors()
    {
        // Load data from CSV file for the current period
        LoadDataFromCSV();
        maxValue = GetMaxValue();
        float predictionArea = 0f;
        UnityEngine.Color boarderColor;

        // Loop through all children of the 'borders' GameObject
        foreach (Transform child in borders.transform)
        {
            // Get country name without quotation marks
            string countryName = child.name.Trim('"'); // Remove quotation marks

            // Check if country prediction area exists in dictionary
            if (countryPredictionAreas.ContainsKey(countryName))
            {
                // Get prediction area for country
                predictionArea = countryPredictionAreas[countryName];
                boarderColor = GetColor(predictionArea);

                // Get all MeshRenderers of the current child
                MeshRenderer[] childRenderers = child.GetComponentsInChildren<MeshRenderer>();

                // Change the color of each child's mesh renderers based on prediction area
                foreach (MeshRenderer renderer in childRenderers)
                {
                    renderer.material.color = GetColor(predictionArea);
                }
            }
            else
            {
                boarderColor = UnityEngine.Color.black;

                // Get all MeshRenderers of the current child
                MeshRenderer[] childRenderers = child.GetComponentsInChildren<MeshRenderer>();

                // Change the color of each child's mesh renderers based on prediction area
                foreach (MeshRenderer renderer in childRenderers)
                {
                    renderer.material.color = boarderColor;
                }
            }
        }
    }

    //private float GetMinValue()
    //{
    //    // Initialize the minimum value as the largest possible float value
    //    float minValue = float.MaxValue;

    //    // Iterate through the values in the dictionary
    //    foreach (float value in countryPredictionAreas.Values)
    //    {
    //        // Update the minimum value if the current value is smaller
    //        if (value < minValue)
    //        {
    //            minValue = value;
    //        }
    //    }

    //    return minValue;
    //}

    private float GetMaxValue()
    {
        // Initialize the maximum value as the smallest possible float value
        float maxValue = float.MinValue;

        // Iterate through the values in the dictionary
        foreach (float value in countryPredictionAreas.Values)
        {
            // Update the maximum value if the current value is greater
            if (value > maxValue)
            {
                maxValue = value;
            }
        }

        return maxValue;
    }

    // Get color based on prediction area value
    private UnityEngine.Color GetColor(float predictionArea)
    {
        // Calculate the normalized value between 0 and 1 based on the logarithm
        float normalizedValue = Mathf.Log10(predictionArea + 1) / Mathf.Log10(maxValue + 1);

        // Interpolate between green and red based on the normalized value
        return UnityEngine.Color.Lerp(UnityEngine.Color.green, UnityEngine.Color.red, normalizedValue);
    }

    IEnumerator ChangePeriodCounter()
    {
        for (int i = 0; i < periodManager.daysInMonth[periodManager.currentMonthIndex]; i++)
        {
            daysElapsed++;
            currentDayText.text = "Days: " + daysElapsed;

            ChangeBorderColors();

            yield return new WaitForSeconds(secondsForDay);
        }

        predictionCoroutine = null;
    }
}
