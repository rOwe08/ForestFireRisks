using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeriodManager : MonoBehaviour
{
    public List<string> years;
    public List<string> months;

    public int currentMonthIndex;
    public int currentYearIndex;
    public string currentMonthName;
    public string currentYear;
    public Text TextMonth;
    public Text TextYear;

    // Start is called before the first frame update
    void Start()
    {
        years = new List<string>();
        for (int i = 2000; i <= 2024; i++)
        {
            years.Add(i.ToString());
        }

        months = new List<string> {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        };
        currentMonthIndex = 0;
        currentYearIndex = 0;
        UpdateMonth();
        UpdateYear();
    }

    // Update is called once per frame
    void Update()
    {

    }

    // Function to increment current month index
    public void NextMonth()
    {
        currentMonthIndex = (currentMonthIndex + 1) % 12;
        UpdateMonth();
    }

    // Function to decrement current month index
    public void PreviousMonth()
    {
        currentMonthIndex = (currentMonthIndex - 1 + 12) % 12;
        UpdateMonth();
    }

    // Function to increment current year index
    public void NextYear()
    {
        currentYearIndex = (currentYearIndex + 1) % years.Count;
        UpdateYear();
    }

    // Function to decrement current year index
    public void PreviousYear()
    {
        currentYearIndex = (currentYearIndex - 1 + years.Count) % years.Count;
        UpdateYear();
    }

    public void UpdateMonth()
    {
        currentMonthName = months[currentMonthIndex];
        TextMonth.text = currentMonthName;
        Debug.Log("Current month is: " + currentMonthName);
    }

    public void UpdateYear()
    {
        currentYear = years[currentYearIndex];
        TextYear.text = currentYear;
        Debug.Log("Current year is: " + currentYear);
    }
}
