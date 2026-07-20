using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class CarController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeftWheel;
    public WheelCollider frontRightWheel;
    public WheelCollider rearLeftWheel;
    public WheelCollider rearRightWheel;

    [Header("Wheel Meshes")]
    public Transform frontLeftMesh;
    public Transform frontRightMesh;
    public Transform rearLeftMesh;
    public Transform rearRightMesh;

    [Header("Drive Settings")]
    public float maxMotorTorque = 1500f;
    public float maxSpeedKPH = 200f;

    [Header("Braking Settings")]
    public float maxBrakeTorque = 5000f;

    [Header("Steering Settings")]
    public float maxSteerAngle = 30f;
    public Transform steeringWheel;
    public float steeringWheelMaxAngle = 450f;
    public float steeringWheelRotateSpeed = 5f;

    [Header("Steering Sensitivity")]
    public bool useSteerSensitivity = false;
    [Range(0f, 2f)] public float steerSensitivity = 1f;

    [Header("Grip Tuning")]
    public float sidewaysStiffness = 8f;
    public float forwardStiffness = 1f;

    [Header("Controls")]
    public KeyCode reverseKey = KeyCode.B;
    public KeyCode respawnKey = KeyCode.R;
    public Transform respawnPoint;

    [Header("Input Axes")]
    public string steerAxis = "Horizontal";

    [Header("Engine Audio")]
    public float minEngineRPM = 800f;
    public float maxEngineRPM = 6000f;
    [HideInInspector] public float CurrentRPM;

    [Header("Mesh Smoothing")]
    [Tooltip("Higher = snappier, lower = more lag but smoother")]
    public float wheelMeshSmooth = 30f;

    // ────────────────────────────────────────────────────────────────────────────

    Rigidbody rb;
    bool isReversed;
    Quaternion steeringWheelStartRot;
    float steeringWheelAngle;

    void Start()
    {
        // Rigidbody setup
        rb = GetComponent<Rigidbody>();
        rb.interpolation = RigidbodyInterpolation.Extrapolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        rb.centerOfMass += Vector3.down * 0.5f;

        // Physics solver
        Physics.defaultSolverIterations = 20;
        Physics.defaultSolverVelocityIterations = 10;

        // Faster physics rate: 100 Hz
        Time.fixedDeltaTime = 0.01f;

        // Wheel friction & substeps
        SetupFriction(frontLeftWheel);
        SetupFriction(frontRightWheel);
        SetupFriction(rearLeftWheel);
        SetupFriction(rearRightWheel);

        frontLeftWheel.ConfigureVehicleSubsteps(100, 5, 5);
        frontRightWheel.ConfigureVehicleSubsteps(100, 5, 5);
        rearLeftWheel.ConfigureVehicleSubsteps(100, 5, 5);
        rearRightWheel.ConfigureVehicleSubsteps(100, 5, 5);

        if (steeringWheel != null)
            steeringWheelStartRot = steeringWheel.localRotation;

        if (respawnPoint == null)
            Debug.LogWarning("RespawnPoint not set", this);
    }

    void Update()
    {
        // toggle reverse
        if (Input.GetKeyDown(reverseKey))
            isReversed = !isReversed;

        // respawn
        if (Input.GetKeyDown(respawnKey) && respawnPoint != null)
            Respawn();

        // steering-wheel visuals (frame-rate update)
        float steerVis = GetSteerInput();
        RotateSteeringWheel(steerVis);
    }

    void FixedUpdate()
    {
        // steering
        ApplySteering(GetSteerInput());

        // accel & brake
        float accel, brake;
        var pad = Gamepad.current;
        if (pad != null)
        {
            accel = pad.rightTrigger.ReadValue();
            brake = pad.leftTrigger.ReadValue();
        }
        else
        {
            accel = Input.GetKey(KeyCode.W) ? 1f : 0f;
            brake = Input.GetKey(KeyCode.S) ? 1f : 0f;
        }

        // drive/brake & RPM
        HandleDriveAndBraking(accel, brake);
    }

    void LateUpdate()
    {
        // smooth wheel-mesh pose
        UpdateWheelPoseSmooth(frontLeftWheel, frontLeftMesh);
        UpdateWheelPoseSmooth(frontRightWheel, frontRightMesh);
        UpdateWheelPoseSmooth(rearLeftWheel, rearLeftMesh);
        UpdateWheelPoseSmooth(rearRightWheel, rearRightMesh);
    }

    float GetSteerInput()
    {
        var pad = Gamepad.current;
        float raw = 0f;

        if (pad != null)
        {
            raw = pad.leftStick.x.ReadValue();
        }
        else
        {
            // explicit A/D keys
            if (Input.GetKey(KeyCode.A)) raw -= 1f;
            if (Input.GetKey(KeyCode.D)) raw += 1f;

            // if neither A nor D, fallback to axis (e.g. arrow keys or custom)
            if (Mathf.Approximately(raw, 0f))
                raw = Input.GetAxisRaw(steerAxis);
        }

        return useSteerSensitivity
            ? raw * steerSensitivity
            : raw;
    }

    void SetupFriction(WheelCollider wc)
    {
        var sf = wc.sidewaysFriction; sf.stiffness = sidewaysStiffness;
        wc.sidewaysFriction = sf;
        var ff = wc.forwardFriction; ff.stiffness = forwardStiffness;
        wc.forwardFriction = ff;
    }

    void ApplySteering(float input)
    {
        float angle = maxSteerAngle * input;
        frontLeftWheel.steerAngle = angle;
        frontRightWheel.steerAngle = angle;
    }

    void HandleDriveAndBraking(float accel, float brake)
    {
        float speedKPH = rb.linearVelocity.magnitude * 3.6f;
        float speedFactor = Mathf.Clamp01(1f - speedKPH / maxSpeedKPH);

        // motor
        float torque = accel > 0f
            ? accel * maxMotorTorque * speedFactor * (isReversed ? -1f : 1f)
            : 0f;
        frontLeftWheel.motorTorque
            = frontRightWheel.motorTorque
            = rearLeftWheel.motorTorque
            = rearRightWheel.motorTorque = torque;

        // brake
        float bTorque = brake > 0f
            ? brake * maxBrakeTorque
            : 0f;
        frontLeftWheel.brakeTorque
            = frontRightWheel.brakeTorque
            = rearLeftWheel.brakeTorque
            = rearRightWheel.brakeTorque = bTorque;

        // RPM
        CurrentRPM = accel > 0f
            ? Mathf.Lerp(minEngineRPM, maxEngineRPM, accel * speedFactor)
            : minEngineRPM;
    }

    void RotateSteeringWheel(float input)
    {
        if (steeringWheel == null) return;

        float target = input * steeringWheelMaxAngle;
        steeringWheelAngle = Mathf.LerpAngle(
            steeringWheelAngle,
            target,
            steeringWheelRotateSpeed * Time.deltaTime
        );

        steeringWheel.localRotation = steeringWheelStartRot
            * Quaternion.Euler(0f, 0f, -steeringWheelAngle);
    }

    void Respawn()
    {
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
    }

    void UpdateWheelPoseSmooth(WheelCollider wc, Transform mesh)
    {
        if (mesh == null) return;

        Vector3 targetPos;
        Quaternion targetRot;
        wc.GetWorldPose(out targetPos, out targetRot);

        // low-pass filter for extra smoothness
        mesh.position = Vector3.Lerp(
            mesh.position, targetPos,
            wheelMeshSmooth * Time.deltaTime
        );
        mesh.rotation = Quaternion.Slerp(
            mesh.rotation, targetRot,
            wheelMeshSmooth * Time.deltaTime
        );
    }
}