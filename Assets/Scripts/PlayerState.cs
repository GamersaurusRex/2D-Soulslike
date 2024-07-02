using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public enum State { Idle, Walking, Running, Jumping, Dodging }
    public State currentState;

    void Start()
    {
        currentState = State.Idle;
    }

    void Update()
    {
        
    }
}
