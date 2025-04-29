using System.Collections.Generic;

using Unity.AI.Navigation;

using UnityEngine;

namespace BlackRece.FloorGen {
    [RequireComponent(typeof(NavMeshSurface))]
    public class FloorManager : MonoBehaviour {
        [SerializeField] private GameObject go_floorPrefab = null;
        [SerializeField] private Vector2Int v2i_floorSize =  new Vector2Int(3, 3);
        
        private NavMeshSurface m_navMeshSurface;
        private List<GameObject> m_floors = new List<GameObject>();
        
        private readonly Vector2 v2_tileSize = new Vector2(10, 10); 
        
        private void Awake() {
            if(go_floorPrefab == null) 
                throw new UnassignedReferenceException("go_floorPrefab not set");
            
            m_navMeshSurface = GetComponent<NavMeshSurface>();
            
        }

        private void Start() {
            //var l_tileSize = 
            var l_pos = transform.position;
            for (int x = 0; x < v2i_floorSize.x; x++) {
                for (int y = 0; y < v2i_floorSize.y; y++) {
                    var l_floorPos = new Vector3(l_pos.x + (x * v2_tileSize.x), 0, l_pos.y + (y *  v2_tileSize.y));
                    GameObject go_floor = Instantiate(go_floorPrefab, l_floorPos, Quaternion.identity);
                    go_floor.transform.SetParent(transform);
                    m_floors.Add(go_floor);
                }
            }
            m_navMeshSurface.BuildNavMesh();
        }

        // Update is called once per frame
        void Update() { }
    }
}