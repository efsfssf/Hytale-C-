using System;
using System.Collections.Generic;
using System.Linq;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules
{
	// Token: 0x020008ED RID: 2285
	internal class ClientFeatureModule : Module
	{
		// Token: 0x0600438E RID: 17294 RVA: 0x000D75E4 File Offset: 0x000D57E4
		public ClientFeatureModule(GameInstance gameInstance) : base(gameInstance)
		{
			foreach (ClientFeature key in Enumerable.Cast<ClientFeature>(Enum.GetValues(typeof(ClientFeature))))
			{
				this.features.Add(key, false);
			}
		}

		// Token: 0x0600438F RID: 17295 RVA: 0x000D7660 File Offset: 0x000D5860
		public bool IsFeatureEnabled(ClientFeature feature)
		{
			bool result;
			this.features.TryGetValue(feature, out result);
			return result;
		}

		// Token: 0x06004390 RID: 17296 RVA: 0x000D7682 File Offset: 0x000D5882
		public void SetFeatureEnabled(ClientFeature feature, bool enabled)
		{
			this.features[feature] = enabled;
			this._gameInstance.App.Interface.SettingsComponent.Build();
		}

		// Token: 0x0400214D RID: 8525
		private Dictionary<ClientFeature, bool> features = new Dictionary<ClientFeature, bool>();
	}
}
