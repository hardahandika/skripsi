using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stat : MonoBehaviour {

	Image content;
	
	[SerializeField] Text statValue;
	float currentFill, currentValue;

	public float MyCurrentValue{
		get{return currentValue;}
		set{
			if(value > MyMaxValue){
				currentValue = MyMaxValue;
			}
			else if(value < 0){
				currentValue = 0;
			}
			else{
				currentValue = value;
			}
            currentFill = currentValue / MyMaxValue;
			statValue.text = Mathf.Round(currentValue) + " / " + MyMaxValue;
		}
	}
	public float MyMaxValue { get; set; }
	// Use this for initialization
	void Start () {
		content = GetComponent<Image>();
		
	}
	
	// Update is called once per frame
	void Update () {
		content.fillAmount = currentFill;
	}

	public void Initialize(float currentValue, float maxValue){
		MyMaxValue = maxValue;
		MyCurrentValue = currentValue;
	}
}
