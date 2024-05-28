using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Collider))]
[DisallowMultipleComponent]
public class BodyPart : MonoBehaviour
{
    private Joint _joint;

    public Rigidbody Rigidbody { get; private set; }
    public Collider Collider { get; private set; }
    public BodyPart ConnectedBodyPart { get; private set; }

    public bool HaveJoint => _joint != null;

    public void Init()
    {
        _joint = GetComponent<Joint>();
        Rigidbody = GetComponent<Rigidbody>();
        Collider = GetComponent<Collider>();
        if (HaveJoint && _joint.connectedBody != null)
            ConnectedBodyPart = _joint.connectedBody.GetComponent<BodyPart>();
    }

    public void SetConnectedBodyPart(BodyPart bodyPart)
    {
        _joint.connectedBody = bodyPart == null ? null : bodyPart.Rigidbody;
        ConnectedBodyPart = bodyPart;
    }

    public void DestroyJoint()
    {
        Destroy(_joint);
    }
}