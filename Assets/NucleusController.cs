using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class NucleusController : MonoBehaviour {
    public static NucleusController instance { get; private set; }

    public GameObject spawnPoint;
    public GameObject cellPrefab;

    private List<Cell> cellList = new List<Cell>();

    private float lastSpawnTime;

    void Awake() {
        instance = this;
        cellPrefab.SetActive(false);
    }

    void Start()
    {
        lastSpawnTime = 0;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (Time.time > lastSpawnTime + 1)
        {
            //AddCell();
            lastSpawnTime = Time.time;
        }
    }

    public void AddCellButtonClicked()
    {
        AddCell();
    }

    public Cell AddCell()
    {
        GameObject cellObj = Instantiate(cellPrefab);
        cellObj.SetActive(true);
        cellObj.transform.position =
            spawnPoint.transform.position + new Vector3(Random.Range(-4, 4), 0, Random.Range(-4, 4));

        Cell cell = cellObj.GetComponent<Cell>();

        cellList.Add(cell);

        cell.SetLevel(1);

        return cell;
    }

    public void RemoveCell(Cell cell)
    {
        cellList.Remove(cell);
        Destroy(cell.gameObject);
    }

    const float G = 5f; // universal gravity constant
    void FixedUpdate()
    {
        if (cellList.Count < 2) return;

        // calculate only other planets gravity!
        foreach (var cell in cellList) {
            var resForce = Vector3.zero;
            var cellRb = cell.GetComponent<Rigidbody>();
            foreach (var testCell in cellList)
            {
                if (testCell == cell) continue;

                var testRb = testCell.GetComponent<Rigidbody>();

                var dir = testCell.gameObject.transform.position - cell.transform.position; // get the force direction
                dir.y = 0;
                var dist2 = dir.sqrMagnitude; // get the squared distance

                // calculate the force intensity using Newton's law
                var gForce = G * cellRb.mass * testRb.mass / dist2;
                gForce = Math.Min(gForce, 1);

                resForce += gForce * dir.normalized; // accumulate in the resulting force variable
            }
            
            cellRb.AddForce(resForce); // apply the resulting gravity force
        }
      
    }

    void MurderCluster()
    {
        if (cellList.Count < 2) return;

        // calculate only other planets gravity!
        foreach (var cell in cellList) {
            var resForce = Vector3.zero;
            var cellRb = cell.GetComponent<Rigidbody>();
            foreach (var testCell in cellList)
            {
                if (testCell == cell) continue;

                var testRb = testCell.GetComponent<Rigidbody>();

                var dir = testCell.gameObject.transform.position - cell.transform.position; // get the force direction
                var dist2 = dir.sqrMagnitude; // get the squared distance

                // calculate the force intensity using Newton's law
                var gForce = G * cellRb.mass * testRb.mass / dist2;
                resForce += gForce * dir.normalized; // accumulate in the resulting force variable
            }
            

            if (cellRb.position.y < 0.2)
            {
                cellRb.AddForce(resForce); // apply the resulting gravity force
                
            }
            else
            {
                cellRb.AddForce(new Vector3(0, -5, 0));
            }
        }
    }
}
