using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private Collider _mainCollider;
    [SerializeField] private Rigidbody _mainRigidbody;
    [SerializeField] private List<BodyPart> _bodyParts;

    public IReadOnlyList<BodyPart> BodyParts => _bodyParts;
    public Collider MainCollider => _mainCollider;

    private void Awake()
    {
        foreach (BodyPart bodyPart in _bodyParts)
        {
            bodyPart.Init();
            //bodyPart.Collider.enabled = false;
            bodyPart.Collider.isTrigger = true;
            bodyPart.Rigidbody.isKinematic = true;
        }
    }

    [Button]
    public void Activate()
    {
        _animator.enabled = false;
        _mainCollider.enabled = false;
        _mainRigidbody.isKinematic = true;

        RemoveJointsWithoutConnections();

        foreach (BodyPart bodyPart in _bodyParts)
        {
            bodyPart.Collider.enabled = true;
            bodyPart.Collider.isTrigger = false;
            bodyPart.Rigidbody.isKinematic = false;
        }
    }

    public void RemoveBodyParts()
    {
        for (int i = _bodyParts.Count - 1; i >= 0; i--)
        {
            RemoveJointWithoutConnection(i);
            Destroy(_bodyParts[i]);
        }
    }

    private void RemoveJointsWithoutConnections()
    {
        for (int i = _bodyParts.Count - 1; i >= 0; i--)
            RemoveJointWithoutConnection(i);
    }

    private void RemoveJointWithoutConnection(int bodyPartIndex)
    {
        if (_bodyParts[bodyPartIndex].HaveJoint && _bodyParts[bodyPartIndex].ConnectedBodyPart == null)
            _bodyParts[bodyPartIndex].DestroyJoint();
    }

    public void FreezeRagdoll()
    {
        foreach (BodyPart bodyPart in _bodyParts)
        {
            bodyPart.Collider.isTrigger = true;
            bodyPart.Rigidbody.isKinematic = true;
        }
    }

    public void FreezeClosestPart(Collider collider)
    {
        float minDistance = Mathf.Infinity;
        BodyPart targetBodyPart = _bodyParts[0];

        foreach (BodyPart bodyPart in _bodyParts)
        {
            Vector3 bodyPartPosition = bodyPart.Rigidbody.position;
            float distance = Vector3.SqrMagnitude(bodyPartPosition - collider.ClosestPoint(bodyPartPosition));
            if (distance >= minDistance) continue;
            minDistance = distance;
            targetBodyPart = bodyPart;
        }

        InverseConnections(targetBodyPart);
        //targetBodyPart.Rigidbody.isKinematic = true;
    }

    private static void InverseConnections(BodyPart bodyPart)
    {
        var chain = new List<BodyPart> {bodyPart};
        BodyPart nextBody = bodyPart.ConnectedBodyPart;
        while (nextBody != null)
        {
            chain.Add(nextBody);
            nextBody = nextBody.ConnectedBodyPart;
        }

        chain[0].SetConnectedBodyPart(null);
        chain[0].transform.SetParent(chain[1].transform.parent);
        for (var i = 1; i < chain.Count; i++)
        {
            chain[i].SetConnectedBodyPart(chain[i - 1]);
            chain[i].Rigidbody.isKinematic = true;
        }
    }

    [Button]
    private void GetComponents()
    {
        _animator = GetComponentInChildren<Animator>();
        _mainCollider = GetComponent<Collider>();
        _mainRigidbody = GetComponent<Rigidbody>();
        _bodyParts = GetComponentsInChildren<BodyPart>().ToList();
    }

    [Button]
    private void CreateBodyParts()
    {
        foreach (Rigidbody rigidbody in GetComponentsInChildren<Rigidbody>().Where(r => r != _mainRigidbody))
            rigidbody.gameObject.AddComponent(typeof(BodyPart));
    }
}