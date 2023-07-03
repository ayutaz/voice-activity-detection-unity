﻿#nullable enable
using UniRx;
using Unity.Logging;
using UnityEngine;

namespace Mochineko.VoiceActivityDetection.Samples
{
    /// <summary>
    /// A sample of voice activity detection as a component.
    /// </summary>
    internal sealed class VADSample : MonoBehaviour
    {
        [SerializeField]
        private float activeVolumeThreshold = 0.01f;
        
        [SerializeField]
        private float maxQueueingTimeSeconds = 1f;
        
        [SerializeField]
        private float activationRateThreshold = 0.6f;
        
        [SerializeField]
        private float deactivationRateThreshold = 0.4f;
        
        [SerializeField]
        private float intervalSeconds = 0.5f;

        private IVoiceActivityDetector? vad;

        private void Start()
        {
            vad = new QueueingVoiceActivityDetector(
                source: new UnityMicrophoneSource(),
                buffer: new NullVoiceBuffer(),
                maxQueueingTimeSeconds,
                activeVolumeThreshold,
                activationRateThreshold,
                deactivationRateThreshold,
                intervalSeconds);

            vad
                .IsActive
                .Subscribe(isActive => Log.Debug("[VAD.Sample] IsActive: {0}", isActive))
                .AddTo(this);
        }

        private void OnDestroy()
        {
            vad?.Dispose();
        }

        private void Update()
        {
            vad?.Update();
        }
    }
}