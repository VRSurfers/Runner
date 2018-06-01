using UnityEngine;
using UnityEngine.UI;

public class AmoComponent : MonoBehaviour
{
	public int MaxAmo = 5;

	public Text AmoText;

	private int amo = 1000;
	public int Amo
	{
		get { return amo; }
		private set
		{
			amo = value;
			AmoText.text = ((int)amo).ToString();
		}
	}

	private void Awake()
	{
		amo = MaxAmo;
	}

	public void Change(int delta)
	{
		int newAmo = Amo + delta;
		if (newAmo > MaxAmo)
		{
			Amo = MaxAmo;
		}
		else if (newAmo < 0)
		{
			Amo = 0;
		}
		else
			Amo = newAmo;
	}
}