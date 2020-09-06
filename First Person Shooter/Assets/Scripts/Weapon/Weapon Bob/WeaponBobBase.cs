using UnityEngine;

namespace WeaponBobbingV2 {
	public abstract class WeaponBobBase : MonoBehaviour {
		[Header("Base Bob Settings")]
		[SerializeField] [Range(0.1f, 10f)] protected float m_BobMultiplier = 1.0f;
		[SerializeField] [Range(0.1f, 0.3f)] protected float m_BobbingSpeed = 0.1f;
		[SerializeField] [Range(0.02f, 0.5f)] protected float m_BobbingAmount = 0.05f;

		protected const float InternalMultiplier = 1.5f;
		protected const float InternalTimerMultiplier = 60f;
		public Vector3 m_InitPos { get; set; }
		protected Vector3 m_InitRot;
		protected float m_BobTimer = 0;
		protected float m_DefaultBobbingSpeed;
		protected float m_DefaultBobbingAmount;

		public float BobMultiplier {
			get {
				return m_BobMultiplier;
			}
			set {
				m_BobMultiplier = value;
			}
		}

		public float BobbingSpeed {
			get {
				return m_BobbingSpeed;
			}
			set {
				m_BobbingSpeed = value;
			}
		}

		public float BobbingAmount {
			get {
				return m_BobbingAmount;
			}
			set {
				m_BobbingAmount = value;
			}
		}

		public float DefaultBobbingSpeed {
			get {
				return m_DefaultBobbingSpeed;
			}
		}
		
		public float DefaultBobbingAmount {
			get {
				return m_DefaultBobbingAmount;
			}
		}

		protected virtual void Start() {
			m_InitPos = transform.localPosition;

			m_DefaultBobbingSpeed = m_BobbingSpeed;
			m_DefaultBobbingAmount = m_BobbingAmount;
		}
	}	
}