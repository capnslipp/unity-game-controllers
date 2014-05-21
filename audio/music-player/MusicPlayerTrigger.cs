/* Plays music based on triggers
 */

using UnityEngine;



public class MusicPlayerTrigger : MonoBehaviour
{
	public int trackID = 0;
	public bool playOnce = true;
	bool _isTriggered = false;
	public GameObject soundtrackObject;
	
	
	void OnTriggerEnter(Collider collider)
	{
		if (this.collider.gameObject.tag == "Player") {
			this.soundtrackObject.GetComponent<SoundtrackController>().PlayTrack(this.trackID);
			if (this.playOnce && !_isTriggered)
				_isTriggered = true;
		}
	}
}
