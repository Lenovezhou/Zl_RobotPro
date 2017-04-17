using UnityEngine;
using System.Collections;
using System.Windows.Forms;
using UnityEngine.UI;

public class ScrollViewController : MonoBehaviour
{

    private ScrollViewController _instance;

    public ScrollViewController Instance
    {
        get { return _instance; }
    }

    private Scrollbar scrollBar;

    void Awake()
    {
        if (_instance==null)
        {
            _instance = this;
        }

        scrollBar = transform.Find("Scrollbar Horizontal").GetComponent<Scrollbar>();
    }

	public void Move(float value)
	{
		scrollBar.value = value;
	}

    public void Next()
    {
        scrollBar.value = 1;
    }

    public void Last()
    {
        scrollBar.value = 0;
    }
}
