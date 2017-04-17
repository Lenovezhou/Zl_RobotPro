using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SliderOnpointer : Slider {

    private Slider selfslider;
    private bool isDown = false;
	void Start () {
        selfslider = GetComponent<Slider>();
	}
    public override void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("手动控制相机关闭!!!!!");
        CameraBess.GetInstance.DisHandCmaera(false);
        isDown = true;
    }
   

    void Update() 
    {
        if (isDown && Input.GetMouseButtonUp(0))
        {
            //Debug.Log("手动控制相机开启!!!!!");
            CameraBess.GetInstance.DisHandCmaera(true);
            isDown = false;
        }
    }

}
