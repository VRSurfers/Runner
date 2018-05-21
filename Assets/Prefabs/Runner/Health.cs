using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour {

    public Text HpText;

    private float hp = 1000;
    public float HP
    {
        get { return hp; }
        set
        {
            hp = value;
            HpText.text = ((int)hp).ToString();
        }
    }
}
