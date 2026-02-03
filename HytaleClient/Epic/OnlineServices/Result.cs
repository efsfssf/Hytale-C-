using System;

namespace Epic.OnlineServices
{
	// Token: 0x02000021 RID: 33
	public enum Result
	{
		// Token: 0x0400005B RID: 91
		Success,
		// Token: 0x0400005C RID: 92
		NoConnection,
		// Token: 0x0400005D RID: 93
		InvalidCredentials,
		// Token: 0x0400005E RID: 94
		InvalidUser,
		// Token: 0x0400005F RID: 95
		InvalidAuth,
		// Token: 0x04000060 RID: 96
		AccessDenied,
		// Token: 0x04000061 RID: 97
		MissingPermissions,
		// Token: 0x04000062 RID: 98
		TokenNotAccount,
		// Token: 0x04000063 RID: 99
		TooManyRequests,
		// Token: 0x04000064 RID: 100
		AlreadyPending,
		// Token: 0x04000065 RID: 101
		InvalidParameters,
		// Token: 0x04000066 RID: 102
		InvalidRequest,
		// Token: 0x04000067 RID: 103
		UnrecognizedResponse,
		// Token: 0x04000068 RID: 104
		IncompatibleVersion,
		// Token: 0x04000069 RID: 105
		NotConfigured,
		// Token: 0x0400006A RID: 106
		AlreadyConfigured,
		// Token: 0x0400006B RID: 107
		NotImplemented,
		// Token: 0x0400006C RID: 108
		Canceled,
		// Token: 0x0400006D RID: 109
		NotFound,
		// Token: 0x0400006E RID: 110
		OperationWillRetry,
		// Token: 0x0400006F RID: 111
		NoChange,
		// Token: 0x04000070 RID: 112
		VersionMismatch,
		// Token: 0x04000071 RID: 113
		LimitExceeded,
		// Token: 0x04000072 RID: 114
		Disabled,
		// Token: 0x04000073 RID: 115
		DuplicateNotAllowed,
		// Token: 0x04000074 RID: 116
		MissingParametersDEPRECATED,
		// Token: 0x04000075 RID: 117
		InvalidSandboxId,
		// Token: 0x04000076 RID: 118
		TimedOut,
		// Token: 0x04000077 RID: 119
		PartialResult,
		// Token: 0x04000078 RID: 120
		MissingRole,
		// Token: 0x04000079 RID: 121
		MissingFeature,
		// Token: 0x0400007A RID: 122
		InvalidSandbox,
		// Token: 0x0400007B RID: 123
		InvalidDeployment,
		// Token: 0x0400007C RID: 124
		InvalidProduct,
		// Token: 0x0400007D RID: 125
		InvalidProductUserID,
		// Token: 0x0400007E RID: 126
		ServiceFailure,
		// Token: 0x0400007F RID: 127
		CacheDirectoryMissing,
		// Token: 0x04000080 RID: 128
		CacheDirectoryInvalid,
		// Token: 0x04000081 RID: 129
		InvalidState,
		// Token: 0x04000082 RID: 130
		RequestInProgress,
		// Token: 0x04000083 RID: 131
		ApplicationSuspended,
		// Token: 0x04000084 RID: 132
		NetworkDisconnected,
		// Token: 0x04000085 RID: 133
		AuthAccountLocked = 1001,
		// Token: 0x04000086 RID: 134
		AuthAccountLockedForUpdate,
		// Token: 0x04000087 RID: 135
		AuthInvalidRefreshToken,
		// Token: 0x04000088 RID: 136
		AuthInvalidToken,
		// Token: 0x04000089 RID: 137
		AuthAuthenticationFailure,
		// Token: 0x0400008A RID: 138
		AuthInvalidPlatformToken,
		// Token: 0x0400008B RID: 139
		AuthWrongAccount,
		// Token: 0x0400008C RID: 140
		AuthWrongClient,
		// Token: 0x0400008D RID: 141
		AuthFullAccountRequired,
		// Token: 0x0400008E RID: 142
		AuthHeadlessAccountRequired,
		// Token: 0x0400008F RID: 143
		AuthPasswordResetRequired,
		// Token: 0x04000090 RID: 144
		AuthPasswordCannotBeReused,
		// Token: 0x04000091 RID: 145
		AuthExpired,
		// Token: 0x04000092 RID: 146
		AuthScopeConsentRequired,
		// Token: 0x04000093 RID: 147
		AuthApplicationNotFound,
		// Token: 0x04000094 RID: 148
		AuthScopeNotFound,
		// Token: 0x04000095 RID: 149
		AuthAccountFeatureRestricted,
		// Token: 0x04000096 RID: 150
		AuthAccountPortalLoadError,
		// Token: 0x04000097 RID: 151
		AuthCorrectiveActionRequired,
		// Token: 0x04000098 RID: 152
		AuthPinGrantCode,
		// Token: 0x04000099 RID: 153
		AuthPinGrantExpired,
		// Token: 0x0400009A RID: 154
		AuthPinGrantPending,
		// Token: 0x0400009B RID: 155
		AuthExternalAuthNotLinked = 1030,
		// Token: 0x0400009C RID: 156
		AuthExternalAuthRevoked = 1032,
		// Token: 0x0400009D RID: 157
		AuthExternalAuthInvalid,
		// Token: 0x0400009E RID: 158
		AuthExternalAuthRestricted,
		// Token: 0x0400009F RID: 159
		AuthExternalAuthCannotLogin,
		// Token: 0x040000A0 RID: 160
		AuthExternalAuthExpired,
		// Token: 0x040000A1 RID: 161
		AuthExternalAuthIsLastLoginType,
		// Token: 0x040000A2 RID: 162
		AuthExchangeCodeNotFound = 1040,
		// Token: 0x040000A3 RID: 163
		AuthOriginatingExchangeCodeSessionExpired,
		// Token: 0x040000A4 RID: 164
		AuthAccountNotActive = 1050,
		// Token: 0x040000A5 RID: 165
		AuthMFARequired = 1060,
		// Token: 0x040000A6 RID: 166
		AuthParentalControls = 1070,
		// Token: 0x040000A7 RID: 167
		AuthNoRealId = 1080,
		// Token: 0x040000A8 RID: 168
		AuthUserInterfaceRequired = 1090,
		// Token: 0x040000A9 RID: 169
		FriendsInviteAwaitingAcceptance = 2000,
		// Token: 0x040000AA RID: 170
		FriendsNoInvitation,
		// Token: 0x040000AB RID: 171
		FriendsAlreadyFriends = 2003,
		// Token: 0x040000AC RID: 172
		FriendsNotFriends,
		// Token: 0x040000AD RID: 173
		FriendsTargetUserTooManyInvites,
		// Token: 0x040000AE RID: 174
		FriendsLocalUserTooManyInvites,
		// Token: 0x040000AF RID: 175
		FriendsTargetUserFriendLimitExceeded,
		// Token: 0x040000B0 RID: 176
		FriendsLocalUserFriendLimitExceeded,
		// Token: 0x040000B1 RID: 177
		PresenceDataInvalid = 3000,
		// Token: 0x040000B2 RID: 178
		PresenceDataLengthInvalid,
		// Token: 0x040000B3 RID: 179
		PresenceDataKeyInvalid,
		// Token: 0x040000B4 RID: 180
		PresenceDataKeyLengthInvalid,
		// Token: 0x040000B5 RID: 181
		PresenceDataValueInvalid,
		// Token: 0x040000B6 RID: 182
		PresenceDataValueLengthInvalid,
		// Token: 0x040000B7 RID: 183
		PresenceRichTextInvalid,
		// Token: 0x040000B8 RID: 184
		PresenceRichTextLengthInvalid,
		// Token: 0x040000B9 RID: 185
		PresenceStatusInvalid,
		// Token: 0x040000BA RID: 186
		EcomEntitlementStale = 4000,
		// Token: 0x040000BB RID: 187
		EcomCatalogOfferStale,
		// Token: 0x040000BC RID: 188
		EcomCatalogItemStale,
		// Token: 0x040000BD RID: 189
		EcomCatalogOfferPriceInvalid,
		// Token: 0x040000BE RID: 190
		EcomCheckoutLoadError,
		// Token: 0x040000BF RID: 191
		EcomPurchaseProcessing,
		// Token: 0x040000C0 RID: 192
		SessionsSessionInProgress = 5000,
		// Token: 0x040000C1 RID: 193
		SessionsTooManyPlayers,
		// Token: 0x040000C2 RID: 194
		SessionsNoPermission,
		// Token: 0x040000C3 RID: 195
		SessionsSessionAlreadyExists,
		// Token: 0x040000C4 RID: 196
		SessionsInvalidLock,
		// Token: 0x040000C5 RID: 197
		SessionsInvalidSession,
		// Token: 0x040000C6 RID: 198
		SessionsSandboxNotAllowed,
		// Token: 0x040000C7 RID: 199
		SessionsInviteFailed,
		// Token: 0x040000C8 RID: 200
		SessionsInviteNotFound,
		// Token: 0x040000C9 RID: 201
		SessionsUpsertNotAllowed,
		// Token: 0x040000CA RID: 202
		SessionsAggregationFailed,
		// Token: 0x040000CB RID: 203
		SessionsHostAtCapacity,
		// Token: 0x040000CC RID: 204
		SessionsSandboxAtCapacity,
		// Token: 0x040000CD RID: 205
		SessionsSessionNotAnonymous,
		// Token: 0x040000CE RID: 206
		SessionsOutOfSync,
		// Token: 0x040000CF RID: 207
		SessionsTooManyInvites,
		// Token: 0x040000D0 RID: 208
		SessionsPresenceSessionExists,
		// Token: 0x040000D1 RID: 209
		SessionsDeploymentAtCapacity,
		// Token: 0x040000D2 RID: 210
		SessionsNotAllowed,
		// Token: 0x040000D3 RID: 211
		SessionsPlayerSanctioned,
		// Token: 0x040000D4 RID: 212
		PlayerDataStorageFilenameInvalid = 6000,
		// Token: 0x040000D5 RID: 213
		PlayerDataStorageFilenameLengthInvalid,
		// Token: 0x040000D6 RID: 214
		PlayerDataStorageFilenameInvalidChars,
		// Token: 0x040000D7 RID: 215
		PlayerDataStorageFileSizeTooLarge,
		// Token: 0x040000D8 RID: 216
		PlayerDataStorageFileSizeInvalid,
		// Token: 0x040000D9 RID: 217
		PlayerDataStorageFileHandleInvalid,
		// Token: 0x040000DA RID: 218
		PlayerDataStorageDataInvalid,
		// Token: 0x040000DB RID: 219
		PlayerDataStorageDataLengthInvalid,
		// Token: 0x040000DC RID: 220
		PlayerDataStorageStartIndexInvalid,
		// Token: 0x040000DD RID: 221
		PlayerDataStorageRequestInProgress,
		// Token: 0x040000DE RID: 222
		PlayerDataStorageUserThrottled,
		// Token: 0x040000DF RID: 223
		PlayerDataStorageEncryptionKeyNotSet,
		// Token: 0x040000E0 RID: 224
		PlayerDataStorageUserErrorFromDataCallback,
		// Token: 0x040000E1 RID: 225
		PlayerDataStorageFileHeaderHasNewerVersion,
		// Token: 0x040000E2 RID: 226
		PlayerDataStorageFileCorrupted,
		// Token: 0x040000E3 RID: 227
		ConnectExternalTokenValidationFailed = 7000,
		// Token: 0x040000E4 RID: 228
		ConnectUserAlreadyExists,
		// Token: 0x040000E5 RID: 229
		ConnectAuthExpired,
		// Token: 0x040000E6 RID: 230
		ConnectInvalidToken,
		// Token: 0x040000E7 RID: 231
		ConnectUnsupportedTokenType,
		// Token: 0x040000E8 RID: 232
		ConnectLinkAccountFailed,
		// Token: 0x040000E9 RID: 233
		ConnectExternalServiceUnavailable,
		// Token: 0x040000EA RID: 234
		ConnectExternalServiceConfigurationFailure,
		// Token: 0x040000EB RID: 235
		ConnectLinkAccountFailedMissingNintendoIdAccountDEPRECATED,
		// Token: 0x040000EC RID: 236
		SocialOverlayLoadError = 8000,
		// Token: 0x040000ED RID: 237
		InconsistentVirtualMemoryFunctions,
		// Token: 0x040000EE RID: 238
		LobbyNotOwner = 9000,
		// Token: 0x040000EF RID: 239
		LobbyInvalidLock,
		// Token: 0x040000F0 RID: 240
		LobbyLobbyAlreadyExists,
		// Token: 0x040000F1 RID: 241
		LobbySessionInProgress,
		// Token: 0x040000F2 RID: 242
		LobbyTooManyPlayers,
		// Token: 0x040000F3 RID: 243
		LobbyNoPermission,
		// Token: 0x040000F4 RID: 244
		LobbyInvalidSession,
		// Token: 0x040000F5 RID: 245
		LobbySandboxNotAllowed,
		// Token: 0x040000F6 RID: 246
		LobbyInviteFailed,
		// Token: 0x040000F7 RID: 247
		LobbyInviteNotFound,
		// Token: 0x040000F8 RID: 248
		LobbyUpsertNotAllowed,
		// Token: 0x040000F9 RID: 249
		LobbyAggregationFailed,
		// Token: 0x040000FA RID: 250
		LobbyHostAtCapacity,
		// Token: 0x040000FB RID: 251
		LobbySandboxAtCapacity,
		// Token: 0x040000FC RID: 252
		LobbyTooManyInvites,
		// Token: 0x040000FD RID: 253
		LobbyDeploymentAtCapacity,
		// Token: 0x040000FE RID: 254
		LobbyNotAllowed,
		// Token: 0x040000FF RID: 255
		LobbyMemberUpdateOnly,
		// Token: 0x04000100 RID: 256
		LobbyPresenceLobbyExists,
		// Token: 0x04000101 RID: 257
		LobbyVoiceNotEnabled,
		// Token: 0x04000102 RID: 258
		LobbyPlatformNotAllowed,
		// Token: 0x04000103 RID: 259
		TitleStorageUserErrorFromDataCallback = 10000,
		// Token: 0x04000104 RID: 260
		TitleStorageEncryptionKeyNotSet,
		// Token: 0x04000105 RID: 261
		TitleStorageFileCorrupted,
		// Token: 0x04000106 RID: 262
		TitleStorageFileHeaderHasNewerVersion,
		// Token: 0x04000107 RID: 263
		ModsModSdkProcessIsAlreadyRunning = 11000,
		// Token: 0x04000108 RID: 264
		ModsModSdkCommandIsEmpty,
		// Token: 0x04000109 RID: 265
		ModsModSdkProcessCreationFailed,
		// Token: 0x0400010A RID: 266
		ModsCriticalError,
		// Token: 0x0400010B RID: 267
		ModsToolInternalError,
		// Token: 0x0400010C RID: 268
		ModsIPCFailure,
		// Token: 0x0400010D RID: 269
		ModsInvalidIPCResponse,
		// Token: 0x0400010E RID: 270
		ModsURILaunchFailure,
		// Token: 0x0400010F RID: 271
		ModsModIsNotInstalled,
		// Token: 0x04000110 RID: 272
		ModsUserDoesNotOwnTheGame,
		// Token: 0x04000111 RID: 273
		ModsOfferRequestByIdInvalidResult,
		// Token: 0x04000112 RID: 274
		ModsCouldNotFindOffer,
		// Token: 0x04000113 RID: 275
		ModsOfferRequestByIdFailure,
		// Token: 0x04000114 RID: 276
		ModsPurchaseFailure,
		// Token: 0x04000115 RID: 277
		ModsInvalidGameInstallInfo,
		// Token: 0x04000116 RID: 278
		ModsCannotGetManifestLocation,
		// Token: 0x04000117 RID: 279
		ModsUnsupportedOS,
		// Token: 0x04000118 RID: 280
		AntiCheatClientProtectionNotAvailable = 12000,
		// Token: 0x04000119 RID: 281
		AntiCheatInvalidMode,
		// Token: 0x0400011A RID: 282
		AntiCheatClientProductIdMismatch,
		// Token: 0x0400011B RID: 283
		AntiCheatClientSandboxIdMismatch,
		// Token: 0x0400011C RID: 284
		AntiCheatProtectMessageSessionKeyRequired,
		// Token: 0x0400011D RID: 285
		AntiCheatProtectMessageValidationFailed,
		// Token: 0x0400011E RID: 286
		AntiCheatProtectMessageInitializationFailed,
		// Token: 0x0400011F RID: 287
		AntiCheatPeerAlreadyRegistered,
		// Token: 0x04000120 RID: 288
		AntiCheatPeerNotFound,
		// Token: 0x04000121 RID: 289
		AntiCheatPeerNotProtected,
		// Token: 0x04000122 RID: 290
		AntiCheatClientDeploymentIdMismatch,
		// Token: 0x04000123 RID: 291
		AntiCheatDeviceIdAuthIsNotSupported,
		// Token: 0x04000124 RID: 292
		TooManyParticipants = 13000,
		// Token: 0x04000125 RID: 293
		RoomAlreadyExists,
		// Token: 0x04000126 RID: 294
		UserKicked,
		// Token: 0x04000127 RID: 295
		UserBanned,
		// Token: 0x04000128 RID: 296
		RoomWasLeft,
		// Token: 0x04000129 RID: 297
		ReconnectionTimegateExpired,
		// Token: 0x0400012A RID: 298
		ShutdownInvoked,
		// Token: 0x0400012B RID: 299
		UserIsInBlocklist,
		// Token: 0x0400012C RID: 300
		ProgressionSnapshotSnapshotIdUnavailable = 14000,
		// Token: 0x0400012D RID: 301
		ParentEmailMissing = 15000,
		// Token: 0x0400012E RID: 302
		UserGraduated,
		// Token: 0x0400012F RID: 303
		AndroidJavaVMNotStored = 17000,
		// Token: 0x04000130 RID: 304
		AndroidReservedMustReferenceLocalVM,
		// Token: 0x04000131 RID: 305
		AndroidReservedMustBeNull,
		// Token: 0x04000132 RID: 306
		PermissionRequiredPatchAvailable = 18000,
		// Token: 0x04000133 RID: 307
		PermissionRequiredSystemUpdate,
		// Token: 0x04000134 RID: 308
		PermissionAgeRestrictionFailure,
		// Token: 0x04000135 RID: 309
		PermissionAccountTypeFailure,
		// Token: 0x04000136 RID: 310
		PermissionChatRestriction,
		// Token: 0x04000137 RID: 311
		PermissionUGCRestriction,
		// Token: 0x04000138 RID: 312
		PermissionOnlinePlayRestricted,
		// Token: 0x04000139 RID: 313
		DesktopCrossplayApplicationNotBootstrapped = 19000,
		// Token: 0x0400013A RID: 314
		DesktopCrossplayServiceNotInstalled,
		// Token: 0x0400013B RID: 315
		DesktopCrossplayServiceStartFailed,
		// Token: 0x0400013C RID: 316
		DesktopCrossplayServiceNotRunning,
		// Token: 0x0400013D RID: 317
		CustomInvitesInviteFailed = 20000,
		// Token: 0x0400013E RID: 318
		UserInfoBestDisplayNameIndeterminate = 22000,
		// Token: 0x0400013F RID: 319
		ConsoleInitOnNetworkRequestedDeprecatedCallbackNotSet = 23000,
		// Token: 0x04000140 RID: 320
		ConsoleInitCacheStorageSizeKBNotMultipleOf16,
		// Token: 0x04000141 RID: 321
		ConsoleInitCacheStorageSizeKBBelowMinimumSize,
		// Token: 0x04000142 RID: 322
		ConsoleInitCacheStorageSizeKBExceedsMaximumSize,
		// Token: 0x04000143 RID: 323
		ConsoleInitCacheStorageIndexOutOfRangeRange,
		// Token: 0x04000144 RID: 324
		UnexpectedError = 2147483647
	}
}
