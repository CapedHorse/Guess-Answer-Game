using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
namespace PikoruaTest
{
    public class Grid : MonoBehaviour
    {
        public Transform gridParent;
        public bool multipleGround;

        [HideIf(nameof(multipleGround))] public Collider groundCollider;
        [ShowIf(nameof(multipleGround))] public List<Collider> groundColliders;

        public List<GridPoint> gridPoints;
        //public List<GridRow> gridRows;
        
        [Button("Test Init Grid")]
        public void Init()
        {
            var count = GameManager.instance == null? 50: GameManager.instance.gameData.respondenCountInDisplay;
            //gridRows = new List<GridRow>();
             
            if (multipleGround)
            {
                var rowCount = groundColliders.Count;
                var pointPerRow = count / rowCount;

                for (int i = 0; i < rowCount; i++)
                {
                    var gridWidth = groundColliders[i].bounds.size.x;
                    //var gridRow = new GridRow();
                    //gridRow.rowId = i;
                    //gridRow.gridPoints = new List<GridPoint>();
                    for (int j = 0; j < pointPerRow; j++)
                    {
                        var pointWidth = (gridWidth / pointPerRow);
                        var pointPos = groundColliders[i].bounds.min;
                        pointPos.x = pointPos.x + (pointWidth/2) +(pointWidth * j);
                        pointPos.y = groundColliders[i].bounds.max.y;
                        pointPos.z = groundColliders[i].bounds.center.z;
                        var gridPoint = new GridPoint();
                        //gridPoint.pointid = j;
                        gridPoint.position = pointPos;
                        gridPoints.Add(gridPoint);
                        //gridRow.gridPoints.Add(gridPoint);
                    }
                    //gridRows.Add(gridRow);
                }
            }
            else
            {
                
                var rowCount = Mathf.RoundToInt(groundCollider.bounds.size.z);
                var pointPerRow = count / rowCount;
                for (int i = 0; i < rowCount; i++)
                {
                    var gridWidth = groundCollider.bounds.size.x;
                    var gridHeight = groundCollider.bounds.size.z;
                    //gridRow.rowId = i;
                    
                    for (int j = 0; j < pointPerRow; j++)
                    {
                        var pointWidth = (gridWidth / pointPerRow);
                        var pointHeight = (gridHeight / rowCount);

                        var pointPos = groundCollider.bounds.min;
                        pointPos.x = pointPos.x + (pointWidth / 2)+(pointWidth * j);
                        pointPos.y = groundCollider.bounds.max.y;
                        pointPos.z = pointPos.z + (pointHeight / 2) + (pointHeight * i);

                        var gridPoint = new GridPoint();

                        //gridPoint.pointid = j;
                        gridPoint.position = pointPos;
                        gridPoints.Add(gridPoint);
                    }
                    //gridRows.Add(gridRow);
                }
            }
        }

       

        [System.Serializable]
        public class GridRow
        {
            public List<GridPoint> gridPoints;
        }

        [System.Serializable]
        public class GridPoint
        {
            
            public bool occupied;
            public Vector3 position;
        }
        private void OnDrawGizmos()
        {
            
                if (gridPoints.Count <= 0 || gridPoints == null)
                    return;

                foreach (var point in gridPoints)
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawCube(point.position, Vector3.one * 0.5f);
                    
                }
            
        }

    }
}


