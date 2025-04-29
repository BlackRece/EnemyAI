using System.Collections.Generic;
using System.Linq;

using UnityEngine;
using UnityEngine.Events;

using RNG = UnityEngine.Random;

namespace BlackRece.Enemies {
    
    public class EnemyManager : MonoBehaviour
    {
        [SerializeField] private GameObject[] m_EnemyPrefab;
        [SerializeField] private int mi_EnemyCount = 10;
        private List<Enemy> m_Enemies = new List<Enemy>();
        
        [SerializeField] private float mf_SpawnRadius = 100.0f;
        [SerializeField] private UnityEvent<Vector3> m_OnBaseSpawn;
        public static UnityEvent<int> m_OnBehaviour = new UnityEvent<int>();
        
        private bool mb_HasFort = false;
        private Vector3 mv3_FortPosition = Vector3.zero;
        private GameObject m_Fort = null;
        private Camera m_Cam = null;

        private bool mb_enemyToggle = false;
        /*
        private void OnEnable() {
            TMC_Timer.AddListener(AlertTags.CGS_ALERT_DAWN, OnDayStart);
            TMC_Timer.AddListener(AlertTags.CGS_ALERT_DAWN, OnNightStart);
            
            //DEBUG
            InputHandler.OnMouseMiddleClickUp.AddListener(OnBaseSpawn);
        }
        
        private void OnDisable() {
            TMC_Timer.RemoveListener(AlertTags.CGS_ALERT_DAWN, OnDayStart);
            TMC_Timer.RemoveListener(AlertTags.CGS_ALERT_DAWN, OnNightStart);
            
            //DEBUG
            InputHandler.OnMouseMiddleClickUp.RemoveListener(OnBaseSpawn);
        }

        private void Awake() {
            // get fort object
            //m_Fort = GameObject.FindGameObjectWithTag("Fort");
        }
        */

        private void Start() {
            m_Cam = Camera.allCameras.FirstOrDefault();
            
            if (m_Cam == null) {
                Debug.LogError("No camera found");
                return;
            }

            m_OnBehaviour.AddListener(OnSetBehaviour);
        }

        void Update()
        {
            if (Input.GetMouseButtonUp(0)) {
                Vector3 l_pos = Input.mousePosition;
                MarkPos(l_pos);
            }

            if (Input.GetMouseButtonUp(1))
                ToggleEnemies();

            if(Input.GetKeyUp(KeyCode.Alpha0))
                m_OnBehaviour?.Invoke(0);
            
            if(Input.GetKeyUp(KeyCode.Alpha1))
                m_OnBehaviour?.Invoke(1);
            
            if (m_Fort != null) {
                if (!mb_HasFort) {
                    // get fort position
                    mv3_FortPosition = m_Fort.transform.position;
                    mb_HasFort = true;
                }
            }  
        }
        
        private void OnNightStart() {
            if (!mb_HasFort) return;
            
            // spawn minions
            Debug.Log("Night started");
            SpawnEnemies();
        }
        
        private void OnDayStart() {
            // remove minions
            Debug.Log("Day started");
            RemoveEnemies();
        }

        private void ToggleEnemies() {
            mb_enemyToggle = !mb_enemyToggle;
            
            if(mb_enemyToggle)
                SpawnEnemies();
            else
                RemoveEnemies();
        }
        
        private void SpawnEnemies() {
            if (m_Enemies.Count > 0)
                return;
            
            for (int i = 0; i < mi_EnemyCount; i++) {
                // spawn minions
                GameObject l_EnemyGO = Instantiate(m_EnemyPrefab[0], GetRandomSpawnPosition(), Quaternion.identity);
                var l_Enemy = l_EnemyGO.GetComponent<Enemy>();
                //mv3_FortPosition = m_Fort == null ? Vector3.zero : m_Fort.transform.position;
                l_Enemy.SetTarget(mv3_FortPosition);
                m_OnBehaviour.AddListener(l_Enemy.SetBehaviour);
                m_Enemies.Add(l_Enemy);
            }
        }
        
        private void RemoveEnemies() {
            if (m_Enemies.Count <= 0)
                return;
            
            // remove minions
            foreach (var l_Enemy in m_Enemies) {
                Destroy(l_Enemy.gameObject);
            }
            m_Enemies.Clear();
        }
        
        // Get a random position on the circumference of a circle centered on the fort
        private Vector3 GetRandomSpawnPosition() {
            Vector3 l_RandomPosition = Vector3.zero;
            float l_RandomAngle = RNG.Range(0, 360);
            l_RandomPosition.x = mv3_FortPosition.x + mf_SpawnRadius * Mathf.Cos(l_RandomAngle);
            l_RandomPosition.z = mv3_FortPosition.z + mf_SpawnRadius * Mathf.Sin(l_RandomAngle);
            return l_RandomPosition;
        }

        public void OnBaseSpawn(Vector3 a_Position) {
            // DEBUG METHOD - REMOVE LATER
            //if (mb_HasFort) return;

            if(m_Cam == null) 
                m_Cam = Camera.allCameras.FirstOrDefault();
            
            if (m_Cam == null) {
                Debug.LogError("No camera found");
                return;
            }
            
            Ray l_ray = m_Cam.ScreenPointToRay(a_Position);
            Physics.Raycast(l_ray, out RaycastHit l_hit);
            
            mv3_FortPosition = l_hit.point;
            GameObject.CreatePrimitive(PrimitiveType.Sphere).transform.position = mv3_FortPosition;
            
            mb_HasFort = true;
        }

        private void MarkPos(Vector3 a_Position) {
            Ray l_ray = m_Cam.ScreenPointToRay(a_Position);
            Physics.Raycast(l_ray, out RaycastHit l_hit);
            
            mv3_FortPosition = l_hit.point;
            mv3_FortPosition.y = 0;
            if(m_Fort == null)
                m_Fort = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            m_Fort.transform.position = mv3_FortPosition;
        }

        private void OnSetBehaviour(int a_BehaviourID) {
            Debug.Log($"behaviour {a_BehaviourID}");
        }
    }
}