using UnityEngine;
using DandI.Utils;
using UnityEngine.AI;

public class Aniemy : MonoBehaviour
{
    [SerializeField] private State statrtingState;
    [SerializeField] private float roamingDistanceMax = 10f;
    [SerializeField] private float roamingDistanceMin = 5f;
    [SerializeField] private float roamingTimerMax = 5f;

    private NavMeshAgent NavMeshAgent;
    private State state;
    private float roamingTime;
    private Vector3 roamPsition;
    private Vector3 startingPosition;


    private enum State{
        Idle,
        Roaming
    }

    private void Awake() {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        NavMeshAgent.updateRotation = false;
        NavMeshAgent.updateUpAxis = false;
        state = statrtingState;
        startingPosition = transform.position;
    }


    private void Update() {
        switch (state) {
            default:
            case State.Idle:
                break;
            case State.Roaming:
                roamingTime -= Time.deltaTime;
                if (roamingTime < 0){
                    Roaming();
                    roamingTime = roamingTimerMax;
                }
                break;
        }
    }


    private void Roaming() {
        roamPsition = GetRoamingPosition();
        NavMeshAgent.SetDestination(roamPsition);
    }

    private Vector3 GetRoamingPosition() {
        return startingPosition + Dand.GetRandomDir() * Random.Range(roamingDistanceMin, roamingDistanceMax);
    }
}
