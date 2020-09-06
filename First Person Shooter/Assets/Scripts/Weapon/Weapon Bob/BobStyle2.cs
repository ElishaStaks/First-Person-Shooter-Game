using UnityEngine;

namespace WeaponBobbingV2 {
	public class BobStyle2 : WeaponRotatorBobBase {
		private const float m_BobbingRotationAmount = 0.75f;

		void Update() {
			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");
			float aHorz = Mathf.Abs(horizontal);
			float aVert = Mathf.Abs(vertical);
			float xMovement = 0.0f;
			float yMovement = 0.0f;
			float yRotation = 0.0f;

			Vector3 calcPosition = transform.localPosition;
			float rotatorValue = transform.localRotation.eulerAngles.y;

			if (aHorz == 0 && aVert == 0) {
				m_BobTimer = 0.0f;
			}
			else {
				xMovement = Mathf.Sin(m_BobTimer);
				yMovement = -Mathf.Abs(Mathf.Abs(xMovement) - 1);
				yRotation = Mathf.Sin(m_BobTimer);

				m_BobTimer += m_BobbingSpeed * Time.deltaTime * InternalTimerMultiplier;
				
				if (m_BobTimer > Mathf.PI * 2) {
					m_BobTimer = m_BobTimer - (Mathf.PI * 2);
				}
			}

			float totalMovement = Mathf.Clamp(aVert + aHorz, 0, 1) * 1.5f;

			if (xMovement != 0) {
				xMovement = xMovement * totalMovement;
				calcPosition.x = m_InitPos.x + xMovement * m_BobbingAmount;
			}
			else {
				calcPosition.x = m_InitPos.x;
			}

			if (yMovement != 0) {
				calcPosition.y = m_InitPos.y + yMovement * m_BobbingAmount;
			}
			else {
				calcPosition.y = m_InitPos.y;
			}

			if(yRotation != 0) {
				rotatorValue = m_InitRot.y + yRotation * totalMovement * m_BobbingRotationAmount;
			}
			else {
				rotatorValue = m_InitRot.y;
			}

			m_Rotator.AngleChanges = rotatorValue * m_BobMultiplier;
			transform.localPosition = Vector3.Lerp(transform.localPosition, calcPosition, Time.deltaTime * InternalMultiplier * m_BobMultiplier);
		}
	}
}