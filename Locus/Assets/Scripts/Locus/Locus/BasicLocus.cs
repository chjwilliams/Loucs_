using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChrsUtils;

using ChrsUtils.PrefabDataBase;
using ChrsUtils.ChrsExtensionMethods;
using ChrsUtils.ChrsManagerSystem.SimpleManager;
using ChrsUtils.BehaviorTree;
using LocusNodes;
using LocusIManager;

namespace Locus
{
	public enum LocusType
	{
        Mela,	// Earth Avoid
        Phle,	//	Water Getting
        Sang,	//	Air Social
        Chol,	//	Fire Ruling
	}

	public class BasicLocus : MonoBehaviour,  IManaged
	{
		public static int zoneSize = 10;

		public bool isFlocking;
		public bool isWandering;
		public bool inDanger;
		public float maxSpeed;
		public float wanderSpeed;
		public float maxForce;
		public float visibilityRange;
		public float fleeTimer;
		public float safetyDistance;
		public float pounceDistance;
		
		public float neighborDistance;
		public float rotationSpeed;
		public float dimIntensity = 2.0f;
		public float brightIntensity = 4.0f;
		public int groupSize;
		public int idealGroupSize;
		public AudioClip[] clips = new AudioClip[2];
		public Transform target;
		public Transform dangerousEntity;
		public Color[] colorPalette = new Color[3];
		public Light spotLight;
		public EasingProperties easing;

		protected Vector3 newTargetPosition = Vector3.zero;
		protected Vector3 velocity;
		protected Vector3 acceleration;
		protected AudioSource m_AudioSource;
		protected const string AUDIO_FILE_PATH = "Audio/";
		protected const string BELL_DRUM = "BellDrum";
		protected const string ARPEG_1 = "arpeg1";
		protected const string ARPEG_2 = "arpeg2";
		protected AudioSource _audioSource;
		protected Rigidbody m_Rigidbody;
		protected Tree<BasicLocus> m_Tree;
		protected LociManager myManager;

	

		// Use this for initialization
		protected virtual void Start () 
		{
			spotLight = GetComponentInChildren<Light>();
			easing = ScriptableObject.CreateInstance("EasingProperties") as EasingProperties;
			_audioSource = GetComponent<AudioSource>();
			clips[0] = Resources.Load(AUDIO_FILE_PATH + ARPEG_1) as AudioClip;
			clips[1] = Resources.Load(AUDIO_FILE_PATH + ARPEG_2) as AudioClip;

			spotLight.intensity = dimIntensity;
			maxSpeed = Random.Range(7.0f, 15.0f);
			maxForce = Random.Range(5.0f, 8.0f);
			visibilityRange = Random.Range(1.0f, 4.0f);
			m_Rigidbody = GetComponent<Rigidbody>();
			wanderSpeed  = maxSpeed * 0.5f;
			myManager = Services.LociManager;
			neighborDistance = Random.Range(1.0f, 3.0f);
			rotationSpeed = Random.Range(1.0f, 4.0f);


			idealGroupSize = Random.Range(0, 20);

			colorPalette[0] = new Color(0.337f, 0.467f, 0.78f);
			colorPalette[1] = new Color(0.251f, 0.498f, 0.498f);
			colorPalette[2] = new Color(0.667f, 0.224f, 0.224f);

			int color = Random.Range(0, 10) % 3;

			spotLight.color = colorPalette[color];
			GetComponent<Renderer>().material.color = colorPalette[color];
		}

		public void OnCreated()
		{

		}

		public void OnDestroyed()
		{

		}

		public bool DangerCheck()
		{
			return Vector3.Distance(dangerousEntity.position, transform.position) < visibilityRange? true : false;
		}

		public bool FeelingConfident()
		{
			return idealGroupSize < groupSize? true: false;
		}

		IEnumerator FadeInLight()
		{
			yield return StartCoroutine(Coroutines.DoOverEasedTime(1.0f, easing.FadeInEasing, t =>
        	{
				float intensity = Mathf.Lerp(dimIntensity, brightIntensity, t * 5.0f);
				spotLight.intensity = intensity;
				StartCoroutine(FadeOutLight());
			}));	
		}

		IEnumerator FadeOutLight()
		{
			yield return StartCoroutine(Coroutines.DoOverEasedTime(1.0f, easing.FadeOutEasing, t =>
        	{
				float intensity = Mathf.Lerp(brightIntensity, dimIntensity, t * 5.0f);
				spotLight.intensity = intensity;
			}));	
		}

		IEnumerator FadeOutAudio()
		{
			yield return StartCoroutine(Coroutines.DoOverEasedTime(1.0f, easing.FadeOutEasing, t =>
        	{
				float volume = Mathf.Lerp(0.2f, 0, t * 0.01f);
				_audioSource.volume = volume;
			}));
		}

		IEnumerator FadeInAudio()
		{
			yield return StartCoroutine(Coroutines.DoOverEasedTime(1.0f, easing.FadeInEasing, t =>
        	{
				float volume = Mathf.Lerp(0, 0.3f, t * 0.01f);
				_audioSource.volume = volume;
			}));
		}

