using System;
using HytaleClient.Math;

namespace HytaleClient.Graphics.Particles
{
	// Token: 0x02000AB7 RID: 2743
	// (Invoke) Token: 0x06005678 RID: 22136
	internal delegate void UpdateParticleCollisionFunc(ParticleSpawner particleSpawner, ref ParticleBuffers.ParticleSimulationData particleData0, ref ParticleBuffers.ParticleRenderData particleData1, ref Vector2 particleScale, ref ParticleBuffers.ParticleLifeData particleLife, Vector3 previousPosition, Quaternion inverseRotation);
}
