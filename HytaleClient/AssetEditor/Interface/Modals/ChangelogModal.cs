using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using HytaleClient.AssetEditor.Interface.Editor;
using HytaleClient.Graphics;
using HytaleClient.Interface.Messages;
using HytaleClient.Interface.UI;
using HytaleClient.Interface.UI.Elements;
using HytaleClient.Interface.UI.Markup;
using HytaleClient.Math;
using HytaleClient.Utils;

namespace HytaleClient.AssetEditor.Interface.Modals
{
	// Token: 0x02000B9D RID: 2973
	internal class ChangelogModal : Element
	{
		// Token: 0x06005BE2 RID: 23522 RVA: 0x001CD11F File Offset: 0x001CB31F
		public ChangelogModal(AssetEditorOverlay assetEditorOverlay) : base(assetEditorOverlay.Desktop, null)
		{
			this._assetEditorOverlay = assetEditorOverlay;
		}

		// Token: 0x06005BE3 RID: 23523 RVA: 0x001CD138 File Offset: 0x001CB338
		public void Build()
		{
			base.Clear();
			this._isInitialized = false;
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/ChangelogModal.ui", out document);
			UIFragment uifragment = document.Instantiate(this.Desktop, this);
			uifragment.Get<TextButton>("CloseButton").Activating = new Action(this.Dismiss);
			this._log = uifragment.Get<Group>("Log");
			this._container = uifragment.Get<Group>("Container");
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this.InitChangelog();
			}
		}

		// Token: 0x06005BE4 RID: 23524 RVA: 0x001CD1CA File Offset: 0x001CB3CA
		private void InitChangelog()
		{
			this._isInitialized = true;
			Task.Run(delegate()
			{
				List<ChangelogModal.ChangelogElement> elements = this.LoadChangelog();
				this._assetEditorOverlay.Interface.Engine.RunOnMainThread(this._assetEditorOverlay.Interface, delegate
				{
					this.BuildLog(elements);
				}, false, false);
			});
		}

		// Token: 0x06005BE5 RID: 23525 RVA: 0x001CD1E8 File Offset: 0x001CB3E8
		private void BuildLog(List<ChangelogModal.ChangelogElement> elements)
		{
			this._log.Clear();
			Document document;
			this.Desktop.Provider.TryGetDocument("AssetEditor/ChangelogVersion.ui", out document);
			Document document2;
			this.Desktop.Provider.TryGetDocument("AssetEditor/ChangelogChange.ui", out document2);
			Document document3;
			this.Desktop.Provider.TryGetDocument("AssetEditor/ChangelogSectionTitle.ui", out document3);
			foreach (ChangelogModal.ChangelogElement changelogElement in elements)
			{
				switch (changelogElement.Type)
				{
				case ChangelogModal.ChangelogElementType.VersionTitle:
				{
					Version v = new Version(changelogElement.Text);
					UIFragment uifragment = document.Instantiate(this.Desktop, this._log);
					uifragment.Get<Label>("Version").Text = changelogElement.Text;
					uifragment.Get<Label>("NewLabel").Visible = (this.PreviouslyUsedVersion != null && v > this.PreviouslyUsedVersion);
					uifragment.Get<Label>("Date").Text = changelogElement.Date;
					break;
				}
				case ChangelogModal.ChangelogElementType.SectionTitle:
				{
					UIFragment uifragment2 = document3.Instantiate(this.Desktop, this._log);
					uifragment2.Get<Label>("Text").Text = changelogElement.Text;
					break;
				}
				case ChangelogModal.ChangelogElementType.Change:
				{
					UIFragment uifragment3 = document2.Instantiate(this.Desktop, this._log);
					Label label = uifragment3.Get<Label>("Text");
					label.TextSpans = FormattedMessageConverter.GetLabelSpansFromMarkup(changelogElement.Text, new SpanStyle
					{
						Color = new UInt32Color?(label.Style.TextColor)
					});
					break;
				}
				}
			}
			bool isMounted = base.IsMounted;
			if (isMounted)
			{
				this._log.Layout(null, true);
			}
		}

