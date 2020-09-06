using UnityEngine;

namespace WeaponBobbingV2 {
	public class LocalRotator : MonoBehaviour {
		[Header("Rotator Settings")]
		[SerializeField] private Vector3 m_RotatingAxis = new Vector3(0, 1, 0);
		private float m_AngleChanges = 0f;
		private Quaternion m_InitRot;

		public float AngleChanges {
			get {
				return m_AngleChanges;
			}
			set {
				m_AngleChanges = value;
			}
		}

		void Start() {
			m_InitRot = transform.localRotation;
		}

		void LateUpdate() {
			Quaternion change = Quaternion.AngleAxis(m_AngleChanges, m_RotatingAxis);
			transform.localRotation = m_InitRot * change;
		}
	}
}