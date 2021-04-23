using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using UnityEngine.InputSystem;

public class MovingAgent : Agent
{

    private float movementX;
    private float movementY;
    private float rotateX;
    mazegen maze;

    const float MIN_X = 0.0f;
    const float MAX_X = 360.0f;

    Rigidbody rBody;

    public Transform endGoal;
    public float forceMultiplier = 3f;

    public GameObject area;

    public override void Initialize()
    {
        rBody = GetComponent<Rigidbody>();
        maze = area.GetComponent<mazegen>();
    }
    public override void OnEpisodeBegin()
    {
        maze.EnvironmentReset();

        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(1f, .25f, 0f);
  
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // Target and Agent positions
        sensor.AddObservation(this.transform.localPosition);

        // Agent velocity
        sensor.AddObservation(rBody.velocity.x);
        sensor.AddObservation(rBody.velocity.z);
    }


    public override void OnActionReceived(ActionBuffers actionBuffers)
    {
        // Actions, size = 2
        //Vector3 controlSignal = Vector3.zero;
        //controlSignal.x = actionBuffers.ContinuousActions[0];
        //controlSignal.z = actionBuffers.ContinuousActions[1];
        //rBody.AddForce(controlSignal * forceMultiplier);
        Vector3 controlSignal = Vector3.zero;
        controlSignal.x = actionBuffers.ContinuousActions[0];
        controlSignal.z = actionBuffers.ContinuousActions[1];
        //controlSignal.y = 0;
        //Vector3 movement = new Vector3(actionBuffers.ContinuousActions[0], 0.0f,actionBuffers.ContinuousActions[1]);
        rBody.AddForce(controlSignal * forceMultiplier);

        /*var rotateDir = Vector3.zero;
        //var rotate = Mathf.Clamp(actionBuffers.ContinuousActions[2], -1f, 1f);
        var rotate = actionBuffers.ContinuousActions[2];
        rotateDir = -transform.up * rotate;
        rotateX += rotate;
        rBody.transform.rotation = Quaternion.Euler(0.0f, rotateX, 0.0f);*/


        float distanceToTarget = Vector3.Distance(this.transform.localPosition, endGoal.localPosition);
        if (this.transform.localPosition.y < 0)
        {
            SetReward(-1f);
            EndEpisode();
        }


        if (distanceToTarget <= .8f)
        {
            SetReward(1f);
            EndEpisode();
        }


    }

    private void OnMove(InputValue movementValue)
    {
        Vector2 movementVector = movementValue.Get<Vector2>();

        movementX = movementVector.x;
        movementY = movementVector.y;
    }

    private void OnLook(InputValue lookValue)
    {


        Vector2 rotationVector = lookValue.Get<Vector2>();

        rotateX += rotationVector.x;

        if (rotateX < MIN_X) rotateX += MAX_X;
        else if (rotateX > MAX_X) rotateX -= MAX_X;

    }

    private void FixedUpdate()
    {
        Vector3 movement = new Vector3(movementX, 0.0f, movementY);

        //transform.rotation = Quaternion.Euler(0.0f, rotateX, 0.0f);
        rBody.AddForce(movement * forceMultiplier);

        AddReward(-.00001f);

        if(rBody.velocity.x == 0 && rBody.velocity.y == 0)
        {
            AddReward(-0.00001f);
        }

        float distanceToTarget = Vector3.Distance(this.transform.localPosition, endGoal.localPosition);
        if (this.transform.localPosition.y < 0)
        {
            SetReward(-1f);
            EndEpisode();
        }


        if (distanceToTarget <= .6f)
        {
            SetReward(1f);
            EndEpisode();
        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "wall")
        {
            AddReward(-0.00001f);
        }
    }

    //public override void Heuristic(in ActionBuffers actionsOut)
    //{
    //    var continuousActionsOut = actionsOut.ContinuousActions;
    //    continuousActionsOut[0] = movementX;
    //    continuousActionsOut[1] = movementY;
    //}
}
