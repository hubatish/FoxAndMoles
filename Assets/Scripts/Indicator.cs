using UnityEngine;
using System.Collections;

public class Indicator : MonoBehaviour {

	public GameObject indicatorArrowPrefab;
	private GameObject instantiatedArrow = null;
	
	public void SpawnIndicator(Vector3 pos, Quaternion rot)
	{
		TryToDestroyArrow();
		instantiatedArrow = (GameObject) Instantiate(indicatorArrowPrefab,pos,rot);
	}
	
	public void TryToDestroyArrow()
	{
		if(instantiatedArrow!=null)
		{
			GameObject.Destroy(instantiatedArrow);
			instantiatedArrow = null;
		}
	}
	
}