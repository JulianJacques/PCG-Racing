using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public enum EaseType
{
    Linear,
    Sin,
    Cos,
    Square,
    Sqrt,
    Slerp
}

public class Action
{
    protected float timer = 0.0f;
    protected float _delay = 0.0f;
    protected float duration { get; set; }
    protected float percentDone;
    protected GameObject parentObject;
    private bool isFirstUpdatebool = true;
    protected bool blocking = false;
    protected EaseType ease = EaseType.Slerp;

    public Action(float pDuration, GameObject pObject, float pDelay = 0.0f, bool pBlock = false)
    {
        duration = pDuration;
        parentObject = pObject;
        _delay = pDelay;
        blocking = pBlock;
    }

    public Action()
    {
        
    }

    public bool IncrementTime()
    {
        if (_delay > 0.0f)
        {
            _delay -= Time.deltaTime;
        }
        else
        {
            timer += Time.deltaTime;
        }

        switch (ease)
        {
            case EaseType.Linear: percentDone = timer / duration;
                break;
            case EaseType.Sin: percentDone = Mathf.Sin(timer / duration);
                break;
            case EaseType.Cos: percentDone = 1 - Mathf.Cos(timer / duration);
                break;
            case EaseType.Square: percentDone = Mathf.Pow(timer / duration,2);
                break;
            case EaseType.Sqrt: percentDone = Mathf.Sqrt(timer / duration);
                break;
            case EaseType.Slerp: percentDone = Mathf.SmoothStep(0.0f,1.0f,timer / duration);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
        

        return _delay > 0.0f;
        
    }

    public virtual bool ActionUpdate()
    {
        return ((percentDone) >= 1.0f);
    }

    public bool isFirstUpdate()
    {
        if (isFirstUpdatebool)
        {
            isFirstUpdatebool = false;
            return true;
        }

        return false;
    }

    public float TimeLeft()
    {
        return duration - timer;
    }

    public bool isBlocking()
    {
        return blocking;
    }

}

public class TranslateUIAction : Action
{
    private RectTransform _rectTransform;
    private Vector2 _startPos;
    private Vector2 _desiredPos;

    public TranslateUIAction(Vector2 desiredPos, float _duration, GameObject parent, float delay = 0.0f) 
        : base(_duration,parent,delay)
    {
        timer = 0.0f;
        _rectTransform = parent.GetComponent<RectTransform>();
        _startPos = _rectTransform.anchoredPosition;
        _desiredPos = desiredPos;
    }
    
    public TranslateUIAction(Vector2 startPos, Vector2 desiredPos, float _duration, GameObject parent, float delay = 0.0f) : base(_duration,parent,delay)
    {
        timer = 0.0f;
        _rectTransform = parent.GetComponent<RectTransform>();
        _startPos = startPos;
        _desiredPos = desiredPos;
    }

    public override bool ActionUpdate()
    {
        _rectTransform.anchoredPosition = Vector2.Lerp(_startPos, _desiredPos, percentDone);
        return base.ActionUpdate();
    }
}

public class RotateUIElementAction : Action
{
    private RectTransform _rectTransform;
    private Vector3 angle;
    private float speed;
    public RotateUIElementAction(Vector3 nangle, float nspeed, float nduration, GameObject parent, float delay = 0.0f) : base(nduration,parent,delay)
    {
        timer = 0.0f;
        _rectTransform = parent.GetComponent<RectTransform>();
        angle = nangle;
        speed = nspeed;
    }
    
    public override bool ActionUpdate()
    {
        _rectTransform.Rotate(angle * speed);
        percentDone = timer / duration;
        return base.ActionUpdate();
    }
}

public class FadeUIAction : Action
{
    private Image _image;
    private Color _desiredColor;
    private Color _startColor;

    public FadeUIAction(Color desiredColor, float _duration, GameObject parent, float delay = 0.0f) : base(_duration,parent,delay)
    {
        _image = parent.GetComponent<Image>();
        _desiredColor = desiredColor;
        _startColor = _image.color;
    }
    
    public FadeUIAction(Color startColor, Color desiredColor, float _duration, GameObject parent, float delay = 0.0f) : base(_duration,parent,delay)
    {
        _image = parent.GetComponent<Image>();
        _desiredColor = desiredColor;
        _startColor = startColor;
    }
    
