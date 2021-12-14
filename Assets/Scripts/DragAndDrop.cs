using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    [SerializeField] private int ID;
    [SerializeField] private int Index;

    private Collider2D collider;

    private GameScript gameEvents;
    private bool canMove;
    private bool dragging;
    private bool isExterminable;
    [SerializeField] private bool IsMovement;
    private bool inGameZone;
    private Vector2 StartPosition;

    void Start()
    {
        collider = GetComponent<Collider2D>();
        canMove = false;
        dragging = false;
        isExterminable = false;
        inGameZone = false;
        StartPosition = transform.position;
    }

    private void Update()
    {
        if (IsMovement)
        {
            Vector2 mousePos = Input.mousePosition;

            if (Input.GetMouseButtonDown(0))
            {
                if (collider == Physics2D.OverlapPoint(mousePos))
                {
                    Debug.Log("OverlapPoint");
                    canMove = true;
                }
                else
                {
                    canMove = false;
                }
                if (canMove)
                {
                    dragging = true;
                }
            }

            if (dragging)
            {
                this.transform.position = mousePos;
            }

            if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("MouseButtonUp");
                canMove = false;
                dragging = false;
                if (inGameZone)
                {
                    inGameZone = false;
                    Debug.Log(this.name + " index = " + Index);
                    gameEvents.OnMoveBlockToSlot(Index);
                }
                else if (isExterminable)
                {
                    isExterminable = false;
                    gameEvents.OnDeleteBlock(Index);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Return to start pos = " + StartPosition);
                    this.transform.position = StartPosition;
                }
            }
        }
    }

    public int Id
    { 
        get
        {
            return ID;
        }
        set
        {
            ID = value;
        }
    }

    public int index
    {
        get
        {
            return Index;
        }
        set
        {
            Index = value;
        }
    }

    public GameScript GameEvent
    {
        set
        {
            gameEvents = value;
        }
    }

    public bool isMovement
    {
        set
        {
            IsMovement = value;
        }
    }

    public Vector2 startPosition
    {
        set
        {
            StartPosition = value;
        }
    }

    public void MoveBlock(Transform incomPos)
    {
        transform.position = incomPos.position;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trash"))
        {
            isExterminable = true;
        }
        if (collision.gameObject.CompareTag("Slot"))
        {
            inGameZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Trash"))
        {
            isExterminable = false;
        }
        if (collision.gameObject.CompareTag("Slot"))
        {
            inGameZone = false;
        }
    }
}
