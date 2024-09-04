START TRANSACTION;

CREATE TABLE "CustomerManagements" (
    "Id" text NOT NULL,
    "Name" text NOT NULL,
    "PhoneNumber" text NOT NULL,
    "Email" text NOT NULL,
    "Cnic" text NOT NULL,
    "Billing" text NOT NULL,
    "Address" text NOT NULL,
    "CreatedBy" text NOT NULL,
    "UpdatedBy" text NOT NULL,
    "CreatedDate" timestamp with time zone NOT NULL,
    "UpdatedDate" timestamp with time zone NOT NULL,
    "IsActive" boolean NOT NULL,
    "IsArchived" boolean NOT NULL,
    CONSTRAINT "PK_CustomerManagements" PRIMARY KEY ("Id")
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20240904063137_addedcustomermanagementtable', '8.0.8');

COMMIT;

