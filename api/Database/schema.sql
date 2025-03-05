IF DB_ID('tf_bounty') IS NULL
BEGIN
    CREATE DATABASE tf_bounty;
END

GO

USE [tf_bounty];

IF OBJECT_ID('Users', 'U') IS NULL
BEGIN
    CREATE TABLE [Users](
        [id]            [int] IDENTITY(1,1) NOT NULL,
        [oid]           [nvarchar](100) NOT NULL,
        [points]        [int]

        CONSTRAINT PK_user_id PRIMARY KEY CLUSTERED([id]),
        CONSTRAINT U_user_oid UNIQUE([oid])
    );
END;

IF OBJECT_ID('Programs', 'U') IS NULL
BEGIN
    CREATE TABLE [Programs](
        [id]            [int] IDENTITY(1,1) NOT NULL,
        [creator]       [int] NOT NULL,
        [logo]          [nvarchar](100),
        [reward]        [int]

        CONSTRAINT PK_Program_id PRIMARY KEY CLUSTERED([id]),
        CONSTRAINT FK_Program_creator FOREIGN KEY([creator]) REFERENCES Users([id])
            ON DELETE CASCADE 
			ON UPDATE CASCADE
    );
END;

IF OBJECT_ID('Rapports', 'U') IS NULL
BEGIN
    CREATE TABLE [Rapports](
        [id]            [int] IDENTITY(1,1) NOT NULL,
        [creator]       [int] NOT NULL,
        [programId]     [int] NOT NULL,
        [content]       [text],
        [status]        [varchar] NOT NULL,
        [createdAt]     [datetime] CONSTRAINT DF_Rapport_createdAt DEFAULT GETDATE() ,

        CONSTRAINT PK_Rapport_Id PRIMARY KEY CLUSTERED([id]),
        CONSTRAINT FK_Rapport_creator FOREIGN KEY([creator]) REFERENCES Users([id])
            ON DELETE CASCADE 
			ON UPDATE CASCADE,

        CONSTRAINT FK_Rapport_ProgramId FOREIGN KEY([programId]) REFERENCES Programs([id])
            ON DELETE NO ACTION 
			ON UPDATE CASCADE,
        
        CONSTRAINT CK_Rapport_Status CHECK(
	        [status] = 'pending' OR 
	        [status] = 'validated' OR
            [status] = 'rejected'
		)
    );
END;

IF OBJECT_ID('PendingRapports', 'U') IS NULL
BEGIN
    CREATE TABLE [PendingRapports](
        [rapportId]     [int] NOT NULL,

        CONSTRAINT FK_Pending_RapportId FOREIGN KEY([rapportId]) REFERENCES Rapports([id])
            ON DELETE CASCADE 
			ON UPDATE CASCADE,
    );
END;