using Core.Systems;
using Core.UI;
using Photon.Pun;
using UnityEngine;

namespace Core.Gameplay
{
    public class CarController : MonoBehaviour
    {
        [SerializeField] private CameraControl mainCamera;
        [SerializeField] private PhotonView photonView;

        [Header("Wheels")]
        [SerializeField] private WheelCollider[] wheelColliders = new WheelCollider[4];
        [SerializeField] private Transform[] wheelVisuals = new Transform[4];

        [Header("Settings")]
        [SerializeField] private CarVisual carVisual;
        [SerializeField] private float maxSpeed = 100f;
        [SerializeField] private float motorForce = 1000f;
        [SerializeField] private float brakeForce = 2000f;
        [SerializeField] private float maxSteerAngle = 30f;
        [SerializeField] private float currentSteerAngle;
        [SerializeField] private float steerSmoothness = 2f;

        private IInputSource inputSource;
        private Rigidbody carRigidbody;
        private UIManager uiManager;

        public void Start()
        {
            if (!photonView.IsMine)
            {
                enabled = false;
                return;
            }
            inputSource = new PlayerInputSource();

            mainCamera.gameObject.SetActive(true);
            mainCamera.transform.SetParent(null);

            carVisual.Initialize();
            carRigidbody = GetComponent<Rigidbody>();

            uiManager = EntryPoint.Get<UIManager>();
        }

        private void Update()
        {
            if (!photonView.IsMine) return;
            inputSource.UpdateInput();

            Accelerate(inputSource.GetAcceleration());
            Brake(inputSource.GetBrake());
            Steer(inputSource.GetSteering());

            UpdateWheelVisuals();

            uiManager.GetPanel<GamePanel>().UpdateSpeed((carRigidbody.velocity.magnitude * 3.6f).ToString("F1"));
        }

        private void Accelerate(float direction)
        {
            if (direction != 0 && carRigidbody.velocity.magnitude * 3.6f < maxSpeed)
            {
                wheelColliders[2].motorTorque = -direction * motorForce;
                wheelColliders[3].motorTorque = -direction * motorForce;
            }
            else
            {
                wheelColliders[2].motorTorque = 0;
                wheelColliders[3].motorTorque = 0;
            }
        }

        private void Brake(bool isBraking)
        {
            if (isBraking)
            {
                wheelColliders[2].brakeTorque = brakeForce;
                wheelColliders[3].brakeTorque = brakeForce;
            }
            else
            {
                wheelColliders[2].brakeTorque = 0;
                wheelColliders[3].brakeTorque = 0;
            }
        }

        private void Steer(float direction)
        {
            float targetSteerAngle = direction * maxSteerAngle;

            currentSteerAngle = Mathf.Lerp(currentSteerAngle, targetSteerAngle, Time.deltaTime * steerSmoothness);

            wheelColliders[0].steerAngle = currentSteerAngle;
            wheelColliders[1].steerAngle = currentSteerAngle;
        }

        private void UpdateWheelVisuals()
        {
            for (int i = 0; i < wheelColliders.Length; i++)
            {
                Vector3 position;
                Quaternion rotation;
                wheelColliders[i].GetWorldPose(out position, out rotation);

                wheelVisuals[i].position = position;
                wheelVisuals[i].rotation = rotation;
            }
        }
    }
}