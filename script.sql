CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "Locations" (
    "Id" text NOT NULL,
    "Name" text NOT NULL,
    "Address" text NOT NULL,
    "City" text NOT NULL,
    "Province" text NOT NULL,
    "CreatedBy" text NOT NULL,
    "UpdatedBy" text NOT NULL,
    "CreatedDate" timestamp with time zone NOT NULL,
    "UpdatedDate" timestamp with time zone NOT NULL,
    "IsActive" boolean NOT NULL,
    "IsArchived" boolean NOT NULL,
    CONSTRAINT "PK_Locations" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240829082501_initial_migration', '8.0.8');

COMMIT;

