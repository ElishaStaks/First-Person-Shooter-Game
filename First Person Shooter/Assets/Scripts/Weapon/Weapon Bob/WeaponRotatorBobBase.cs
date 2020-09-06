using UnityEngine;

namespace WeaponBobbingV2 {
	public class WeaponRotatorBobBase: WeaponBobBase {
		[Header("Rotator Bob Settings")]
		[SerializeField] protected Transform m_Target;
		[SerializeField] protected LocalRotator m_Rotator;

		protected override void Start() {
			base.Start();

			if(m_Rotator != null) {
				m_Rotator.enabled = true;
			}
		}

		void OnEnable() {
			if(m_Rotator != null) {
				m_Rotator.enabled = true;
			}
		}

		void OnDisable() {
			if(m_Rotator != null) {
				m_Rotator.enabled = false;
			}
		}
	}
}
