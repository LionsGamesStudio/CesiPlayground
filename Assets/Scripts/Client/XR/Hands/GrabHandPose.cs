using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace CesiPlayground.Client.XR.Hands
{
    /// <summary>
    /// A client-side component that allows a grabbable object to define a custom
    /// hand pose for the grabbing hand, disabling the animator temporarily.
    /// </summary>
    public class GrabHandPose : MonoBehaviour
    {
        [SerializeField] private float poseTransitionDuration = 0.2f;
        [SerializeField] private HandData rightHandPose;
        [SerializeField] private HandData leftHandPose;

        private Vector3 _startingHandPosition, _finalHandPosition;
        private Quaternion _startingHandRotation, _finalHandRotation;
        private Quaternion[] _startingFingerRotations, _finalFingerRotations;

        private void Start()
        {
            if (TryGetComponent<UnityEngine.XR.Interaction.Toolkit.Interactables.XRGrabInteractable>(out var grabInteractable))
            {
                grabInteractable.selectEntered.AddListener(SetupPose);
                grabInteractable.selectExited.AddListener(UnsetPose);
            }

            if (rightHandPose != null) rightHandPose.gameObject.SetActive(false);
            if (leftHandPose != null) leftHandPose.gameObject.SetActive(false);
        }

        public void SetupPose(SelectEnterEventArgs arg)
        {
            if (arg.interactorObject.transform.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor>() != null)
            {
                HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();
                if (handData == null) return;

                handData.animator.enabled = false;

                HandData poseToApply = handData.handType == HandData.HandModelType.Right ? rightHandPose : leftHandPose;
                SetHandDataValues(handData, poseToApply);
                StartCoroutine(SetHandDataRoutine(handData, _finalHandPosition, _finalHandRotation, _finalFingerRotations, _startingHandPosition, _startingHandRotation, _startingFingerRotations));
            }
        }

        public void UnsetPose(SelectExitEventArgs arg)
        {
            if (arg.interactorObject.transform.GetComponent<UnityEngine.XR.Interaction.Toolkit.Interactors.XRDirectInteractor>() != null)
            {
                HandData handData = arg.interactorObject.transform.GetComponentInChildren<HandData>();
                if (handData == null) return;

                handData.animator.enabled = true;
                StartCoroutine(SetHandDataRoutine(handData, _startingHandPosition, _startingHandRotation, _startingFingerRotations, _finalHandPosition, _finalHandRotation, _finalFingerRotations));
            }
        }

        public void SetHandDataValues(HandData h1, HandData h2)
        {
            _startingHandPosition = new Vector3(h1.root.localPosition.x / h1.root.localScale.x,
                h1.root.localPosition.y / h1.root.localScale.y, h1.root.localPosition.z / h1.root.localScale.z);
            _finalHandPosition = new Vector3(h2.root.localPosition.x / h2.root.localScale.x,
                h2.root.localPosition.y / h2.root.localScale.y, h2.root.localPosition.z / h2.root.localScale.z);

            _startingHandRotation = h1.root.localRotation;
            _finalHandRotation = h2.root.localRotation;

            _startingFingerRotations = new Quaternion[h1.fingerBones.Length];
            _finalFingerRotations = new Quaternion[h1.fingerBones.Length];

            for (int i = 0; i < h1.fingerBones.Length; i++)
            {
                _startingFingerRotations[i] = h1.fingerBones[i].localRotation;
                _finalFingerRotations[i] = h2.fingerBones[i].localRotation;
            }
        }
        
        public IEnumerator SetHandDataRoutine(HandData h, Vector3 newPosition, Quaternion newRotation,
            Quaternion[] newBonesRotation, Vector3 startingPosition, Quaternion startingRotation, Quaternion[] startingBonesRotation)
        {
            float timer = 0;
            while (timer < poseTransitionDuration)
            {
                float progress = timer / poseTransitionDuration;
                Vector3 p = Vector3.Lerp(startingPosition, newPosition, progress);
                Quaternion r = Quaternion.Lerp(startingRotation, newRotation, progress);

                h.root.localPosition = p;
                h.root.localRotation = r;

                for (int i = 0; i < newBonesRotation.Length; i++)
                {
                    h.fingerBones[i].localRotation = Quaternion.Lerp(startingBonesRotation[i], newBonesRotation[i], progress);
                }

                timer += Time.deltaTime;
                yield return null;
            }
            // Ensure final pose is set perfectly
            h.root.localPosition = newPosition;
            h.root.localRotation = newRotation;
            for (int i = 0; i < newBonesRotation.Length; i++) { h.fingerBones[i].localRotation = newBonesRotation[i]; }
        }
        
#if UNITY_EDITOR
        [MenuItem("Tools/Mirror Selected Right Grab Pose")]
        public static void MirrorRightPose()
        {
            if (Selection.activeGameObject == null) return;
            GrabHandPose handPose = Selection.activeGameObject.GetComponent<GrabHandPose>();
            if (handPose != null)
            {
                handPose.MirrorPose(handPose.leftHandPose, handPose.rightHandPose);
            }
        }
#endif
        
        public void MirrorPose(HandData poseToMirror, HandData poseUsedToMirror)
        {
            Vector3 mirroredPosition = poseUsedToMirror.root.localPosition;
            mirroredPosition.x *= -1;

            Quaternion mirroredQuaternion = poseUsedToMirror.root.localRotation;
            mirroredQuaternion.y *= -1;
            mirroredQuaternion.z *= -1;

            poseToMirror.root.localPosition = mirroredPosition;
            poseToMirror.root.localRotation = mirroredQuaternion;

            for (int i = 0; i < poseUsedToMirror.fingerBones.Length; i++)
            {
                poseToMirror.fingerBones[i].localRotation = poseUsedToMirror.fingerBones[i].localRotation;
            }
        }
    }
}