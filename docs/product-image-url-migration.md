# Product ImageUrl migration

`AddProductImageUrl` adds nullable PostgreSQL `text` column `Products.ImageUrl`.
Existing products keep their values and receive NULL for the new column.

The application originally created its tables with `EnsureCreatedAsync`, so this
first migration assumes the application tables already exist. Its model snapshot
records the current model for future migrations; it does not recreate the tables.
For a new empty database, initialize the schema through the application first.
`IF NOT EXISTS` allows the migration to be recorded when `EnsureCreatedAsync`
has already created `ImageUrl` from the current model.

From the repository root, with dotnet-ef 10.0.0 installed and the configured
PostgreSQL database running:

```powershell
dotnet ef database update --project src/EloisStore.Api
```

Verify the column and migration history:

```sql
SELECT column_name, data_type, is_nullable
FROM information_schema.columns
WHERE table_schema = 'public' AND table_name = 'Products' AND column_name = 'ImageUrl';
SELECT * FROM "__EFMigrationsHistory";
```

Rolling back this migration drops `ImageUrl` and any image URLs stored in it.
