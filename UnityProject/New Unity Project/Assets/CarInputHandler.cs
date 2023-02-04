using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CarInputHandler : MonoBehaviour
{
    //Components
    CarController topDownCarController;

    [SerializeField] private ControlType controlType;
    [SerializeField] private float closeRad;
    [SerializeField] [Range(0.1f,1.0f)]private float turnPower;
    private Queue<Vector3> path = new Queue<Vector3>();
    
    
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

    public void setPath(List<Vector3> newPath)
    {
        foreach (Vector3 pos in newPath)
        {
            path.Enqueue(pos);
        }
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
        //Get the next go to in the path
        Vector3 target = path.Peek();

        //Check if your close enough
        if (isCloseEnough(target))
        {
            //If close enough pop the path
            path.Dequeue();
            target = path.Peek();
        }
        
        Vector2 vectorToTarget = target - transform.position;
        vectorToTarget.Normalize();

        //Calculate an angle towards the target 
        float angleToTarget = Vector2.SignedAngle(transform.up, vectorToTarget);
        angleToTarget *= -1;

        //We want the car to turn as much as possible if the angle is greater than 45 degrees and we wan't it to smooth out so if the angle is small we want the AI to make smaller corrections. 
        float steerAmount = angleToTarget / 45.0f;

        //Clamp steering to between -1 and 1.
        steerAmount = Mathf.Clamp(steerAmount, -1.0f, 1.0f);
        
        Vector2 inputVector = Vector2.zero;

        //Get input from Unity's input system.
        inputVector.x = steerAmount;
        inputVector.y = Mathf.PerlinNoise(offsetForVer, Time.time);

        //Send the input to the car controller.
        topDownCarController.SetInputVector(inputVector);

    }

    bool isCloseEnough(Vector3 target)
    {
        if (Vector3.Distance(transform.position, target) < closeRad)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}