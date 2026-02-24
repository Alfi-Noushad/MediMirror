using UnityEngine;

public class ArmMoveTest : MonoBehaviour
{
    public Transform rightArm;
    private Quaternion startRotation;

    void Start()
    {
        startRotation = rightArm.localRotation;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rightArm.localRotation = startRotation * Quaternion.Euler(45f, 0f, 0f);
        }
        else
        {
            rightArm.localRotation = startRotation;
        }
    }
}
