using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    private int daysElapsed;
    private Coroutine predictionCoroutine;

    private Dictionary<string, float> countryPredictionAreas = new Dictionary<string, float>();

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
        daysElapsed = 0;
        if (predictionCoroutine != null)
        {
            StopCoroutine(predictionCoroutine);
        }

        predictionCoroutine = StartCoroutine(ChangePeriodCounter());
    }

    private string GetFileName()
    {
        // Get the current date components
        int day = periodManager.currentDay;
        int month = periodManager.currentMonth;
        int year = periodManager.currentYear;

        // Create a DateTime object with the current date components
        DateTime currentDate = new DateTime(year, month, day);

        // Format the date as "yyyy-MM-dd"
        string formattedDate = currentDate.ToString("yyyy-MM-dd");

        // Append ".csv" to the formatted date
        string fileName = formattedDate + ".csv";

        return fileName;
    }

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

            // Parse each line of the CSV file
            foreach (string line in lines)
            {
                // Split the line into columns using the comma as the delimiter
                string[] columns = line.Split(',');

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
    private void ChangeBorderColors()
    {
        // Load data from CSV file for the current period
        LoadDataFromCSV();

        // Loop through all children of the 'borders' GameObject
        foreach (Transform child in borders.transform)
        {
            // Get country name
            string countryName = child.name;

            // Check if country prediction area exists in dictionary
            if (countryPredictionAreas.ContainsKey(countryName))
            {
                // Get prediction area for country
                float predictionArea = countryPredictionAreas[countryName];

                // Get all MeshRenderers of the current child
                MeshRenderer[] childRenderers = child.GetComponentsInChildren<MeshRenderer>();

                // Change the color of each child's mesh renderers based on prediction area
                foreach (MeshRenderer renderer in childRenderers)
                {
                    renderer.material.color = GetColor(predictionArea);
                }
            }
        }
    }

    // Get color based on prediction area value
    private Color GetColor(float predictionArea)
    {
        // Example: Implement your logic to map prediction area values to colors here
        // For simplicity, we'll return a random color
        return new Color(0f, 0f, 0f);
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
