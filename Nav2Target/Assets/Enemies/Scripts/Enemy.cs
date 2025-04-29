using UnityEngine;
    using UnityEngine.AI;

namespace BlackRece.Enemies {
    [RequireComponent(
        typeof(NavMeshAgent),
        typeof(SphereCollider),
        typeof(Rigidbody)
    )]
    public class Enemy : MonoBehaviour {
        [SerializeField] private float mf_AttackRange = 0.5f;
        
        private enum Actions {
            Idle = 0,
            Seek = 1,
            MeleeAttack = 2,
            RangedAttack = 3,
        }
        
        private NavMeshAgent m_NavMeshAgent;
        private Rigidbody m_rigidBody;
        private Vector3 mv3_TargetDestination;
        private Actions me_Action = Actions.Seek;
        
        private void Awake() {
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_rigidBody = GetComponent<Rigidbody>();
            
            me_Action = Actions.Idle;
        }

        private void Start() {
            Init(transform.position);
        }

        private void Update() {
            if (m_NavMeshAgent.remainingDistance <= mf_AttackRange) {
                m_NavMeshAgent.ResetPath();
            }
                
            switch (me_Action) {
                case Actions.Idle:
                    break;
                case Actions.Seek:
                    break;
                case Actions.MeleeAttack:
                    break;
                case Actions.RangedAttack:
                    break;
            }
        }

        public void SetBehaviour(int ai_behaviourID) {
            me_Action = ai_behaviourID switch {
                1 => Actions.Seek,
                2 => Actions.MeleeAttack,
                3 => Actions.RangedAttack,
                _ => Actions.Idle
            };
        }
        
        private void Init(Vector3 av3_Position) {
            m_NavMeshAgent.Warp(av3_Position);
        }
        
        private void UpdateAgentDestination(Vector3 av3_Destination) 
            => m_NavMeshAgent.SetDestination(av3_Destination);

        public void SetTarget(GameObject ago_Target) {
            UpdateAgentDestination(ago_Target.transform.position);
            me_Action = Actions.Seek;
        }

        public void SetTarget(Vector3 av3_Target) {
            UpdateAgentDestination(av3_Target);
            me_Action = Actions.Seek;
        }
    }
}