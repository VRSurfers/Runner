using UnityEngine;
using UnityEngine.UI;

public class HealthComponent : MonoBehaviour
{
	public int MaxHp = 1000;

    public Text HpText;

    private float hp = 1000;
    private float HP
    {
        get { return hp; }
        set
        {
            hp = value;
            HpText.text = ((int)hp).ToString();
        }
    }

	private void Awake()
	{
		hp = MaxHp;
	}

	public void Change(float delta)
	{
		float newHp = HP + delta;
		if (newHp > MaxHp)
		{
			HP = MaxHp;
		}
		else if (newHp < 0)
		{
			HP = 0;
			// TODO death:
		}
		else
			HP = newHp;
	}
}