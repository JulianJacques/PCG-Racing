using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarInputHandler : MonoBehaviour
{
    //Components
    CarController topDownCarController;

    [SerializeField] private ControlType controlType;
    
    public enum ControlType
    {
        PlayerInput,
        SmartAI,
        RandomAI
    }

    private float offsetForHor;
    private float offsetForVer;

    //Awake is called when the script instance is being loaded.
    void Awake()
    {
        topDownCarController = GetComponent<CarController>();
        offsetForHor = Random.Range(0.0f, 10000.0f);
        offsetForVer = Random.Range(0.0f, 10000.0f);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame and is frame dependent
    void Update()
    {
        switch (controlType)
        {
            case ControlType.PlayerInput: PlayerInputUpdate(); break;
            case ControlType.RandomAI: RandomAIUpdate(); break;
            case ControlType.SmartAI: SmartAIUpdate(); break;
        }
    }

    void PlayerInputUpdate()
    {
        Vector2 inputVector = Vector2.zero;

        //Get input from Unity's input system.
        inputVector.x = Input.GetAxis("Horizontal");
        inputVector.y = Input.GetAxis("Vertical");

        //Send the input to the car controller.
        topDownCarController.SetInputVector(inputVector);
    }
    
    void RandomAIUpdate()
    {
        Vector2 inputVector = Vector2.zero;

        //Get input from Unity's input system.
        inputVector.x = Mathf.Lerp(-1.0f,1.0f,-Mathf.PerlinNoise(offsetForHor, Time.time));
        inputVector.y = Mathf.Lerp(-1.0f,1.0f,Mathf.PerlinNoise(offsetForVer, Time.time));

        //Send the input to the car controller.
        topDownCarController.SetInputVector(inputVector);
    }
    
    void SmartAIUpdate()
    {
        
    }
}