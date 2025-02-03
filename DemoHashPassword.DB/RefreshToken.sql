CREATE TABLE [RefreshToken] (
	[Id] INTEGER PRIMARY KEY IDENTITY(1,1),
    [UserId] INTEGER NOT NULL,
    [Token] VARCHAR(max) NOT NULL,
    [ExpiredAt] DATETIME NOT NULL,
    [Used] BIT NOT NULL DEFAULT 0,

	CONSTRAINT fk_refreshToken_userId FOREIGN KEY ([UserId]) REFERENCES [User]([Id])
);