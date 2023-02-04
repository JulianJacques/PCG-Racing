using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class ActionList : MonoBehaviour
{
    private List<Action> actionList = new List<Action>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    {
        for (int i = 0; i < actionList.Count; i++)
        {
            Action action = actionList[i];
            if(action.IncrementTime())
                continue;
            
            if (action.ActionUpdate())
            {
                actionList.Remove(action);
                i--;
            }

            if (action.isBlocking())
            {
                break;
            }
        }
    }

    public void ClearAll()
    {
        actionList.Clear();
    }

    public void Add(Action nAction)
    {
        actionList.Add(nAction);
    }

    public bool IsEmpty()
    {
        return  actionList.Count <= 0;
    }
}
