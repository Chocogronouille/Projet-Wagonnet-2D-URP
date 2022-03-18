using UnityEngine;
using UnityEngine.UI;

public class InteractBar : MonoBehaviour
{
    public Slider slider;
    public Gradient gradient;
    public Image fill;

    public void SetMaxCount(int count)
    {
        slider.maxValue = count;
        slider.value = 0;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetCount(int count)
    {
        slider.value = count;
        
        fill.color = gradient.Evaluate(slider.normalizedValue);

    }
}
