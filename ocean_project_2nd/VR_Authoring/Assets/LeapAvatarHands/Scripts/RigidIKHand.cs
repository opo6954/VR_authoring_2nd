/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap.Unity;

// The model for our rigid hand made out of various polyhedra.
public class RigidIKHand : RigidHand {
	public float offset = 0.1f;
	public HandModel targetHand;

    /*public override void UpdateHand() {
		//skip the update if there's any unset parameters (i.e. we're not initialised properly yet)
		if(targetHand == null || controller_ == null || targetHand.GetController() == null || hand_ == null || controller_.transform == null || hand_.Direction == null)
		{
			return;
		}
		
		Vector3 offs = targetHand.GetPalmDirection () * offset;
		
		for (int f = 0; f < fingers.Length; ++f) {
			if (fingers[f] != null)
			{
				fingers[f].SetOffset (offs);
				fingers[f].UpdateFinger();
			}
		}
		
		if (palm != null) {
			// Set palm velocity.
			Vector3 target_position = targetHand.GetPalmPosition() + (offs);
			palm.GetComponent<Rigidbody>().velocity = (target_position - palm.transform.position) *
				(1 - filtering) / Time.deltaTime;
			
			// Set palm angular velocity.
			Quaternion target_rotation = GetPalmRotation();
			Quaternion delta_rotation = target_rotation *
				Quaternion.Inverse(palm.transform.rotation);
			float angle = 0.0f;
			Vector3 axis = Vector3.zero;
			delta_rotation.ToAngleAxis(out angle, out axis);
			
			if (angle >= 180) {
				angle = 360 - angle;
				axis = -axis;
			}
			if (angle != 0) {
				float delta_radians = (1 - filtering) * angle * Mathf.Deg2Rad;
				palm.GetComponent<Rigidbody>().angularVelocity = delta_radians * axis / Time.deltaTime;
			}
		}
		
		if (forearm != null)
		{
			// Set arm velocity.
			Vector3 target_position = GetArmCenter() + offs;
			forearm.GetComponent<Rigidbody>().velocity = (target_position - forearm.transform.position) *
				(1 - filtering) / Time.deltaTime;
			
			// Set arm velocity.
			Quaternion target_rotation = GetArmRotation();
			Quaternion delta_rotation = target_rotation *
				Quaternion.Inverse(forearm.transform.rotation);
			float angle = 0.0f;
			Vector3 axis = Vector3.zero;
			delta_rotation.ToAngleAxis(out angle, out axis);
			
			if (angle >= 180)
			{
				angle = 360 - angle;
				axis = -axis;
			}
			if (angle != 0)
			{
				float delta_radians = (1 - filtering) * angle * Mathf.Deg2Rad;
				forearm.GetComponent<Rigidbody>().angularVelocity = delta_radians * axis / Time.deltaTime;
			}
		}
	}*/

    public override void UpdateHand()
    {
        //skip the update if there's any unset parameters (i.e. we're not initialised properly yet)
        if (targetHand == null || controller_ == null || targetHand.GetController() == null || hand_ == null || controller_.transform == null || hand_.Direction == null)
        {
            return;
        }

        Vector3 offs = targetHand.GetPalmDirection() * offset;

        for (int f = 0; f < fingers.Length; ++f)
        {
            if (fingers[f] != null)
            {
                fingers[f].SetOffset(offs);
                fingers[f].UpdateFinger();
            }
        }

        if (palm != null)
        {
            Rigidbody palmBody = palm.GetComponent<Rigidbody>();
            if (palmBody)
            {
                palmBody.MovePosition(GetPalmCenter()+offs);
                palmBody.MoveRotation(GetPalmRotation());
            }
            else
            {
                palm.position = GetPalmCenter()+offs;
                palm.rotation = GetPalmRotation();
            }
        }

        if (forearm != null)
        {
            // Set arm dimensions.
            CapsuleCollider capsule = forearm.GetComponent<CapsuleCollider>();
            if (capsule != null)
            {
                // Initialization
                capsule.direction = 2;
                forearm.localScale = new Vector3(1f, 1f, 1f);

                // Update
                capsule.radius = GetArmWidth() / 2f;
                capsule.height = GetArmLength() + GetArmWidth();
            }

            Rigidbody forearmBody = forearm.GetComponent<Rigidbody>();
            if (forearmBody)
            {
                forearmBody.MovePosition(GetArmCenter());
                forearmBody.MoveRotation(GetArmRotation());
            }
            else
            {
                forearm.position = GetArmCenter();
                forearm.rotation = GetArmRotation();
            }
        }
    }
}
