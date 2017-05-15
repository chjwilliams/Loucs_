using UnityEngine;

namespace GameScenes
{
	public class Scene<TTransitionData> : MonoBehaviour
	{

		public GameObject Root { get { return gameObject;	}	}

		internal void _OnEnter(TTransitionData data)
		{
			Root.SetActive(true);
			OnEnter(data);
		}

		internal void _OnExit()
		{
			Root.SetActive(false);
			OnExit();
		}

		internal virtual void OnEnter(TTransitionData data) { }
		internal virtual void OnExit() { }
		//	internal virtual void Init() { }
		//	internal virtual void Destroy() { }
		//	internal virtual void Update() { }
		//	internal virtual void Tick () { }
		//	internal virtual void Render() { }
		
		
	}
}