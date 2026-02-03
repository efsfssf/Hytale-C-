using System;
using Coherent.UI.Binding;
using HytaleClient.Protocol;

namespace HytaleClient.InGame.Modules.ImmersiveScreen.Data
{
	// Token: 0x02000941 RID: 2369
	[CoherentType]
	public class ClientMediaData
	{
		// Token: 0x060048F0 RID: 18672 RVA: 0x0011B3EC File Offset: 0x001195EC
		public ClientMediaData()
		{
		}

		// Token: 0x060048F1 RID: 18673 RVA: 0x0011B3F8 File Offset: 0x001195F8
		public ClientMediaData(ImmersiveViewMediaData packet)
		{
			this.Platform = packet.Service;
			this.Id = packet.Id;
			this.Title = packet.Title;
			this.ChannelId = packet.ChannelId;
			this.ChannelName = packet.ChannelName;
			this.PublicationDate = packet.PublicationDate;
			this.AddedByUsername = packet.AddedByUsername;
			this.AddedByUuid = packet.AddedByUuid;
			this.Position = (int)packet.Position;
			this.Duration = (int)packet.Duration;
			this.Playing = packet.Playing;
			this.Stream = packet.Stream;
			this.SeekPositionCounter = packet.SeekPositionCounter;
			bool flag = packet.Thumbnail != null;
			if (flag)
			{
				this.Thumbnail = new ClientMediaData.ClientThumbnails(packet.Thumbnail);
			}
		}

		// Token: 0x060048F2 RID: 18674 RVA: 0x0011B4CC File Offset: 0x001196CC
		public ImmersiveViewMediaData ToPacket()
		{
			ImmersiveViewMediaData immersiveViewMediaData = new ImmersiveViewMediaData();
			immersiveViewMediaData.Service = this.Platform;
			immersiveViewMediaData.Id = this.Id;
			immersiveViewMediaData.Title = this.Title;
			immersiveViewMediaData.ChannelId = this.ChannelId;
			immersiveViewMediaData.ChannelName = this.ChannelName;
			immersiveViewMediaData.PublicationDate = this.PublicationDate;
			immersiveViewMediaData.AddedByUsername = this.AddedByUsername;
			immersiveViewMediaData.AddedByUuid = this.AddedByUuid;
			ClientMediaData.ClientThumbnails thumbnail = this.Thumbnail;
			immersiveViewMediaData.Thumbnail = ((thumbnail != null) ? thumbnail.ToPacket() : null);
			immersiveViewMediaData.Playing = this.Playing;
			immersiveViewMediaData.Position = (float)this.Position;
			immersiveViewMediaData.Duration = (float)this.Duration;
			immersiveViewMediaData.SeekPositionCounter = this.SeekPositionCounter;
			immersiveViewMediaData.Stream = this.Stream;
			return immersiveViewMediaData;
		}

		// Token: 0x040024EB RID: 9451
		[CoherentProperty("service")]
		public MediaService Platform;

		// Token: 0x040024EC RID: 9452
		[CoherentProperty("id")]
		public string Id;

		// Token: 0x040024ED RID: 9453
		[CoherentProperty("title")]
		public string Title;

		// Token: 0x040024EE RID: 9454
		[CoherentProperty("channelId")]
		public string ChannelId;

		// Token: 0x040024EF RID: 9455
		[CoherentProperty("channelName")]
		public string ChannelName;

		// Token: 0x040024F0 RID: 9456
		[CoherentProperty("publicationDate")]
		public string PublicationDate;

		// Token: 0x040024F1 RID: 9457
		[CoherentProperty("addedByUsername")]
		public string AddedByUsername;

		// Token: 0x040024F2 RID: 9458
		[CoherentProperty("addedByUuid")]
		public Guid AddedByUuid;

		// Token: 0x040024F3 RID: 9459
		[CoherentProperty("position")]
		public int Position;

		// Token: 0x040024F4 RID: 9460
		[CoherentProperty("seekPositionCounter")]
		public int SeekPositionCounter;

		// Token: 0x040024F5 RID: 9461
		[CoherentProperty("duration")]
		public int Duration;

		// Token: 0x040024F6 RID: 9462
		[CoherentProperty("playing")]
		public bool Playing;

		// Token: 0x040024F7 RID: 9463
		[CoherentProperty("stream")]
		public bool Stream;

		// Token: 0x040024F8 RID: 9464
		[CoherentProperty("thumbnail")]
		public ClientMediaData.ClientThumbnails Thumbnail;

		// Token: 0x040024F9 RID: 9465
		[CoherentProperty("viewCount")]
		public int ViewCount;

		// Token: 0x040024FA RID: 9466
		public string GameTitle;

		// Token: 0x02000E25 RID: 3621
		[CoherentType]
		public class ClientThumbnails
		{
			// Token: 0x060066F9 RID: 26361 RVA: 0x002167CA File Offset: 0x002149CA
			public ClientThumbnails()
			{
			}

			// Token: 0x060066FA RID: 26362 RVA: 0x002167D4 File Offset: 0x002149D4
			public ClientThumbnails(ImmersiveViewMediaData.Thumbnails packet)
			{
				this.Small = packet.Small;
				this.Normal = packet.Normal;
			}

			// Token: 0x060066FB RID: 26363 RVA: 0x002167F8 File Offset: 0x002149F8
			public ImmersiveViewMediaData.Thumbnails ToPacket()
			{
				return new ImmersiveViewMediaData.Thumbnails(this.Small, this.Normal);
			}

			// Token: 0x04004547 RID: 17735
			[CoherentProperty("small")]
			public string Small;

			// Token: 0x04004548 RID: 17736
			[CoherentProperty("normal")]
			public string Normal;
		}
	}
}
