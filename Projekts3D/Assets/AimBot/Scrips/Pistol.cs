using UnityEngine;

namespace AimBot.Scrips {
    public class Pistol : MonoBehaviour {
        public bool Fire() {
            if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit)) {
                Debug.Log("Hit " + hit.transform.gameObject);
                if (hit.transform.gameObject.layer == LayerMask.GetMask("Target")) {
                    hit.transform.gameObject.GetComponent<Target>()?.ChangePosition();
                    return true;
                }
            }

            return false;
        }
    }
}