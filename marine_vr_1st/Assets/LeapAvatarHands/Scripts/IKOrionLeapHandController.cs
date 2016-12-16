/**
IKOrionLeapHandController
Extends the Leap Motion orion "LeapHandController" class to enable the use of a game avatar characters
own hands instead of disembodied virtual hands when using the Leap Motion controller.

Author: Ivan Bindoff
*/

using UnityEngine;
using Leap.Unity;
using System.Collections.Generic;

public class IKOrionLeapHandController : LeapHandController {
    protected Animator animator;
    private GameObject rootNode;							//the root, parent, gameobject that this script ultimately lives on.
    public bool ikActive = true;
    public bool leftActive = false;
    public bool rightActive = false;

    public GameObject avatarLeftHand;   //keeps track of the left hand gameobject for IK
    public GameObject avatarRightHand;  //keeps track of the right hand gameobject for IK
    public RiggedHand leftHand;
    public RiggedHand rightHand;
        
    [HideInInspector]
    public Vector3 lTarget;             //where we want the left hand to be in worldspace
    [HideInInspector]
    public Vector3 rTarget;                 //where we want the right hand to be in worldspace
    [HideInInspector]
    public Quaternion lTargetRot;           //the rotation we want for left hand
    [HideInInspector]
    public Quaternion rTargetRot;			//the rotation we want for the right hand

    protected float leftInactiveWeight = 0f;
    protected float rightInactiveWeight = 0f;
    public float inactiveLerpTime = 0.2f;	//how many seconds it takes to lerp back to default pose after a hand goes inactive

    private float lWeight = 0f;         //how much IK to apply to the left hand
    private float rWeight = 0f;			//how much IK to apply to the right hand

    public RigidHand leftPhysicsHand;
    public RigidHand rightPhysicsHand;

    protected virtual void Awake()
    {
        //find the Animator somewhere in this gameobject's hierarchy
        Transform t = transform;
        while (t != null)
        {
            if (t.gameObject.GetComponent<Animator>())
            {
                animator = t.gameObject.GetComponent<Animator>();
                rootNode = t.gameObject;
                break;
            }
            t = t.parent;
        }
        if (animator == null)
        {
            Debug.LogError("IKOrionLeapHandController:: no animator found on GameObject or any of its parent transforms. Are you sure you added IKLeapHandController to a properly defined rigged avatar?");
        }

        if (leftHand == null)
            Debug.LogError("IKOrionLeapHandController::Awake::No Rigged Hand set for left hand parameter. You have to set this in the inspector.");
        if (rightHand == null)
            Debug.LogError("IKOrionLeapHandController::Awake::No Rigged Hand set for right hand parameter. You have to set this in the inspector.");

        
    }

