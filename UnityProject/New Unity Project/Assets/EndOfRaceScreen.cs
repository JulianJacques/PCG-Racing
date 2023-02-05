using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class EndOfRaceScreen : MonoBehaviour
{
    public Transform Player;
    public Vector3 StartPosition;

    public List<StarDisplay> Stars = new List<StarDisplay>();
    public TextMeshProUGUI Score;

    public void Awake()
    {
    }

    public void ResetRace()
    {
        ToggleActive();
        Time.timeScale = 1;
        finishSquareBehavior.timer = 0;
        Player.position = StartPosition;
        FindObjectOfType<PCGGen>().sendPathsToAI();
        foreach(CarInputHandler cih in FindObjectsOfType<CarInputHandler>())
        {
            cih.SendToStart();
        }
    }
    public void NewRace()
    {
        ToggleActive();

        finishSquareBehavior.timer = 0;
        Time.timeScale = 1;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ToggleActive()
    {
        transform.GetChild(0).gameObject.SetActive(!transform.GetChild(0).gameObject.active);
        foreach (StarDisplay item in Stars)
        {
            item.Update(finishSquareBehavior.timer);
        }
        Score.text = $"Time: {finishSquareBehavior.timer:0.000}";

    }

}

[System.Serializable]
public class StarDisplay
{
    public float Threshold;
    public Image Display;

    public void Update(float timer)
    {
        if (timer <= Threshold)
            Display.enabled = true;
        else
            Display.enabled = false;
    }
}
