using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.UI;
using System.Drawing;

public class PredictionManager : MonoBehaviour
{
    public Text currentDateText;

    public GameObject earth;
    public GameObject borders;

    public PeriodManager periodManager;

    [SerializeField]
    private float secondsForDay = 1.0f;

    private int daysElapsed;
    private Coroutine predictionCoroutine;

    private Dictionary<string, float> countryPredictionAreas = new Dictionary<string, float>();
    private float maxValue;

    public GameObject noDataPanel;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(borders.transform.childCount);
    }

    public void PredictForDay()
    {
        periodManager.UpdateDateUI();
        Debug.Log("Prediction for day started...");
        ChangeBorderColors();
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
        string fileName = formattedDate;

        return fileName;
    }
    // Load country prediction areas from CSV file
    // Load country prediction areas from CSV file
    private void LoadDataFromCSV()
    {
        string csvFileName = GetFileName(); // Get CSV file name
        TextAsset csvFile = Resources.Load<TextAsset>(csvFileName); // Load CSV file as TextAsset

        if (csvFile != null)
        {
            Debug.Log("CSV file loaded successfully.");

            // Split the text of the CSV file into lines
            string[] lines = csvFile.text.Split('\n');

            // Iterate through each line starting from the second line (skipping the header)
            for (int i = 1; i < lines.Length; i++)
            {
                string[] columns = lines[i].Split(',');
                if (columns.Length >= 2)
                {
                    string countryName = columns[0];
                    float predictionArea;
                    if (float.TryParse(columns[1], out predictionArea))
                    {
                        countryPredictionAreas[countryName] = predictionArea;
                    }
                }
            }
        }
        else
        {
            // Show error message if CSV file is not found
            noDataPanel.GetComponent<PanelAnimation>().ShowPanel();
            noDataPanel.GetComponentInChildren<Text>().text = "No data for " + periodManager.GetFormattedDate();

            Debug.LogError("CSV file not found: " + csvFileName);
        }
    }

    // Change border colors based on data from CSV file
    private void ChangeBorderColors()
    {
        // Load data from CSV file for the current period
        LoadDataFromCSV();
        maxValue = GetMaxValue();
        float predictionArea;
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
}
