using System;
using UnityEngine;

namespace SoulsLantern.MonoScripts
{
    public class JointResolver : MonoBehaviour
    {
        public ConfigurableJoint joint;
        internal Player m_localPlayer;

        private void OnEnable()
        {
            if (Player.m_localPlayer != null)
            {
                m_localPlayer = Player.m_localPlayer;
            }

            joint = GetComponent<ConfigurableJoint>();
            joint.connectedBody = m_localPlayer.gameObject.GetComponent<Rigidbody>();
            joint.connectedAnchor = m_localPlayer.m_visEquipment.m_backTool.position;
        }
    }
}