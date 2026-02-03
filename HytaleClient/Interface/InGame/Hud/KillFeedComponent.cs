using System;
using System.Collections.Generic;
using HytaleClient.Graphics;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Interface.UI.Styles;

namespace HytaleClient.Interface.InGame.Hud
{
	// Token: 0x020008BE RID: 2238
	internal class KillFeedComponent : InterfaceComponent
	{
		// Token: 0x060040EE RID: 16622 RVA: 0x000BCFCC File Offset: 0x000BB1CC
		public KillFeedComponent(InGameView inGameView) : base(inGameView.Interface, inGameView.HudContainer)
		{
			this._inGameView = inGameView;
		}

		// Token: 0x060040EF RID: 16623 RVA: 0x000BCFF4 File Offset: 0x000BB1F4
		public void Build()
		{
			base.Clear();
			Document document;
			this.Interface.TryGetDocument("InGame/Hud/KillFeed.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			this._killFeed = uifragment.Get<Group>("KillFeed");
			this.RebuildFeed();
		}

		// Token: 0x060040F0 RID: 16624 RVA: 0x000BD042 File Offset: 0x000BB242
		protected override void OnMounted()
		{
			this._currentTime = 0f;
			this.Desktop.RegisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x060040F1 RID: 16625 RVA: 0x000BD068 File Offset: 0x000BB268
		protected override void OnUnmounted()
		{
			this.Desktop.UnregisterAnimationCallback(new Action<float>(this.Animate));
		}

		// Token: 0x060040F2 RID: 16626 RVA: 0x000BD083 File Offset: 0x000BB283
		public void ResetState()
		{
			this._currentTime = 0f;
			this._killFeedEntries.Clear();
			this._killFeed.Clear();
		}

		// Token: 0x060040F3 RID: 16627 RVA: 0x000BD0AC File Offset: 0x000BB2AC
		public void RebuildFeed()
		{
			this._killFeed.Clear();
			Document doc;
			this.Interface.TryGetDocument("InGame/Hud/KillFeedEntry.ui", out doc);
			foreach (KillFeedComponent.KillFeedEntry entry in this._killFeedEntries)
			{
				this.AddKillFeedEntry(entry, doc);
			}
			this._killFeed.Layout(null, true);
		}

		// Token: 0x060040F4 RID: 16628 RVA: 0x000BD13C File Offset: 0x000BB33C
		private void Animate(float deltaTime)
		{
			bool flag = false;
			this._currentTime += deltaTime;
			while (this._killFeedEntries.Count > 0)
			{
				KillFeedComponent.KillFeedEntry killFeedEntry = this._killFeedEntries[0];
				bool flag2 = killFeedEntry.ExpirationTime < this._currentTime;
				if (!flag2)
				{
					break;
				}
				this._killFeedEntries.RemoveAt(0);
				this._killFeed.Remove(killFeedEntry.Element);
				killFeedEntry.Element = null;
				flag = true;
			}
			bool flag3 = flag;
			if (flag3)
			{
				this._killFeed.Layout(null, true);
			}
		}

		// Token: 0x060040F5 RID: 16629 RVA: 0x000BD1DC File Offset: 0x000BB3DC
		private void AddKillFeedEntry(KillFeedComponent.KillFeedEntry entry, Document doc)
		{
			UIFragment uifragment = doc.Instantiate(this.Desktop, this._killFeed);
			bool flag = entry.Killer == null;
			if (flag)
			{
				uifragment.Get<Label>("Killer").Visible = false;
			}
			else
			{
				uifragment.Get<Label>("Killer").Text = entry.Killer;
			}
			uifragment.Get<Label>("Decedent").Text = entry.Decedent;
			TextureArea textureArea;
			bool flag2 = entry.Icon != null && this._inGameView.TryMountAssetTexture(entry.Icon, out textureArea);
			if (flag2)
			{
				uifragment.Get<Element>("Icon").Background = new PatchStyle(textureArea);
			}
			entry.Element = uifragment.Get<Group>("KillFeedEntry");
		}

		// Token: 0x060040F6 RID: 16630 RVA: 0x000BD29C File Offset: 0x000BB49C
		public void OnReceiveNewEntry(string decedent, string killer, string icon)
		{
			Document doc;
			this.Interface.TryGetDocument("InGame/Hud/KillFeedEntry.ui", out doc);
			KillFeedComponent.KillFeedEntry killFeedEntry = new KillFeedComponent.KillFeedEntry
			{
				Decedent = decedent,
				Killer = killer,
				ExpirationTime = this._currentTime + 5f,
				Icon = icon
			};
			this.AddKillFeedEntry(killFeedEntry, doc);
			this._killFeedEntries.Add(killFeedEntry);
			this._killFeed.Layout(null, true);
		}

		// Token: 0x04001F28 RID: 7976
		public const float EntryDuration = 5f;

		// Token: 0x04001F29 RID: 7977
		private float _currentTime;

		// Token: 0x04001F2A RID: 7978
		private InGameView _inGameView;

		// Token: 0x04001F2B RID: 7979
		private Group _killFeed;

		// Token: 0x04001F2C RID: 7980
		private List<KillFeedComponent.KillFeedEntry> _killFeedEntries = new List<KillFeedComponent.KillFeedEntry>();

		// Token: 0x02000D80 RID: 3456
		private class KillFeedEntry
		{
			// Token: 0x04004227 RID: 16935
			public string Decedent;

			// Token: 0x04004228 RID: 16936
			public string Killer;

			// Token: 0x04004229 RID: 16937
			public float ExpirationTime;

			// Token: 0x0400422A RID: 16938
			public string Icon;

			// Token: 0x0400422B RID: 16939
			public Group Element;
		}
	}
}
