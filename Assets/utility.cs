using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class utility
{
	public static int chooseRandomSign()
	{
		int[] choices = { -1, 1 };
		return choices[Random.Range(0, 2)];          //Random.Range is exclusive of max, so gives either 0 or 1, corresponding to index of choices[]
	}
}
