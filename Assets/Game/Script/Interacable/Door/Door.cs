using UnityEngine;
using UnityEngine.Events;
 
public class Door : MonoBehaviour, IInteractable
{
   protected Coroutine _animatingDoorCoroutine;
    [SerializeField]
    private string _name;
   
    public string Name => _name;

    [SerializeField]
    protected Transform _doorTransform;
    [SerializeField]
    protected float _duration = 1f;
    [SerializeField]
    protected bool _isLocked;
    [SerializeField]
    protected string _keyID;
    protected bool _isAnimating;
 
    protected bool _isOpen;
    public bool IsAnimating => _isAnimating;
    public UnityEvent OnDoorOpen;
    public UnityEvent OnDoorClose;
[ContextMenu("Interact Door")]
    public void Interact()
    {
       if (_isOpen == true)
        {
            Close();
        }
        else
        {
            Open();
        }
    }
public virtual void Open()
    {
    
        _isOpen = true;
   
        OnDoorOpen?.Invoke();
    }
 
  
    public virtual void Close()
    {
        
        _isOpen = false;
        
        OnDoorClose?.Invoke();
    }
}