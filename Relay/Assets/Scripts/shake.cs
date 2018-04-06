using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shake : MonoBehaviour {

    // magnitude is how far it can shake. duration is how long
    public static IEnumerator purloinShake(GameObject target, float magnitude, float duration) {
        Vector3 basePosition = target.transform.position;

        float elapsed = 0;
        while (elapsed < duration) {
            float percentComplete = elapsed / duration;
            float damper = 1.0f - Mathf.Clamp(4.0f * percentComplete - 3.0f, 0.0f, 1.0f);

            float x = Random.value * 2.0f - 1.0f;
            x *= magnitude * damper;
            target.transform.position = basePosition + Vector3.right * x;
            elapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        target.transform.position = basePosition;
    }

    public static IEnumerator screenshake(GameObject target, float t, float strength) {
        Vector3 shakeUpStuff;
        Vector3 originalPosition = target.transform.position;

        //float z = target.transform.position.z;
        while (t > 0) {
            t -= Time.deltaTime * 10;
            shakeUpStuff = Random.insideUnitCircle * strength / 8;
            target.transform.position = originalPosition + shakeUpStuff;
            yield return null;
        }
        shakeUpStuff = Vector3.zero;
        target.transform.position = originalPosition;
    }
}
