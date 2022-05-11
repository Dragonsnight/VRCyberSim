using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public CallData[] callerData;

    public Queue<Caller> callers = new Queue<Caller>();
    public PhoneBehaviour phone;

    public TextMeshPro displayList;
    public GameObject gameOverScreen;
    public GameObject gameWinScreen;

    public static LevelManager instance;
    private Caller currentCaller;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);

        instance = this;
    }

    private void Start()
    {
        UpdateCallerList();
        TryContinue();
    }

    private void UpdateCallerList()
    {
        callers.Clear();

        StringBuilder builder = new StringBuilder();

        foreach (CallData data in callerData.OrderBy(v => Random.value))
            callers.Enqueue(new Caller(data));

        foreach (Caller caller in callers.OrderBy(v => Random.value))
        {
            builder.Append(caller.data.callerName);
            builder.Append(" | ");

            // Fake the caller number if the caller is a bad actor
            if (caller.isBadActor)
                builder.Append(Caller.GenerateRandomNumber());
            else
                builder.Append(caller.number);

            builder.Append("\n");
        }

        displayList.SetText(builder.ToString());

    }

    public void SubmitGuess(bool wasBadActor)
    {
        if (currentCaller.isBadActor == wasBadActor)
        {
            TryContinue();
        }
        else
        {
            callers.Clear();
            gameOverScreen.SetActive(true);
        }



    }

    public void TryContinue()
    {
        if (callers.Count == 0)
        {
            gameWinScreen.SetActive(true);
            return;
        }

        StartCoroutine(Utility.Defer(8, StartNewCall));
    }
    private void StartNewCall()
    {
        currentCaller = callers.Dequeue();
        phone.StartCall(currentCaller);
    }

    public void Restart()
    {
        gameWinScreen.SetActive(false);
        gameOverScreen.SetActive(false);
        UpdateCallerList();
        TryContinue();
    }

    public void Quit()
    {
        Application.Quit();
    }

}
