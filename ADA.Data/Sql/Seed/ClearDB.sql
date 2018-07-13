
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'GetMediaFond') DROP PROCEDURE [dbo].[GetMediaFond];
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ProcessTags') DROP PROCEDURE [dbo].[ProcessTags];
IF EXISTS (SELECT * FROM sys.objects WHERE type = 'P' AND name = 'ImportFond') DROP PROCEDURE [dbo].[ImportFond];

IF TYPE_ID(N'ListInt') IS NOT NULL DROP TYPE dbo.ListInt;
IF TYPE_ID(N'FondInformationValue') IS NOT NULL DROP TYPE dbo.FondInformationValue;
-- Types

CREATE TYPE [dbo].[ListInt] AS TABLE(
	[ID] [int] NULL
);

CREATE TYPE [dbo].[FondInformationValue] AS TABLE(
	[Id] [int] NOT NULL,
	[Value] [nvarchar](2000) NULL
);


