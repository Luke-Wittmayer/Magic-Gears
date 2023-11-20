using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using UnityEngine.Events;

public class ButtonScript : MonoBehaviour {

    public GameObject definedButton;
    public UnityEvent OnClick = new UnityEvent();

    public BattleSystem Test;

    // Use this for initialization
    void Start () {
        definedButton = this.gameObject;
        //Test = GameObject.Find("Battle System") as BattleSystem;
	}
	
	// Update is called once per frame
	void Update () {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;
        
        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
            {
                Debug.Log(Hit.collider.gameObject.name);
                var id = Hit.colliderInstanceID;
#if UNITY_EDITOR
                var placeholder = EditorUtility.InstanceIDToObject(id);
                Unit PH2 = placeholder as Unit;
#endif
                Debug.Log("222");

            }
        }    
    }
}