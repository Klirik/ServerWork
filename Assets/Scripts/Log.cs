using UnityEngine;

public enum ModelType
{
    standart,
    neon,
    blue
}
public class Log : MonoBehaviour
{
    [SerializeField] private ModelType modelType = ModelType.standart;
    private void Start()
    {
        Debug.Log("I'm here! " + modelType);
    }
}
