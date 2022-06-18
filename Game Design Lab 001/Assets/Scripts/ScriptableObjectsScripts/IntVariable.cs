using UnityEngine;

[CreateAssetMenu(fileName = "IntVariable", menuName = "ScriptableObjects/IntVariable", order = 2)]
public class IntVariable : ScriptableObject //a scriptable object instead of MonoBehavior
{
#if UNITY_EDITOR
    [Multiline]
    public string DeveloperDescription = "";
#endif
    private int _value = 0;
    public int Value
    {
        get{
            return _value;
        }
    }

    public void SetValue(int value)
    {
        _value = value;
    }

    //overload
    public void SetValue(IntVariable value)
    {
        _value = value._value;
    }

    public void ApplyChange(int amount)
    {
        _value += amount;
    }

    //overload v2
    public void ApplyChange(IntVariable amount)
    {
        _value += amount._value;
    }
}