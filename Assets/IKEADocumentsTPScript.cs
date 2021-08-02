using KeepCoding;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IKEADocumentsTPScript : TPScript<IKEADocumentsScript>
{
	public override IEnumerator ForceSolve()
	{
		if (Module.IsSolved)
			yield break;

		yield return null;

		yield return StartCoroutine(NavigateToIndex(Module._correctIndex));
	}

	private IEnumerator NavigateToIndex(int index)
	{
		while (Module._cycleIndex != index)
		{
			Module.CycleRight();
			yield return new WaitForSecondsRealtime(0.1f);
		}

		Module.Submit();
	}

	private bool MatchName(string name, string input)
	{
		return string.Equals(name, input, StringComparison.InvariantCultureIgnoreCase);
	}

	public override IEnumerator Process(string command)
	{
		int index = Array.FindIndex(Module._manuals, m => MatchName(m.Name, command) || m.TwitchNames.Any(n => MatchName(n, command)));
		if (index < 0)
		{
			yield return SendToChatError("Invalid document name!");
			yield break;
		}

		yield return null;
		yield return StartCoroutine(NavigateToIndex(index));
	}
}
