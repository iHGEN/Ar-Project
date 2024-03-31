using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using TMPro;
using UnityEngine.XR.Interaction.Toolkit.Transformers;

public class Placement : MonoBehaviour
{
    [SerializeField] List<GameObject> ObjectToSpawon;
    [SerializeField] GameObject CharcterController;
    [SerializeField] GameObject ParticalSystem;
    [SerializeField] GameObject DeleteButton;
    [SerializeField] GameObject SelectButton;
    [SerializeField] GameObject RayPoint;
    [SerializeField] Camera ARCamera;
    [SerializeField] Button button;
    [SerializeField] Transform Content;
    [SerializeField] GameObject EditObject;
    [SerializeField] GameObject SpownObject;
    [SerializeField] GameObject ScanText;
    [SerializeField] float MaxRayDistance;
    GameObject m_SelectedObject;
    List<GameObject> ObjectAlreadySpown = new List<GameObject>();
    RaycastHit raycastHit;
    void Start()
    {
        foreach (var item in ObjectToSpawon)
        {
            Button xbutton = Instantiate(button, Content);
            xbutton.GetComponentInChildren<TextMeshProUGUI>().text = item.name;
            xbutton.onClick.AddListener(() => Spwon(item));
        }
        StartCoroutine(StartScan());
    }
    private void OnDisable()
    {
        button.onClick.RemoveAllListeners();
    }
    IEnumerator StartScan()
    {
        CharcterController.SetActive(false);
        SelectButton.SetActive(false);
        Content.gameObject.SetActive(false);
        RayPoint.SetActive(false);
        DeleteButton.gameObject.SetActive(false);
        yield return new WaitUntil(IsArSessionTracking);
        ScanText.SetActive(false);
        Content.gameObject.SetActive(true);
        RayPoint.SetActive(true);
    }
    public void SelectObject()
    {
        if (Physics.Raycast(ARCamera.transform.position, ARCamera.transform.forward, out raycastHit, MaxRayDistance))
        {
            if (ObjectAlreadySpown.Contains(raycastHit.transform.gameObject))
            {
                m_SelectedObject = raycastHit.transform.gameObject;
                EditObject.SetActive(true);
                SpownObject.SetActive(false);
                CharcterController.SetActive(false);
            }
            if(raycastHit.transform.tag == "Charcter")
            {
                EditObject.SetActive(false);
                SpownObject.SetActive(false);
                CharcterController.SetActive(true);
            }
        }
    }
    public void CloseEditObjectMenu()
    {
        EditObject.SetActive(false);
        SpownObject.SetActive(true);
    }
    public void RotateObject(int Degree)
    {
        if (m_SelectedObject != null)
        {
            m_SelectedObject.transform.eulerAngles = new Vector3(m_SelectedObject.transform.eulerAngles.x, m_SelectedObject.transform.eulerAngles.y + Degree, m_SelectedObject.transform.eulerAngles.z);
        }
    }
    bool IsArSessionTracking()
    {
        return ARSession.state == ARSessionState.SessionTracking;
    }
    public void RemoveObject()
    {
        if (Physics.Raycast(ARCamera.transform.position, ARCamera.transform.forward,out raycastHit,MaxRayDistance))
        {
            if(ObjectAlreadySpown.Contains(raycastHit.transform.gameObject))
            {
                ObjectAlreadySpown.Remove(raycastHit.transform.gameObject);
                Destroy(raycastHit.transform.gameObject);
                CloseEditObjectMenu();
                m_SelectedObject = null;
                if(ObjectAlreadySpown.Count < 1)
                {
                    DeleteButton.SetActive(false);
                    SelectButton.SetActive(false);
                }
            }
        }
    }

    public void Spwon(GameObject obj)
    {
        if(Physics.Raycast(ARCamera.transform.position, ARCamera.transform.forward, out raycastHit, MaxRayDistance))
        {
            GameObject m_ParticalSystem = Instantiate(ParticalSystem, raycastHit.point, Quaternion.identity);
            Destroy(m_ParticalSystem, 2f);
            GameObject m_object = Instantiate(obj, raycastHit.point, Quaternion.identity);
            m_object.AddComponent<ARTransformer>();
            ObjectAlreadySpown.Add(m_object);
            DeleteButton.SetActive(true); 
            SelectButton.SetActive(true);
        }
    }
}
