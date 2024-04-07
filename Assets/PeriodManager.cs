using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PeriodManager : MonoBehaviour
{
    public List<string> years;
    public List<string> months;

    public int currentDay;
    public int currentDayIndex;

    public int currentMonth;
    public int currentMonthIndex;

    public int currentYear;
    public int currentYearIndex;

    public string currentMonthName;
    public string currentYearName;
    public string currentDayName;

    public Text TextDay;
    public Text TextMonth;
    public Text TextYear;
    public Text TextDate;

    public int[] daysInMonth = { 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };

    // Start is called before the first frame update
    void Start()
    {
        years = new List<string>();
        for (int i = 2023; i <= 2024; i++)
        {
            years.Add(i.ToString());
        }

        months = new List<string> {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        };
        currentDayIndex = 28;
        currentMonthIndex = 7;

        UpdateDayUI();
        UpdateMonthUI();
        UpdateYearUI();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int get_year()
    {
        return System.Convert.ToInt32(years[currentYearIndex]);
    }
    public int get_month()
    {
        int tempCurrentMonthIndex = currentMonthIndex + 1;
        return tempCurrentMonthIndex;
    }
    public int get_day()
    {
        return currentDayIndex + 1;
    }
    public void NextDay()
    {
        currentDayIndex = (currentDayIndex + 1) % daysInMonth[currentMonthIndex];
        UpdateDayUI();
    }

    // Function to decrement current month index
    public void PreviousDay()
    {
        currentDayIndex = (currentDayIndex - 1 + daysInMonth[currentMonthIndex]) % daysInMonth[currentMonthIndex];
        
        UpdateDayUI();
    }

    // Function to increment current month index
    public void NextMonth()
    {
        currentMonthIndex = (currentMonthIndex + 1) % 12;
        UpdateMonthUI();
    }

    // Function to decrement current month index
    public void PreviousMonth()
    {
        currentMonthIndex = (currentMonthIndex - 1 + 12) % 12;
        UpdateMonthUI();
    }

    // Function to increment current year index
    public void NextYear()
    {
        currentYearIndex = (currentYearIndex + 1) % years.Count;
        UpdateYearUI();
    }

    // Function to decrement current year index
    public void PreviousYear()
    {
        currentYearIndex = (currentYearIndex - 1 + years.Count) % years.Count;
        UpdateYearUI();
    }

    public void UpdateDayUI()
    {
        currentDayName = System.Convert.ToString(currentDayIndex + 1);
        TextDay.text = currentDayName;
        Debug.Log("Current day is: " + currentDayName);
    }
    public void UpdateMonthUI()
    {
        currentMonthName = months[currentMonthIndex];
        TextMonth.text = currentMonthName;
        Debug.Log("Current month is: " + currentMonthName);
    }

    public void UpdateYearUI()
    {
        currentYearName = years[currentYearIndex];
        TextYear.text = currentYearName;
        Debug.Log("Current year is: " + currentYearName);
    }

    public void UpdateDateUI()
    {
        // Format the date as "dd/MM/yyyy"
        string formattedDate = GetFormattedDate();

        // Update the text of the currentDateText UI element
        TextDate.text = "Date: " + formattedDate;
    }

    public string GetFormattedDate()
    {
        int day = get_day();
        int month = get_month();
        int year = get_year();

        // Format the date as "dd/MM/yyyy"
        string formattedDate = string.Format("{0:00}/{1:00}/{2}", day, month, year);
        return formattedDate;
    }
}
