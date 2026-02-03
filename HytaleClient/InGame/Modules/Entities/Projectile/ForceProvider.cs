using System;

namespace HytaleClient.InGame.Modules.Entities.Projectile
{
	// Token: 0x0200095B RID: 2395
	public interface ForceProvider
	{
		// Token: 0x06004AE0 RID: 19168
		void Update(PhysicsBodyState state, ForceAccumulator accumulator, bool onGround);
	}
}
