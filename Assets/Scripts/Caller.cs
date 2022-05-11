using System.Collections;
using System.Text;
using UnityEngine;

public class Caller 
{
    public CallData data;
    public bool isBadActor;
    public string number;

    public Caller(CallData data)
    {
        this.data = data;
        isBadActor = Random.value > 0.5;
        number = GenerateRandomNumber();
    }

    public AudioClip RandomRepeatClip()
    {
        return data.repeatLines[Random.Range(0, data.repeatLines.Length)];
    }

    public static string GenerateRandomNumber()
    {
        // UK format: 0123 456 7890
        // Reminder Range() is incluse, exclusive with integers
        StringBuilder builder = new StringBuilder("0");

        builder.Append(Random.Range(100, 1000));
        builder.Append(" ");
        builder.Append(Random.Range(100, 1000));
        builder.Append(" ");
        builder.Append(Random.Range(1000, 10000));

        return builder.ToString();
    }
}
