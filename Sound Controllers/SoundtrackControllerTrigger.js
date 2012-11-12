#pragma strict

var TrackID = 0;
var PlayOnce : boolean = true;
@HideInInspector var isTriggered : boolean = false;
var SoundtrackObject : GameObject;

function OnTriggerEnter(collider : Collider) {
	if(!PlayOnce) {
		if(collider.gameObject.tag == "Player" && !isTriggered) {
			isTriggered = true;
			SoundtrackObject.GetComponent(SoundtrackController).playTrack(TrackID);
		}
	} else {
		if(collider.gameObject.tag == "Player") {
			SoundtrackObject.GetComponent(SoundtrackController).playTrack(TrackID);
		}
	}
}