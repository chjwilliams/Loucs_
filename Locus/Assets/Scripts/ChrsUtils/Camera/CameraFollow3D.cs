using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ChrsUtils
{
	namespace ChrsCamera
	{
		/*--------------------------------------------------------------------------------------*/
		/*																						*/
		/*	CameraCollow3D: Camera movement script												*/
		/*																						*/
		/*		Functions:																		*/
		/*			Start ()																	*/
		/*			FindTarget ()																*/
		/*			LateUpdate ()																	*/
		/*																						*/
		/*--------------------------------------------------------------------------------------*/
		public class CameraFollow3D : MonoBehaviour 
		{
			public const string PLAYER_TAG = "Player";

			//	Public Variables
			public float nextTimeToSearch = 0;				//	How long unitl the camera searches for the target again
			public float yPosBoundary;						//	The highest the camera can go in the y direction
			public float yNegBoundary;						//	The lowest the camera can go in the y direction
			public float xPosBoundary;						//	The furthest the camera can go in the x direction
			public float xNegBoundary;						//	The lowest the camera can go in the x direction
			public float nearBoundary;						//	How close the camera can come towards the player
			public float farBoundary;						//	How far the camera go go away form the player
			public Transform target;						//	What the camera is fixed on
			//	Private Variables
			private Vector3 offset;

			void Start()
			{
				offset = transform.position;
				target = GameObject.FindGameObjectWithTag(PLAYER_TAG).transform;				
			}

			void FindTarget()
			{
				if (nextTimeToSearch <= Time.time)
				{
					GameObject result = GameObject.FindGameObjectWithTag ("Player");
					if (result != null)
						target = result.transform;

					nextTimeToSearch = Time.time + 2.0f;
				}
			}

			void LateUpdate()
			{
				if (target == null)
				{
					FindTarget ();
					return;
				}

				transform.position = Vector3.Lerp(transform.position, target.position + offset, 0.9f);

				if (transform.position.x > xPosBoundary) 
				{
					transform.position = new Vector3(xPosBoundary, transform.position.y, transform.position.z);
				}

				if (transform.position.x < xNegBoundary) 
				{
					transform.position = new Vector3(xNegBoundary, transform.position.y, transform.position.z);
				}

				if (transform.position.y > yPosBoundary) 
				{
					transform.position = new Vector3(transform.position.x, yPosBoundary, transform.position.z);
				}

				if (transform.position.y < yNegBoundary) 
				{
					transform.position = new Vector3(transform.position.x, yNegBoundary, transform.position.z);
				}

				if (transform.position.z > farBoundary) 
				{
					transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
				}

				if (transform.position.z < nearBoundary) 
				{
					transform.position = new Vector3(transform.position.x, transform.position.y, nearBoundary);
				}
				
			}
		}
	}
}