    void Update()
    {
        if (graphicsEnabled)
        {
            UpdateHandRepresentations();
        }
    }

    
    /**
	 * When an IK pass happens, this happens.
	 * 
	 * This handles the IK for the hands
	 * 
	 * IMPORTANT NOTE: You need to have IK pass turned on in the animator or this will never happen,
	 * and your hands won't work properly! They will have horrible stretchy wrists at best.
	 * */
    public void OnAnimatorIK()
    {
        //Debug.Log ("Receiving message");
        //if we're using the leap controller then do the animation IK override
        if (animator)
        {
            //if the IK is active, set the position and rotation directly to the goal. 
            if (ikActive)
            {
                //find the LEAP hands and set them as the target
                setHandTargets();


                //set the position and the rotation of the left hand where the leap hand is
                if (leftActive && leftHand != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, lWeight);
                    animator.SetIKPosition(AvatarIKGoal.LeftHand, lTarget);
                    animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, lWeight);
                    animator.SetIKRotation(AvatarIKGoal.LeftHand, lTargetRot);
                    leftInactiveWeight = 1f;
                }
                else
                {
                    //gradually lerp away from the target, returning hand back to normal operation but without a jarring effect
                    leftInactiveWeight -= Time.deltaTime / inactiveLerpTime;
                    if (leftInactiveWeight < 0f)
                    {
                        leftInactiveWeight = 0f;
                        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftInactiveWeight);
                        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, leftInactiveWeight);
                    }
                    else
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, leftInactiveWeight);
                        animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0f);
                        animator.SetIKPosition(AvatarIKGoal.LeftHand, lTarget);
                        animator.SetIKRotation(AvatarIKGoal.LeftHand, lTargetRot);
                    }
                }

                //set the position and the rotation of the right hand where the leap hand is
                if (rightActive && rightHand != null)
                {
                    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rWeight);
                    animator.SetIKPosition(AvatarIKGoal.RightHand, rTarget);
                    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rWeight);
                    animator.SetIKRotation(AvatarIKGoal.RightHand, rTargetRot);
                    rightInactiveWeight = 1f;
                }
                else
                {
                    //gradually lerp away from the target, returning hand back to normal operation
                    rightInactiveWeight -= Time.deltaTime / inactiveLerpTime;
                    if (rightInactiveWeight < 0f)
                    {
                        rightInactiveWeight = 0f;
                        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightInactiveWeight);
                        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, rightInactiveWeight);
                    }
                    else
                    {
                        animator.SetIKPositionWeight(AvatarIKGoal.RightHand, rightInactiveWeight);
                        animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0f);
                        animator.SetIKPosition(AvatarIKGoal.RightHand, rTarget);
                        animator.SetIKRotation(AvatarIKGoal.RightHand, rTargetRot);
                    }
                }
            }
            else
            {
                //IK not active, resume normal operation
                animator.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
                animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
                animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            }
        }
    }

    /**
	 * Set the current Leap hands as the IK targets
	 * 
	 * */
    private void setHandTargets()
    {
        if (leftActive && leftHand != null)
        {
            lTarget = leftHand.GetWristPosition();
            lTargetRot = leftHand.GetPalmRotation();
            lWeight = 1f;
            //Debug.Log ("left hand target set to " + lTarget);
        }

        if (rightActive && rightHand != null)
        {
            rTarget = rightHand.GetWristPosition();
            rTargetRot = rightHand.GetPalmRotation();
            rWeight = 1f;
        }
    }

    /// <summary>
    /// Tells the hands to update to match the new Leap Motion hand frame data. Also keeps track of
    /// which hands are currently active.
    /// </summary>
    void UpdateHandRepresentations()
    {
        leftActive = false;
        rightActive = false;
        foreach (Leap.Hand curHand in Provider.CurrentFrame.Hands)
        {
            if (curHand.IsLeft)
            {
                if(leftHand.GetController() != this)
                    leftHand.SetController(this);
                leftHand.SetLeapHand(curHand);
                leftHand.UpdateHand();

                leftActive = true;
            }
            if(curHand.IsRight)
            {
                if (rightHand.GetController() != this)
                    rightHand.SetController(this);
                rightHand.SetLeapHand(curHand);
                rightHand.UpdateHand();
                rightActive = true;
            }
        }
    }

    /// <summary>
    /// Tells the physics hands to update to match the new leap motion hand frame data.
    /// </summary>
    void UpdatePhysicsHandRepresentations()
    {
        leftActive = false;
        rightActive = false;
        
        foreach (Leap.Hand curHand in Provider.CurrentFrame.Hands)
        {
            if (curHand.IsLeft)
            {
                leftPhysicsHand.gameObject.SetActive(true);
                if (leftPhysicsHand.GetController() != this)
                    leftPhysicsHand.SetController(this);
                leftPhysicsHand.SetLeapHand(curHand);
                leftPhysicsHand.UpdateHand();

                leftActive = true;
            }
            if (curHand.IsRight)
            {
                rightPhysicsHand.gameObject.SetActive(true);
                if (rightPhysicsHand.GetController() != this)
                    rightPhysicsHand.SetController(this);
                rightPhysicsHand.SetLeapHand(curHand);
                rightPhysicsHand.UpdateHand();
                rightActive = true;
            }
        }
        leftPhysicsHand.gameObject.SetActive(leftActive);
        rightPhysicsHand.gameObject.SetActive(rightActive);
    }

    /** Updates the physics objects */
    protected override void FixedUpdate()
    {
        if (leftPhysicsHand != null || rightPhysicsHand != null)
        {
            UpdatePhysicsHandRepresentations();
            //UpdateHandModels(hand_physics_, frame.Hands, leftPhysicsModel, rightPhysicsModel);
        }
    }
}
