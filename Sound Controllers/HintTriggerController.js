#pragma strict

var HintText : String;
var HintStyle : GUISkin;

// Durations
var HintDuration : float;
var FadeInDuration : float = 3.0;
var FadeOutDuration : float = 3.0;

var color : Color;

// Boolean
@HideInInspector var HintDone : boolean = false;
@HideInInspector var HintExecute : boolean = false;
@HideInInspector var FadeIn : boolean = false;
@HideInInspector var FadeOut : boolean = false;

function Start() {	
	color.a = 0;
}

function OnGUI() {
	GUI.skin = HintStyle;
	
	if(HintExecute) {
		GUI.color = color;
		GUI.Label(Rect(Screen.width / 2 - 200, 10, 400, 30), HintText);
	}

	if(FadeIn) {
		if(color.a < 1) {
			color.a += Time.deltaTime / FadeInDuration;
		} else {
			FadeIn = false;
		}
	}

	if(FadeOut) {
		if(color.a > 0) {
			color.a -= Time.deltaTime / FadeOutDuration;
		} else {
			FadeOut = false;
		}
	}

	// Fix Numbers
	if(color.a > 1) {
		color.a = 1;
	}
	if(color.a < 0) {
		color.a = 0;
	}
}

function OnTriggerEnter(collider : Collider) {
	if(collider.gameObject.tag == "Player" && !HintDone) {
		HintDone = true;
		HintExecute = true;
		FadeIn = true;
		HintDuration += FadeInDuration;
		yield WaitForSeconds(HintDuration);
		FadeOut = true;
		yield WaitForSeconds(FadeOutDuration);
		HintExecute = false;
	}
}