using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameScript : MonoBehaviour
{
    [SerializeField] private IntegerVariable score;
    [SerializeField] private int combinationSize = 3;
    [SerializeField] private int colorCount = 3;
    [SerializeField] private int panelCount = 5;
    [SerializeField] private float levelTimer = 60;
    [SerializeField] private float visionTimer = 6;

    [SerializeField] private Text levelTimerText;
    [SerializeField] private Text visionTimerText;
    [SerializeField] private Text levelText;
    [SerializeField] private Text scoreText;

    [SerializeField] private GameObject[] blockPrefabs;
    [SerializeField] private GameObject slotPrefab;

    [SerializeField] private GameObject blockPanel;
    [SerializeField] private Transform slotPanel;

    [SerializeField] private GameObject[] panelPositions;

    private GameObject[] panelBlocks;
    private GameObject[] sampleBlocks;
    private GameObject[] slotPositions;
    private List<GameObject> gamerBlocks;

    private int levelNumber = 1;
    private bool isVision = true;
    private bool isPlayStop = false;
    private System.Random rand = new System.Random();

    private float usedStartTimer;
    private float usedVisionTimer;
    private int usedCombinationSize;
    private int usedPanelCount;
    private int usedColorCount;

   // private RectTransform panelRect;
    private float panelHiegh;
    //private float panelWidth;
    private float prefabHieght;
    //private float prefabWidth;
    //private double columnsNumber = 1;
    //private double lineNumber = 1;

    public delegate void OnSlotDrop();
    private event OnSlotDrop slotDropListener;
    public event OnSlotDrop SlotDropListeners
    {
        add
        {
            slotDropListener -= value;
            slotDropListener += value;
        }
        remove => slotDropListener -= value;
    }

    private void Start()
    {
        RectTransform panelRect = slotPanel.GetComponent<RectTransform>();
        panelHiegh = panelRect.rect.height;
        //panelWidth = panelRect.rect.width;
        RectTransform prefabRect = slotPrefab.GetComponent<RectTransform>();
        prefabHieght = prefabRect.rect.height;
        //prefabWidth = prefabRect.rect.width;
        usedStartTimer = levelTimer;
        usedVisionTimer = visionTimer;
        usedColorCount = colorCount;
        usedPanelCount = panelCount;
        usedCombinationSize = combinationSize;

        slotPositions = new GameObject[usedCombinationSize];
        sampleBlocks = new GameObject[usedCombinationSize];
        gamerBlocks = new List<GameObject>();
        panelBlocks = new GameObject[usedPanelCount];

        levelTimerText.text = levelTimer.ToString();

        ReloadSlots();
        ReloadSimple();
        ReloadBlocks();
        VisionBlockPanel(false);
    }

    void Update()
    {
        if (!isPlayStop)
        {
            if (isVision)
            {
                //Debug.Log("vision timer go");
                usedVisionTimer -= Time.deltaTime;
                levelTimerText.text = "";
                visionTimerText.text = usedVisionTimer.ToString();
                if (usedVisionTimer < 0)
                {
                    visionTimerText.text = "";
                    isVision = false;
                    HidenSample();
                    VisionBlockPanel(true);
                }
            }
            else
            {
                //Debug.Log("game timer go");
                usedStartTimer -= Time.deltaTime;
                if (usedStartTimer < 0)
                {
                    levelTimerText.text = "";
                    LoseGame();
                }
                levelTimerText.text = usedStartTimer.ToString();
            }
        }
        else
        {
            levelTimerText.text = "";
        }
    }

    private void HidenSample()
    {
        //Debug.Log("HidenSample()");
        foreach (GameObject block in sampleBlocks)
        {
            block.SetActive(false);
        }
    }

    private void VisionBlockPanel(bool incom)
    {
        blockPanel.SetActive(incom);
    }

    private void NewLevel()
    {
        //Debug.Log("NewLevel() go");
        AddScore();
        ClearGamerBlock();
        if (levelNumber % 10 == 0)
        {
            usedCombinationSize++;
            usedColorCount++;
            ReloadSlots();
        }
        usedVisionTimer = visionTimer - levelNumber;
        usedStartTimer = levelTimer - levelNumber / 10;
        ReloadSimple();
        VisionBlockPanel(false);
        isVision = true;
        isPlayStop = false;
        //Debug.Log("NewLevel() Finish");
    }

    private void AddScore()
    {
        int addScore = System.Convert.ToInt32(System.Math.Ceiling(usedStartTimer * 0.1 * levelNumber));
        score.ApplyChange(addScore);
        scoreText.text = "Score: " + score.GetValue().ToString();
    }

    private Vector3 GetStartPosition(int incomIndex)
    {
        float x = 0;
        float y = (prefabHieght * 0.96f * usedCombinationSize) / 2 - incomIndex * prefabHieght;
        Vector3 pos = new Vector3(x, y, 0);
        Debug.Log("pos = " + pos.ToString());
        Vector3 result = transform.TransformPoint(pos);
        Debug.Log("pos = " + result.ToString());
        return result;
    }

    private void CheckPanelArea()
    { 
        if (panelHiegh < prefabHieght * usedCombinationSize * 0.96f)
        {
            // Скажем потом решим выстраивать в 2 колонки и более
            /* columnsNumber = System.Math.Ceiling((prefabHieght * usedCombinationSize * 0.96f) / panelHiegh);
         }
         if (panelWidth < prefabWidth * columnsNumber * 0.96f)
         {*/
            Debug.Log("Места неть");
            LoseGame();
        }
    }

    private void ReloadSlots()
    {
        CheckPanelArea();
        //Debug.Log("!!! ReloadSlots() go");
        foreach (GameObject slot in slotPositions)
        {
            Destroy(slot);
        }
        slotPositions = new GameObject[usedCombinationSize];
        for (int i = 0; i < usedCombinationSize; i++)
        {
            GameObject slot = AddSlot(GetStartPosition(i), slotPanel);
            slot.name = "slot_" + i.ToString();
            slotPositions[i] = slot;
        }
        //Debug.Log("!!! ReloadSlots() Finish");
    }

    private void ReloadSimple()
    {
        //Debug.Log("+++ ReloadSimple() go");
        foreach (GameObject sample in sampleBlocks)
        {
            Destroy(sample);
        }
        sampleBlocks = new GameObject[usedCombinationSize];
        for (int i = 0; i < usedCombinationSize; i++)
        {
            sampleBlocks[i] = AddBlock(slotPositions[i].transform, slotPanel, i, false);
            DragAndDrop blockID = sampleBlocks[i].GetComponent<DragAndDrop>();
        }
        //Debug.Log("+++ ReloadSimple() Finish");
    }

    private void ReloadBlocks()
    {
        //Debug.Log("--- ReloadBlocks() go");
        foreach (GameObject block in panelBlocks)
        {
            if (block != null)
            {
                Debug.Log("Destroy(block) name = " + block.name);
            }
            Destroy(block);
        }
        for (int i = 0; i < usedPanelCount; i++)
        {
            panelBlocks[i] = AddBlock(panelPositions[i].transform, blockPanel.transform, i, true);
        }
        //Debug.Log("--- ReloadBlocks() Finish");
    }

    private void ClearGamerBlock()
    {
        foreach (GameObject block in gamerBlocks)
        {
            Destroy(block);
        }
        gamerBlocks.Clear();
    }

    private GameObject AddBlock(Transform incomPos, Transform parent, int index, bool isMove)
    {
        int colorNumber = rand.Next(0, usedColorCount);
        Object prefab = blockPrefabs[colorNumber];
        Vector3 vec = new Vector3(incomPos.position.x, incomPos.position.y, 0);
        Object newObj = Instantiate(prefab, vec, new Quaternion(), parent);
        DragAndDrop block = (newObj as GameObject).GetComponent<DragAndDrop>();
        block.GameEvent = this;
        block.index = index;
        block.isMovement = isMove;
        return newObj as GameObject;
    }

    private GameObject AddSlot(Vector3 vector, Transform parent)
    {
        return Instantiate(slotPrefab, vector, new Quaternion(), parent);
    }

    public void OnMoveBlockToSlot(int incomIndex)
    {
        MoveBlockToSlot(incomIndex);
    }

    private void MoveBlockToSlot(int incomIndex)
    {
        GameObject droppedBlock = panelBlocks[incomIndex];
        droppedBlock.name = "gamerBlock_" + gamerBlocks.Count;
        DragAndDrop blockScript = droppedBlock.GetComponent<DragAndDrop>();
        blockScript.MoveBlock(slotPositions[gamerBlocks.Count].transform);
        blockScript.isMovement = false;
        gamerBlocks.Add(droppedBlock);
        
        if (!GamerBlocksCountCheck())
        {
            RefreshBlockQueue(incomIndex);
        }
        else
        {
            GameEnd();
            RefreshBlockQueue(incomIndex);
        }
    }

    private void RefreshBlockQueue(int incomIndex)
    {
        for (int i = incomIndex; i < panelBlocks.Length - 1; i++)
        {
            panelBlocks[i] = panelBlocks[i + 1];
            DragAndDrop block = panelBlocks[i].GetComponent<DragAndDrop>();
            block.index = i;
            block.MoveBlock(panelPositions[i].transform);
            block.startPosition = new Vector2(panelPositions[i].transform.position.x, panelPositions[i].transform.position.y);
        }
        GameObject newBlock = AddBlock(panelPositions[panelBlocks.Length - 1].transform, blockPanel.transform, panelBlocks.Length - 1, true);
        panelBlocks[panelBlocks.Length - 1] = newBlock;
    }

    public void OnDeleteBlock(int incomIndex)
    {
        DeleteBlock(incomIndex);
    }

    private void DeleteBlock(int incomIndex)
    {
        RefreshBlockQueue(incomIndex);
    }

    private bool IsGamerComboCheck()
    {
        Debug.Log("IsGamerComboCheck()");
        bool result = true;
        for (int i = 0; i < usedCombinationSize; i++)
        {
            DragAndDrop gamerId = gamerBlocks[i].GetComponent<DragAndDrop>();
            DragAndDrop sampleID = sampleBlocks[i].GetComponent<DragAndDrop>();
            Debug.Log("sampleID = " + sampleID.Id + ", gamerId.Id = " + gamerId.Id);
            if (gamerId.Id != sampleID.Id)
            {
                result = false;
            }
        }
        return result;
    }

    private bool GamerBlocksCountCheck()
    {
        if (gamerBlocks.Count == usedCombinationSize)
        {
            return true;
        }
        return false;
    }

    private void GameEnd()
    {
        Debug.Log("GameEnd() go");
        isPlayStop = true;
        if (IsGamerComboCheck())
        {
            levelNumber++;
            levelText.text = "Level " + levelNumber;
            NewLevel();
        }
        else
        {
            Debug.Log("GameEnd() Finish new scene");
            LoseGame();
        }
        Debug.Log("GameEnd() Finish");
    }

    private void LoseGame()
    {
        SceneManager.LoadScene("EndScene");
    }
}
