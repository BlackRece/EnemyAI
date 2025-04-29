using UnityEngine;
using UnityEngine.AI;

namespace BlackRece.Obstacles {
    [RequireComponent(typeof(NavMeshObstacle))]
    public class Obstacle : MonoBehaviour {
        private NavMeshObstacle m_obstacle;
        private void Awake() {
            m_obstacle = GetComponent<NavMeshObstacle>();
        }

        private void Start() {
            m_obstacle.carving = true;
        }

    }
}