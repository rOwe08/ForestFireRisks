using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonthManager : MonoBehaviour
{
    public List<string> months;
    public int currentMonthIndex;
    public string currentMonthName;
    public Text TextMonth;

    // Start is called before the first frame update
    void Start()
    {
        months = new List<string> {
            "January", "February", "March", "April", "May", "June",
            "July", "August", "September", "October", "November", "December"
        };
        currentMonthIndex = 0;
        UpdateMonth();
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

    public void UpdateMonth()
    {
        currentMonthName = months[currentMonthIndex];
        TextMonth.text = currentMonthName;
        Debug.Log("Current month is: " + currentMonthName);
    }
}
