using System;
using System.IO;
using Hypixel.ProtoPlus;
using HytaleClient.Auth.Proto.Protocol;

namespace HytaleClient.Application.Services
{
	// Token: 0x02000BF9 RID: 3065
	public class ServerQueueUtil
	{
		// Token: 0x060061A7 RID: 24999 RVA: 0x00201990 File Offset: 0x001FFB90
		public static ProtoSerializable ReadResponseFrom(ClientServerQueueReply reply)
		{
			ProtoSerializable result;
			using (ProtoBinaryReader protoBinaryReader = ProtoBinaryReader.Create((byte[])reply.EncodedResponse))
			{
				int num = (int)protoBinaryReader.ReadByte();
				switch (num)
				{
				case 0:
					result = ClientServerQueueTicket.Supplier().Deserialize(protoBinaryReader);
					break;
				case 1:
					result = ClientServerQueueFinal.Supplier().Deserialize(protoBinaryReader);
					break;
				case 2:
					result = ClientServerQueueFailure.Supplier().Deserialize(protoBinaryReader);
					break;
				case 3:
					result = ClientServerQueueStatus.Supplier().Deserialize(protoBinaryReader);
					break;
				case 4:
					result = ClientServerQueueWorldTransfer.Supplier().Deserialize(protoBinaryReader);
					break;
				default:
					throw new IOException("Unknown queue response " + num.ToString());
				}
			}
			return result;
		}
	}
}
