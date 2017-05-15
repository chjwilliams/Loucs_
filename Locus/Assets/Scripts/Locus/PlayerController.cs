using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChrsUtils;
using ChrsUtils.EasingEquations;

public class PlayerController : MonoBehaviour 
{
	public float moveSpeed = 20.0f;
	public float dimIntensity = 2.0f;
	public float brightIntensity = 4.0f;
	public EasingProperties easing;
	public AudioClip clip;

	public Light spotLight;
	private Rigidbody _rigidbody;
	private const string Y_AIXS = "Vertical";
	private const string X_AXIS = "Horizontal";
	public const string AUDIO_FILE_PATH = "Audio/";
	private const string MALLETS = "Mallets";
	private AudioSource _audioSource;

	// Use this for initialization
	void Start () 
	{
		spotLight = GetComponentInChildren<Light>();
		_rigidbody = GetComponent<Rigidbody>();
		_audioSource = GetComponent<AudioSource>();
		clip = Resources.Load(AUDIO_FILE_PATH + MALLETS) as AudioClip;

		spotLight.intensity = dimIntensity;
		easing = ScriptableObject.CreateInstance("EasingProperties") as EasingProperties;
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

	void OnCollisionEnter(Collision col)
	{
		if (col.gameObject.tag == "Loci" || col.gameObject.tag == "Wall" )
		{
			if(!_audioSource.isPlaying)
			{
				_audioSource.pitch = Random.Range(0.8f, 1.2f);
				_audioSource.PlayOneShot(clip, Random.Range(0.5f, 0.85f));
			}
			StartCoroutine(FadeInLight());
		}
	}

	private void Move(float dx, float dy)
	{
		Vector3 movement = new Vector3(dx, 0, dy) ;
		_rigidbody.AddForce(movement * moveSpeed * Time.deltaTime, ForceMode.Impulse);
	}
	
	// Update is called once per frame
	void Update () 
	{
		float y = Input.GetAxis(Y_AIXS);
		float x = Input.GetAxis(X_AXIS);

		Move(x,y);
	}
}
