using UnityEngine;
using UnityEngine.UI;

public class BarTimeLeft : MonoBehaviour
{
    public Image filled;
    void FixedUpdate()
    {
        if (ManagerQuizGame.instance.TimerOnGoing.Value == true)
        {
            float t = ManagerQuizGame.instance.currentTime.Value / ManagerQuizGame.instance.maxTimePerRound.Value;
            filled.fillAmount = t;

            // Orange → Rouge → Noir
            if (t < 0.5f)
            {
                // Orange vers Rouge
                filled.color = Color.Lerp(new Color(1f, 0.5f, 0f), Color.red, t / 0.5f);
            }
            else
            {
                // Rouge vers Noir
                filled.color = Color.Lerp(Color.red, new Color(0.4f, 0f, 0f), (t - 0.5f) / 0.5f);
            }
        }
        else
        {
            //filled.fillAmount = 0;
            //filled.color = new Color(1f, 0.5f, 0f); // Reset en orange
        }
    }
}
