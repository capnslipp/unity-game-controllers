#pragma strict

var TrackID = 0;
var PlayOnce : boolean = true;
@HideInInspector var isTriggered : boolean = false;
var SoundtrackObject : GameObject;

function OnTriggerEnter(collider : Collider) {
	if(collider.gameObject.tag == "Player") {
		SoundtrackObject.GetComponent(SoundtrackController).playTrack(TrackID);
		if(PlayOnce && !isTriggered) {
			isTriggered = true;
		}
	}
}