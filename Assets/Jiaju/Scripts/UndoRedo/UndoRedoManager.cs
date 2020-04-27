using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Portalble.Functions.Grab;

public class UndoRedoManager : MonoBehaviour
{

    public static UndoRedoManager URMgr;

    public Stack<ICommand> UndoStack = new Stack<ICommand>();
    public Stack<ICommand> RedoStack = new Stack<ICommand>();
    public FoamDataManager m_data;

    void Awake()
    {
        if (URMgr != null)
        {
            GameObject.Destroy(URMgr);
        }
        else
        {
            URMgr = this;
        }

        DontDestroyOnLoad(this);

        if (GameObject.Find(gameObject.name) && GameObject.Find(gameObject.name) != this.gameObject)
        {
            Destroy(GameObject.Find(gameObject.name));
        }
    }

    public void AddNewAction(ICommand newAction)
    {
        UndoStack.Push(newAction);
        RedoStack.Clear();
    }

    public void UndoAction()
    {
        if (!CanPerformUndoRedo()) return;

        Debug.Log("Undo Button Presseed");
        if (UndoStack.Count > 0)
        {
            ICommand actionUndone = UndoStack.Pop();
            actionUndone.Undo();
            RedoStack.Push(actionUndone);
        }
    }

    public void RedoAction()
    {
        if (!CanPerformUndoRedo()) return;

        Debug.Log("Redo Button Presseed");
        if (RedoStack.Count > 0)
        {
            ICommand actionRedone = RedoStack.Pop();
            actionRedone.Redo();
            UndoStack.Push(actionRedone);
        }
    }

    private bool CanPerformUndoRedo()
    {
        if (Grab.Instance.IsGrabbing)
        {
            return false;
        }

        if (!m_data.StateMachine.GetCurrentAnimatorStateInfo(0).IsTag("Switchable"))
        {
            return false;
        }

        return true;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
