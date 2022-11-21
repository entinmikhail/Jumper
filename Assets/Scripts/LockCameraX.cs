using Cinemachine;
using UnityEngine;

/// <summary>
/// An add-on module for Cinemachine Virtual Camera that locks the camera's Z co-ordinate
/// </summary>
[ExecuteInEditMode]
[SaveDuringPlay]
[AddComponentMenu("")] // Hide in menu
public class LockCameraX : CinemachineExtension
{
    [Tooltip("Lock the camera's Z position to this value")]
    public float m_XPosition = 0;
    public float m_YPosition = 1;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
    {
        if (stage == CinemachineCore.Stage.Body)
        {
            var pos = state.RawPosition;
            pos.x = m_XPosition;
            pos.y += m_YPosition;
            state.RawPosition = pos;
        }
    }
}