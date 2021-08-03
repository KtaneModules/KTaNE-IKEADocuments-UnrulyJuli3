using KeepCoding;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IKEADocumentsScript : ModuleScript
{
	[SerializeField]
	internal IKEAManual[] _manuals;
	[SerializeField]
	private KMSelectable _leftArrow;
	[SerializeField]
	private KMSelectable _rightArrow;
	[SerializeField]
	private KMSelectable _submitButton;
	[SerializeField]
	private Renderer _pageRenderer;
	[SerializeField]
	private TextMesh _displayText;
	[SerializeField]
	private Texture2D _solveTexture;

	internal int _correctIndex;
	internal int _cycleIndex;

	private void Start()
	{
		_manuals = _manuals.Randomize().ToArray();
		_correctIndex = Random.Range(0, _manuals.Length);
		Texture2D page = _manuals[_correctIndex].Pages.PickRandom();
		_pageRenderer.material.mainTexture = page;
		Log("Selected: Page {0} of {1} ({2})", page.name.Split('_').Last(), _manuals[_correctIndex].Name, page.name);

		CycleDirection(0);

		_leftArrow.Assign(onInteract: CycleLeft);
		_rightArrow.Assign(onInteract: CycleRight);
		_submitButton.Assign(onInteract: Submit);
	}

	private void ButtonEffect(KMSelectable selectable)
	{
		ButtonEffect(selectable, 1f, KMSoundOverride.SoundEffect.ButtonPress);
	}

	private void CycleLeft()
	{
		ButtonEffect(_leftArrow);
		CycleDirection(-1);
	}

	internal void CycleRight()
	{
		ButtonEffect(_rightArrow);
		CycleDirection(1);
	}

	private int GetWidth(TextMesh textMesh)
	{
		textMesh.font.RequestCharactersInTexture(textMesh.text);

		int width = 0;
		foreach (char c in textMesh.text)
		{
			CharacterInfo info;
			if (textMesh.font.GetCharacterInfo(c, out info))
				width += info.advance;
		}
		return width;
	}

	private void CycleDirection(int offset)
	{
		if (IsSolved)
			return;

		_cycleIndex = (_cycleIndex + offset).Modulo(_manuals.Length);
		_displayText.text = _manuals[_cycleIndex].Name;
		_displayText.fontSize = Mathf.FloorToInt(Mathf.Min(1f, 62f / GetWidth(_displayText)) * 110);
	}

	private new void Solve(params string[] logs)
	{
		base.Solve(logs);
		_pageRenderer.material.mainTexture = _solveTexture;
	}

	internal void Submit()
	{
		ButtonEffect(_submitButton);

		if (IsSolved)
			return;

		Log("Submitted {0}", _manuals[_cycleIndex].Name);

		if (_cycleIndex == _correctIndex) Solve("Module solved.");
		else Strike("Strike!");
	}
}
