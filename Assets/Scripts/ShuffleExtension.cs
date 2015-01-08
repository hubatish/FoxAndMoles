using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class MyExtensions
{

	public static void Shuffle<T>(this IList<T> list)  
	{  
		int n = list.Count;  
		while (n > 1) {  
			n--;  
			int k = Random.Range(0,n + 1);  
			T value = list[k];  
			list[k] = list[n];  
			list[n] = value;  
		}  
	}
	
	/*public static IList<T> Randomize<T>(IList<T> list)
    {
        IList<T> randomizedList = new List<T>();
        while (list.Count > 0)
        {
            int index = Random.Range(0, list.Count); //pick a random item from the master list
            randomizedList.Add(list[index]); //place it at the end of the randomized list
            list.RemoveAt(index);
        }
        return randomizedList;
    }*/
	
}