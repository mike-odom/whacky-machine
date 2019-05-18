using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Material level1Material;
    public Material level2Material;
    public Material level3Material;

    private List<GameObject> touching = new List<GameObject>();

    private int level;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -1)
        {
            NucleusController.instance.RemoveCell(this);
        }

        if (touching.Count > 2)
        {
            var pos = (transform.position + touching[0].transform.position + touching[1].transform.position) / 3;

            var nc = NucleusController.instance;

            var newCell = nc.AddCell();
            newCell.transform.position = pos;
            newCell.SetLevel(level + 1);

            nc.RemoveCell(touching[0].GetComponent<Cell>());
            nc.RemoveCell(touching[1].GetComponent<Cell>());
            nc.RemoveCell(this.GetComponent<Cell>());

            touching.RemoveRange(0, 2);
           
        }
    }

    void OnDestroy()
    {
        foreach (var touched in touching)
        {
            var cell = touched.GetComponent<Cell>();
            if (cell != null)
            {
                cell.OtherCellDestroyed(this.gameObject);
            }
        }
    }

    public void OtherCellDestroyed(GameObject cellObj)
    {
        touching.Remove(cellObj);
    }

    public void SetLevel(int newLevel)
    {
        level = newLevel;

        Material material = null;

        if (level == 1)
        {
            material = level1Material;
        } else if (level == 2)
        {
            Debug.Log("Setting level 2");
            material = level2Material;
        }else if (level == 3)
        {
            Debug.Log("Setting level 3");
            material = level3Material;
        }

        var renderer = gameObject.GetComponent<MeshRenderer>();
        var materials = renderer.materials;
        materials[0] = material;
        renderer.materials = materials;
    }

    public void OnCollisionEnter(Collision col)
    {
        var cell = col.gameObject.GetComponent<Cell>();
        if (!cell) return;

        if (cell.level == level)
        {
            touching.Add(col.gameObject);
        }
    }

    public void OnCollisionLeave(Collision col)
    {
        touching.Remove(col.gameObject);
    }
}