		public void RandomizeTarget()
		{
			while(target == null || target == dangerousEntity)
			{
				if(Random.Range(0, 10) < 1)
				{
					target = myManager.managedObjects[Random.Range(0, myManager.managedObjects.Count)].transform;
				}
				else
				{
					target = GameObject.FindGameObjectWithTag("Player").transform;
				}
			}
		}

		protected virtual void OnCollisionEnter(Collision col)
		{
			if (col.gameObject.tag == "Loci" || col.gameObject.tag == "Wall" )
			{
				if(!_audioSource.isPlaying)
				{
					_audioSource.pitch = Mathf.PerlinNoise(0, 5);
					_audioSource.PlayOneShot(clips[Random.Range(0, 10) % 2], Random.Range(0.1f, 0.15f));
				}
				StartCoroutine(FadeInLight());
			}
		}

		protected void Move(Vector3 direction)
		{
			Vector3 movement = new Vector3(direction.x, 0, direction.z);
			m_Rigidbody.AddForce(movement, ForceMode.Impulse);
		}

		public virtual void Seek(Vector3 targetPosition)
		{
			Vector3 desiredVelocity = targetPosition - transform.position;
			float d = desiredVelocity.magnitude;
			desiredVelocity.Normalize();

			if(d < pounceDistance)
			{
				float m = ExtensionMethods.Map(d, 0, pounceDistance, 0, maxSpeed);
				desiredVelocity = desiredVelocity * m;
			}
			else
			{
				if(isWandering)
				{
					desiredVelocity = desiredVelocity * wanderSpeed;
				}
				else
				{
					desiredVelocity = desiredVelocity * maxSpeed;
				}
			}

			Vector3 steeringForce = desiredVelocity - velocity;

			Vector3 direction = Vector3.ClampMagnitude(steeringForce, maxForce);

			Move(direction);
		}

		public virtual void Wander(Vector3 currentPosition, float angle)
		{
			if(Random.Range(0, 10000) < 100)
			{
				newTargetPosition = new Vector2(Random.Range(-zoneSize, zoneSize), Random.Range(-zoneSize, zoneSize));
				Vector3 predictedPoint = new Vector3(transform.position.x, transform.position.y, transform.position.z) + m_Rigidbody.velocity * Time.deltaTime;
				Seek(new Vector3(newTargetPosition.x, 0, newTargetPosition.y));
			}
		}

		public virtual void Flee(Vector3 targetPosition)
		{
			Vector3 desiredVelocity = transform.position - targetPosition;
			float d = desiredVelocity.magnitude;
			desiredVelocity.Normalize();

			if(d < pounceDistance)
			{
				float m = ExtensionMethods.Map(d, 0, pounceDistance, 0, maxSpeed);
				desiredVelocity = desiredVelocity * m;
			}
			else
			{
				desiredVelocity = desiredVelocity * maxSpeed;
			}

			Vector3 steeringForce = desiredVelocity - velocity;

			Vector3 direction = Vector3.ClampMagnitude(steeringForce, maxForce);

			Move(direction);
		}

		public virtual void Flock()
		{
			float moveSpeed = 0.1f;
			
			Vector3 averageHeadingDirection;
			Vector3 averagePostion;
			

		}

		public void ApplyFlockingRules()
		{
			Vector3 centerOfGroup = Vector3.zero;
			Vector3 avoidance = Vector3.zero;
			float groupSpeed = 1.0f;

			float distance;
			newTargetPosition = centerOfGroup;
			groupSize = 0;
			foreach(BasicLocus loci in myManager.managedObjects)
			{
				if(loci.gameObject != this.gameObject)
				{
					distance = Vector3.Distance(loci.transform.position, transform.position);
					if(distance <= neighborDistance)
					{
						centerOfGroup += loci.transform.position;
						groupSize++;

						if(distance < 0.5f)
						{
							avoidance =  avoidance + (transform.position - loci.transform.position);
						}

						groupSpeed =  groupSpeed + loci.velocity.magnitude;
					}
				}
			}

			if (groupSize > 0)
			{
				isFlocking = true;
				centerOfGroup  = centerOfGroup / groupSize + (newTargetPosition - transform.position);
				maxSpeed = groupSpeed / groupSize;

				Vector3 direction = (centerOfGroup + avoidance) - transform.position;
				
				if(direction != Vector3.zero)
				{
					transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), rotationSpeed * Time.deltaTime);
				}
			}
			else
			{
				isFlocking = false;
			}
		}

		protected virtual void FixedUpdate () 
		{
			velocity += acceleration;
			Vector3.ClampMagnitude(velocity, maxSpeed);
			acceleration = Vector3.zero;


			ApplyFlockingRules();
			// float wanderMod = Mathf.PerlinNoise(0, Time.timeSinceLevelLoad) * 360f;
			// isWandering = true;
			// Wander(transform.position, wanderMod);
			m_Tree.Update(this);
		}
	}
}