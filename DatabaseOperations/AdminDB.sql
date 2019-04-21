CREATE TABLE [dbo].[LOGIN] (
    [Id]        INT         IDENTITY (1, 1) NOT NULL,
    [EMPID]     INT         NOT NULL,
    [LOGINTIME] DATETIME    NOT NULL,
    [Dbstate]   NUMERIC (1) DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[ORIGINALPRICE] (
    [Id]                INT           IDENTITY (1, 1) NOT NULL,
    [ORIGINALPRICEDATA] VARCHAR (MAX) NOT NULL,
    [DATE]              DATE          NOT NULL,
    [Dbstate]           NUMERIC (1)   DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);

CREATE TABLE [dbo].[DISCOUNT] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [DISCOUNTDATA] VARCHAR (MAX) NOT NULL,
    [DATE]         DATE          NOT NULL,
    [Dbstate]      NUMERIC (1)   DEFAULT ((1)) NOT NULL,
    PRIMARY KEY CLUSTERED ([Id] ASC)
);