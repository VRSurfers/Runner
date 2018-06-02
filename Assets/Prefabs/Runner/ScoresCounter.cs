using UnityEngine;
using UnityEngine.UI;

public class ScoresCounter : MonoBehaviour
{
	public float MaxScore;

	public Text ScoreText;

	private float currentScore = 1000;
	public float Score
	{
		get { return currentScore; }
		private set
		{
			currentScore = value;
			UpdateText();
		}
	}

	private void UpdateText()
	{
		ScoreText.text = (Mathf.Round(currentScore)).ToString();
	}

	private void Awake()
	{
		currentScore = MaxScore;
		UpdateText();
	}

	public void Change(float delta)
	{
		float newScore = currentScore + delta;
		if (newScore > MaxScore)
		{
			Score = MaxScore;
			// add max handler
		}
		else if (newScore < 0)
		{
			Score = 0;
			// add min handler
		}
		else
			Score = newScore;
	}
}