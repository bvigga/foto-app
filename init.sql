CREATE DATABASE "FotoquestDB";
CREATE USER zee WITH PASSWORD 'ZoopLe!1$~2';
GRANT ALL PRIVILEGES ON DATABASE "FotoquestDB" to zee;
USE FotoquestDB;
if not exists (select * from sysobjects where name='Fotos' and xtype='U')
    create table Fotos (
        Id Guid PRIMARY,
        GeoDirection varchar(64) not null,
        Latitude decimal null,
        Longitude decimal null,
        Width int null,
        Height int null,
        ImageUrl varchar(255) null,
        DateCreated Date null
    )
go;
