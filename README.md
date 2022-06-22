### Setup project

Chạy lệnh

```
dotnet restore
```

-   để cài đặt các thư viện cần thiết

```
dotnet ef migrations add -o Data/Migrations InitialDb
dotnet ef update database
```

-   Chạy prj

```
dotnet run
```

-   Chạy prj với hot reload

```
dotnet run watch
```

### Controller

-   Tất cả api thì viết trong controller

### Data

### DTOs vs Entities

-   DTOs : phia fe gui ve
-   Entities: phia db

### Helper

-   Chứa các class tiện ích, trợ giúp
    -- AutoMapperProfiles.cs

### Services

## Interface

-   Khai báo class và các phương thức dưới dạng interface

## Implement

-   Kế thừa interface và triển khai code logic