    public override bool ActionUpdate()
    {
        _image.color = Color.Lerp(_startColor, _desiredColor, percentDone);
        return base.ActionUpdate();
    }
}

public class ScaleUIAction : Action
{
    private RectTransform _rectTransform;
    private Vector3 _startScale;
    private Vector3 _desiredScale;

    public ScaleUIAction(Vector3 desiredScale, float _duration, GameObject parent, float delay = 0.0f) : base(_duration,parent,delay)
    {
        _rectTransform = parent.GetComponent<RectTransform>();
        _startScale = _rectTransform.lossyScale;
        _desiredScale = desiredScale;
    }
    
    public ScaleUIAction(Vector3 startScale, Vector3 desiredScale, float _duration, GameObject parent, float delay = 0.0f) : base(_duration,parent,delay)
    {
        _rectTransform = parent.GetComponent<RectTransform>();
        _startScale = startScale;
        _desiredScale = desiredScale;
    }
    
    public override bool ActionUpdate()
    {
        _rectTransform.localScale = Vector3.Lerp(_startScale, _desiredScale, percentDone);
        return base.ActionUpdate();
    }
}                                                                  

public class SpawnGameObjectAction : Action
{
    private GameObject _spawnie;
    private Vector3 _pos = Vector3.negativeInfinity;

    public SpawnGameObjectAction(Vector3 pos, GameObject spawnie, float _duration, GameObject parent, float delay = 0.0f) : base(_duration,parent,delay)
    {
        _pos = pos;
        _spawnie = spawnie;
    }
    
    public SpawnGameObjectAction( GameObject spawnie, float _duration, GameObject parent, float delay = 0.0f) : base(_duration,parent,delay)
    {
        _pos = Vector3.negativeInfinity;
        _spawnie = spawnie;
    }

    public override bool ActionUpdate()
    {
        if (_delay <= 0.0f)
        {
            Object.Instantiate(_spawnie, parentObject.transform);
            return true;
        }
        else
        {
            return false;
        }
    }
}

public class RotateToAction : Action
{
    private Vector3 startRotation;
    private Vector3 endRotation;
    private Transform _transform;

    public RotateToAction(Vector3 nangle, float nduration, GameObject parent, float delay = 0.0f) : base(nduration,parent,delay)
    {
        startRotation = parent.transform.rotation.eulerAngles;
        endRotation = nangle;
        _transform = parent.transform;
    }

    public override bool ActionUpdate()
    {
        _transform.rotation = Quaternion.Euler(Vector3.Lerp(startRotation,endRotation, percentDone));
        return base.ActionUpdate();
    }
}

public class Translate : Action
{
    private Vector3 _startPos;
    private Vector3 _desiredPos;
    private bool takeFromParent;

    public Translate(Vector3 sPos, Vector3 desiredPos, float _duration, GameObject parent, float delay = 0.0f, bool _block = false) : base(_duration,parent,delay)
    {
        _startPos = sPos;
        _desiredPos = desiredPos;
        blocking = _block;
        takeFromParent = false;
    }
    
    public Translate(Vector3 desiredPos, float _duration, GameObject parent, float delay = 0.0f, bool _block = false) : base(_duration,parent,delay)
    {
        _startPos = parent.transform.position;
        _desiredPos = desiredPos;
        blocking = _block;
        takeFromParent = true;
    }

    public override bool ActionUpdate()
    {
        if (isFirstUpdate())
        {
            if (_startPos != parentObject.transform.position && takeFromParent == true)
                _startPos = parentObject.transform.position; 
        }

        parentObject.transform.position = Vector3.Lerp(_startPos, _desiredPos, percentDone);
        return base.ActionUpdate();
    }
}

public class ScaleTo : Action
{
    public Vector3 _startScale;
    public Vector3 _endScale;
    public ScaleTo(Vector3 desiredScale, float _duration, GameObject parent, float delay = 0.0f, bool _block = false) : base(_duration,parent,delay)
    {
        _startScale = parent.transform.lossyScale;
        _endScale = desiredScale;
        blocking = _block;
    }

    public override bool ActionUpdate()
    {
        parentObject.transform.localScale = Vector3.Lerp(_startScale, _endScale, percentDone);
        return base.ActionUpdate();
    }
}

public class WaitAction : Action
{
    public WaitAction(float _duration, bool blocks = true) 
    {
        duration = _duration;
        blocking = blocks;
    }
}


