using UnityEngine;
using Random = UnityEngine.Random;

namespace AimBot.Scrips {
    public class Target : MonoBehaviour {
        public void ChangePosition() {
            Vector3 pos = new Vector3(Random.Range(-10, 10), Random.Range(1, 10), Random.Range(-3, 10));
            transform.localPosition = pos;
        }
    }
}