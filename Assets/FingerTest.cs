//Tested the finger moving 

/*using UnityEngine;

public class FingerTest : MonoBehaviour
{
    public Transform index1;
    public Transform index2;
    public Transform index3;

    private Quaternion startRot1;
    private Quaternion startRot2;
    private Quaternion startRot3;

    void Start()
    {
        startRot1 = index1.localRotation;
        startRot2 = index2.localRotation;
        startRot3 = index3.localRotation;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            index1.localRotation = startRot1 * Quaternion.Euler(60f, 0f, 0f);
            index2.localRotation = startRot2 * Quaternion.Euler(60f, 0f, 0f);
            index3.localRotation = startRot3 * Quaternion.Euler(60f, 0f, 0f);
        }
        else
        {
            index1.localRotation = startRot1;
            index2.localRotation = startRot2;
            index3.localRotation = startRot3;
        }
    }
}
*/

//worked with the slider
/*using UnityEngine;

public class FingerTest : MonoBehaviour
{
    public Transform index1;
    public Transform index2;
    public Transform index3;

    private Quaternion startRot1;
    private Quaternion startRot2;
    private Quaternion startRot3;

    [Range(0f, 1f)]
    public float bendAmount = 0f;

    void Start()
    {
        startRot1 = index1.localRotation;
        startRot2 = index2.localRotation;
        startRot3 = index3.localRotation;
    }

    void Update()
    {
        float bendAngle = 60f * bendAmount;

        index1.localRotation = startRot1 * Quaternion.Euler(bendAngle, 0f, 0f);
        index2.localRotation = startRot2 * Quaternion.Euler(bendAngle, 0f, 0f);
        index3.localRotation = startRot3 * Quaternion.Euler(bendAngle, 0f, 0f);
    }
}*/

using UnityEngine;

public class FingerTest : MonoBehaviour
{
    public Transform index1;
    public Transform index2;
    public Transform index3;

    private Quaternion startRot1;
    private Quaternion startRot2;
    private Quaternion startRot3;

    private float bendAmount = 0f;

    void Start()
    {
        startRot1 = index1.localRotation;
        startRot2 = index2.localRotation;
        startRot3 = index3.localRotation;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.F))
        {
            bendAmount += Time.deltaTime * 3f;
        }
        else
        {
            bendAmount -= Time.deltaTime * 3f;
        }

        bendAmount = Mathf.Clamp01(bendAmount);

        Quaternion bendRotation = Quaternion.Euler(60f * bendAmount, 0f, 0f);

        index1.localRotation = startRot1 * bendRotation;
        index2.localRotation = startRot2 * bendRotation;
        index3.localRotation = startRot3 * bendRotation;
    }
}