		// Token: 0x06005BE6 RID: 23526 RVA: 0x001CD3FC File Offset: 0x001CB5FC
		private List<ChangelogModal.ChangelogElement> LoadChangelog()
		{
			List<ChangelogModal.ChangelogElement> list = new List<ChangelogModal.ChangelogElement>();
			using (StreamReader streamReader = new StreamReader(Path.Combine(Paths.EditorData, "Changelog.md")))
			{
				Version v = null;
				string text;
				while ((text = streamReader.ReadLine()) != null)
				{
					text = text.Trim();
					bool flag = text == "";
					if (!flag)
					{
						bool flag2 = text.StartsWith("-");
						if (flag2)
						{
							list.Add(new ChangelogModal.ChangelogElement
							{
								Type = ChangelogModal.ChangelogElementType.Change,
								Text = text.Substring(1).TrimStart(Array.Empty<char>())
							});
						}
						else
						{
							bool flag3 = text.StartsWith("###");
							if (flag3)
							{
								list.Add(new ChangelogModal.ChangelogElement
								{
									Type = ChangelogModal.ChangelogElementType.SectionTitle,
									Text = text.Substring(3).Trim()
								});
							}
							else
							{
								bool flag4 = text.StartsWith("##");
								if (flag4)
								{
									string[] array = text.Substring(2).Trim().Split(new char[]
									{
										'-'
									}, 2);
									string text2 = array[0].Trim().TrimStart(new char[]
									{
										'['
									}).TrimEnd(new char[]
									{
										']'
									});
									bool flag5 = text2.ToLowerInvariant() == "unreleased";
									if (!flag5)
									{
										string date = array[1].Trim();
										bool flag6 = v == null;
										if (flag6)
										{
											v = new Version(text2);
										}
										list.Add(new ChangelogModal.ChangelogElement
										{
											Type = ChangelogModal.ChangelogElementType.VersionTitle,
											Text = text2,
											Date = date
										});
									}
								}
							}
						}
					}
				}
			}
			return list;
		}

		// Token: 0x06005BE7 RID: 23527 RVA: 0x001CD5E4 File Offset: 0x001CB7E4
		protected override void OnMounted()
		{
			bool flag = !this._isInitialized;
			if (flag)
			{
				this.InitChangelog();
				base.Layout(null, true);
			}
		}

		// Token: 0x06005BE8 RID: 23528 RVA: 0x001CD619 File Offset: 0x001CB819
		public override Element HitTest(Point position)
		{
			return base.HitTest(position) ?? this;
		}

		// Token: 0x06005BE9 RID: 23529 RVA: 0x001CD628 File Offset: 0x001CB828
		protected override void OnMouseButtonUp(MouseButtonEvent evt, bool activate)
		{
			base.OnMouseButtonUp(evt, activate);
			bool flag = activate && !this._container.AnchoredRectangle.Contains(this.Desktop.MousePosition);
			if (flag)
			{
				this.Dismiss();
			}
		}

		// Token: 0x06005BEA RID: 23530 RVA: 0x001CD671 File Offset: 0x001CB871
		protected internal override void Dismiss()
		{
			this.Desktop.ClearLayer(4);
		}

		// Token: 0x06005BEB RID: 23531 RVA: 0x001CD680 File Offset: 0x001CB880
		protected internal override void Validate()
		{
			this.Dismiss();
		}

		// Token: 0x04003985 RID: 14725
		public Version PreviouslyUsedVersion;

		// Token: 0x04003986 RID: 14726
		private readonly AssetEditorOverlay _assetEditorOverlay;

		// Token: 0x04003987 RID: 14727
		private Group _container;

		// Token: 0x04003988 RID: 14728
		private Group _log;

		// Token: 0x04003989 RID: 14729
		private bool _isInitialized;

		// Token: 0x02000F9B RID: 3995
		private struct ChangelogElement
		{
			// Token: 0x04004B7B RID: 19323
			public ChangelogModal.ChangelogElementType Type;

			// Token: 0x04004B7C RID: 19324
			public string Text;

			// Token: 0x04004B7D RID: 19325
			public string Date;
		}

		// Token: 0x02000F9C RID: 3996
		private enum ChangelogElementType
		{
			// Token: 0x04004B7F RID: 19327
			VersionTitle,
			// Token: 0x04004B80 RID: 19328
			SectionTitle,
			// Token: 0x04004B81 RID: 19329
			Change
		}
	}
}
