using UnityEngine;

namespace WeaponBobbingV2 {
	public class BobStyle1 : WeaponBobBase {
        private PlayerController m_player;

        private void Awake()
        {
            m_player = FindObjectOfType<PlayerController>();
        }

        void Update()
        {
			float horizontal = Input.GetAxis("Horizontal");
			float vertical = Input.GetAxis("Vertical");
			float aHorz = Mathf.Abs(horizontal);
			float aVert = Mathf.Abs(vertical);
			float xMovement = 0.0f;
			float yMovement = 0.0f;

			Vector3 calcPosition = transform.localPosition; 

			if (aHorz == 0 && aVert == 0) {
				m_BobTimer = 0.0f;
			}
			else {
				xMovement = Mathf.Sin(m_BobTimer);
				yMovement = -Mathf.Abs(Mathf.Abs(xMovement) - 1);

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

            transform.localPosition = Vector3.Lerp(transform.localPosition, calcPosition, Time.deltaTime * InternalMultiplier * m_BobMultiplier);
        }
	}
}