/* Plays music based on triggers
 */

using System.Collections;
using UnityEngine;



public class MusicPlayer : MonoBehaviour
{
	// Soundtrack list
	public AudioClip[] soundtrackList;
	
	// Booleans
	public bool fadeOutBetweenTracks = true;
	public bool fadeInNewTrack = true;
	
	// Speeds
	public float fadeOutSpeed = 1.0f;
	public float fadeInSpeed = 1.0f;
	
	// Delay between tracks
	public float transitionDelay = 1.0f;
	
	// Additional (private)
	bool _isFadingOut = false;
	bool _isFadingIn = false;
	float _fadeoutSeconds = 1.0f;
	float _fadeinSeconds = 1.0f;
	int _trackID;
	
	
	void Start()
	{
		// Convert speed into actual seconds
		_fadeoutSeconds = 1f / this.fadeOutSpeed;
		_fadeinSeconds = 1f / this.fadeInSpeed;
	}
	
	public void PlayTrack(int id)
	{
		AudioSource audio = this.audio;
		
		_trackID = id;
		if (audio.isPlaying && this.fadeOutBetweenTracks) {
			_isFadingOut = true;
		} else {
			if (this.fadeInNewTrack) {
				audio.volume = 0f;
				audio.clip = this.soundtrackList[_trackID];
				audio.Play();
				FadeIn();
			} else {
				audio.volume = 1f;
				audio.clip = this.soundtrackList[_trackID];
				audio.Play();
			}
		}
	}
	
	void FadeOut() {
		StartCoroutine(FadeOutCorountine());
	}
	IEnumerator FadeOutCorountine()
	{
		if (!_isFadingOut) {
			this.audio.Stop();
			yield return new WaitForSeconds(this.transitionDelay);
			PlayTrack(_trackID);
		}
	}
	
	void FadeIn()
	{
		_isFadingIn = true;
	}
	
	void Update()
	{
		AudioSource audio = this.audio;
		
		if (_isFadingOut) {
			if (audio.volume > 0f) {
				audio.volume -= Time.deltaTime * _fadeoutSeconds;
			} else {
				_isFadingOut = false;
				FadeOut();
			}
		}
	
		if (_isFadingIn) {
			if (audio.volume < 1f) {
				audio.volume += Time.deltaTime * _fadeinSeconds;
			} else {
				_isFadingIn = false;
			}
		}
	}
}
