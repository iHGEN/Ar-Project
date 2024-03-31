using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CharcterMovement : MonoBehaviour
{
    [SerializeField] GameObject Charcter;
    [SerializeField] Camera ArCamera;
    [SerializeField] float CharcterSpeed;
    [SerializeField] float CharcterRotationSpeed;
    [SerializeField] float MaxRayDistance;
    [SerializeField] bool IsReadyToGo;
    GameObject TargetDestination;
    Animator m_animator;
    GameObject m_Charcter;
    RaycastHit raycastHit;


    private void Start()
    {
        TargetDestination = new GameObject();
    }
    public void GetLocation()
    {
        if (Physics.Raycast(ArCamera.transform.position, ArCamera.transform.forward, out raycastHit, MaxRayDistance))
        {
            TargetDestination.transform.position = raycastHit.point;
            IsReadyToGo = true;
        }
    }
    public void RunAnimation(string AnimationName)
    {
        m_animator.SetTrigger(AnimationName);
    }
    public void CharcterSpown()
    {
        if (Physics.Raycast(ArCamera.transform.position, ArCamera.transform.forward, out raycastHit, MaxRayDistance))
        {
            if (m_Charcter != null) return;

            m_Charcter = Instantiate(Charcter, raycastHit.point, Quaternion.identity);

            m_animator = m_Charcter.GetComponent<Animator>();
        }
    }
    public void MoveToDestination()
    {

        m_Charcter.transform.position = Vector3.MoveTowards(m_Charcter.transform.position, TargetDestination.transform.position, CharcterSpeed * Time.deltaTime);

        Vector3 TargetDirection = TargetDestination.transform.position - m_Charcter.transform.position;

        Vector3 NewDirection = Vector3.RotateTowards(m_Charcter.transform.forward, TargetDirection, CharcterRotationSpeed * Time.deltaTime, 0.0f);

        m_Charcter.transform.rotation = Quaternion.LookRotation(NewDirection);

        bool IsRichDestination = Vector3.Distance(m_Charcter.transform.position, TargetDestination.transform.position) < .01f;

        m_animator.SetBool("Walk", !IsRichDestination);

        if (IsRichDestination) IsReadyToGo = false;
    }
    void Update()
    {
        if (IsReadyToGo) MoveToDestination();
    }
}
