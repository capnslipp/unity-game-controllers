/* Plays music based on triggers
 *
 * To-Do:
 * - Rewrite script to C#
 */

#pragma strict
#pragma downcast

// Soundtrack list
var SoundtrackList : AudioClip[];

// Booleans
var FadeOutBetweenTracks : boolean = true;
var FadeInNewTrack : boolean = true;

// Speeds
var FadeOutSpeed : float = 1.0;
var FadeInSpeed : float = 1.0;

// Delay between tracks
var TransitionDelay : float = 1.0;

// Additional (hidden)
@HideInInspector var fadeOutBool : boolean = false;
@HideInInspector var fadeInBool : boolean = false;
@HideInInspector var fadeout_speed_fix : float = 1.0;
@HideInInspector var fadein_speed_fix : float = 1.0;
@HideInInspector var track_id : int;

function Start() {
	// Convert speed into actual seconds
	fadeout_speed_fix = 1 / FadeOutSpeed;
	fadein_speed_fix = 1 / FadeInSpeed;
}

function playTrack(id) {
	track_id = id;
	if(audio.isPlaying && FadeOutBetweenTracks) {
		fadeOutBool = true;
	} else {
		if(FadeInNewTrack) {
			audio.volume = 0;
			audio.clip = SoundtrackList[track_id];
			audio.Play();
			fadeIn();
		} else {
			audio.volume = 1;
			audio.clip = SoundtrackList[track_id];
			audio.Play();
		}
	}
}

function fadeOut() {
	if(!fadeOutBool) {
		audio.Stop();
		yield WaitForSeconds(TransitionDelay);
		playTrack(track_id);
	}
}

function fadeIn() {
	fadeInBool = true;
}

function Update() {
	if(fadeOutBool) {
		if(audio.volume != 0) {
			audio.volume -= 1 * Time.deltaTime * fadeout_speed_fix;
		} else {
			fadeOutBool = false;
			fadeOut();
		}
	}

	if(fadeInBool) {
		if(audio.volume != 1) {
			audio.volume += 1 * Time.deltaTime * fadein_speed_fix;
		} else {
			fadeInBool = false;
		}
	}
}