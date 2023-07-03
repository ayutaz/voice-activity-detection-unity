﻿#nullable enable
using System.Collections.Generic;
using System.Linq;

namespace Mochineko.VoiceActivityDetection
{
    internal sealed class VoiceSegmentActivityQueue
    {
        private readonly Queue<VoiceSegmentActivity> queue = new();
        private readonly float maxQueueingTimeSeconds;
        
        public VoiceSegmentActivityQueue(float maxQueueingTimeSeconds)
        {
            this.maxQueueingTimeSeconds = maxQueueingTimeSeconds;
        }
        
        public void Enqueue(VoiceSegmentActivity activity)
        {
            queue.Enqueue(activity);

            while (CalculateTotalTimeSeconds() > maxQueueingTimeSeconds)
            {
                queue.Dequeue();
            }
        }

        private float CalculateTotalTimeSeconds()
        {
            var totalTimeSeconds = 0f;    
            foreach (var activity in queue)
            {
                totalTimeSeconds += activity.timeSeconds;
            }

            return totalTimeSeconds;
        }

        public float ActiveTimeRate()
        {
            if (!queue.Any())
            {
                return 0f;
            }
            
            var totalTimeSeconds = 0f;
            var activeTimeSeconds = 0f;
            foreach (var activity in queue)
            {
                totalTimeSeconds += activity.timeSeconds;
                if (activity.isActive)
                {
                    activeTimeSeconds += activity.timeSeconds;
                }
            }

            return activeTimeSeconds / totalTimeSeconds;
        }
    }
}