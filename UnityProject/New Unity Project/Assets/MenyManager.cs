using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class MenyManager : MonoBehaviour
{
    public string[] Titles;


    public TextMeshProUGUI Title;

    // Start is called before the first frame update
    void Start()
    {
        Title.text = Titles[Random.Range(0, Titles.Length)];
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
