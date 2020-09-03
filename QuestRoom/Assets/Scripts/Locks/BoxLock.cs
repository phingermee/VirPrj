using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

class Box
{    
    public List<Color> cellColors = new List<Color>() { new Color(255, 0, 0), new Color(255, 255, 0), new Color(0, 255, 0), new Color(0, 0, 255) };
    Image Cell { get; set; }
    
    int _cellColorIndex;
    public int CellColorIndex
    {
        get
        {
            return _cellColorIndex;
        }
        set
        {
            if (value > cellColors.Count-1)
                _cellColorIndex = 0;
            else if (value < 0)
                _cellColorIndex = cellColors.Count-1;
            else _cellColorIndex = value;
        }
    }

    public Box(GameObject gObj, int colind)
    {
        Cell = gObj.transform.GetChild(0).GetChild(0).GetComponent<Image>();
        CellColorIndex = colind;        
    }
}

public class BoxLock : MonoBehaviour
{
    Modes mode;
    GameObject seifCap;
    List<Box> cells;

    void Start()
    {
        mode = GameObject.FindGameObjectWithTag("GameController").GetComponent<Modes>();
        seifCap = GameObject.FindGameObjectWithTag("Cap");
        cells = new List<Box>()
        {
        new Box(this.gameObject, 0),
        new Box(this.gameObject, 1),
        new Box(this.gameObject, 0),
        new Box(this.gameObject, 3)
        };
    }

    public void Exit()
    {
        mode._isBoxLockActive = false;
        Destroy(this.gameObject);
    }

    IEnumerator OpenBox()
    {
        for (int i = 0; i < 90; i++)
        {
            seifCap.transform.Rotate(new Vector3(-1, 0, 0));
            yield return new WaitForFixedUpdate();
        }
        Exit();
    }

    void Display(int partNum)
    {
        Box cell = cells[partNum];
        transform.GetChild(partNum).GetChild(0).GetComponent<Image>().color = cell.cellColors[cell.CellColorIndex];
        int count = 0;
        for (int i = 0; i < 4; i++)
        {
            if (transform.GetChild(i).GetChild(0).GetComponent<Image>().color == cell.cellColors[2])
                count++;
        }
        if (count == 4)
        {
            StartCoroutine(OpenBox());
        }
    }

    public void ChangeColorUp(int partNum)
    {
        cells[partNum].CellColorIndex--;
        Display(partNum);
    }

    public void ChangeColorDown(int partNum)
    {
        cells[partNum].CellColorIndex++;
        Display(partNum);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            Exit();
        }
    }
}
