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

    const float MIN_X = 0.0f;
    const float MAX_X = 360.0f;

    Rigidbody rBody;

    public Transform endGoal;
    public float forceMultiplier = .1f;

    void Start()
    {
        rBody = GetComponent<Rigidbody>();
    }
    public override void OnEpisodeBegin()
    {
        // If the Agent fell, zero its momentum

        this.rBody.angularVelocity = Vector3.zero;
        this.rBody.velocity = Vector3.zero;
        this.transform.localPosition = new Vector3(6f, 1f, 4f);
  
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
        rBody.transform.Translate(controlSignal * forceMultiplier);

        var rotateDir = Vector3.zero;
        //var rotate = Mathf.Clamp(actionBuffers.ContinuousActions[2], -1f, 1f);
        var rotate = actionBuffers.ContinuousActions[2];
        rotateDir = -transform.up * rotate;
        rotateX += rotate;
        rBody.transform.rotation = Quaternion.Euler(0.0f, rotateX, 0.0f);


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

        transform.rotation = Quaternion.Euler(0.0f, rotateX, 0.0f);
        rBody.transform.Translate(movement * forceMultiplier);

        AddReward(-.00001f);

        float distanceToTarget = Vector3.Distance(this.transform.localPosition, endGoal.localPosition);
        if (this.transform.localPosition.y < 0)
        {
            //SetReward(-1f);
            EndEpisode();
        }


        if (distanceToTarget <= 1f)
        {
            SetReward(1f);
            EndEpisode();
        }

    }

    //public override void Heuristic(in ActionBuffers actionsOut)
    //{
    //    var continuousActionsOut = actionsOut.ContinuousActions;
    //    continuousActionsOut[0] = movementX;
    //    continuousActionsOut[1] = movementY;
    //}
}
