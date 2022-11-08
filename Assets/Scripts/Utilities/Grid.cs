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

        public List<GridRow> gridRows;
        
        void Start()
        {

        }

        void Update()
        {

        }

        public void Init()
        {
            if (multipleGround)
            {

            }
            else
            {

            }
        }

        public class GridRow
        {
            public int rowId;
            public List<GridPoint> gridPoints;
        }

        public class GridPoint
        {
            public int pointid;
            public bool occupied;
            public Vector3 position;
        }
    }
}


