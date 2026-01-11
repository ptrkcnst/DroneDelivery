# DroneDelivery - Mobile + Web API (SQLite)

Pachetul contine:
- `DroneDelivery.WebApi` – ASP.NET Core Web API + SQLite (DB local in fisier `drone_delivery.db`)
- `DroneDelivery.Mobile` – .NET MAUI (Android)

Aplicatia mobila poate rula in **Mock mode** (fara backend) sau conectata la API real.

## 1) Backend (Web API)

### Pornire

Din terminal, in folderul pachetului:

```bash
cd DroneDelivery.WebApi
dotnet restore
dotnet run
```

API va porni pe `http://localhost:5000`.

### SQLite

La prima rulare se creeaza automat fisierul `drone_delivery.db` in folderul `DroneDelivery.WebApi` (cu tabelele).

### Test rapid

Deschide Swagger:
- `http://localhost:5000/swagger`

## 2) Mobile (MAUI Android)

### URL corect pentru emulator Android

Din emulator, `localhost` al PC-ului este `10.0.2.2`.

Exemplu:
- backend pe PC: `http://localhost:5000`
- in aplicatie: `http://10.0.2.2:5000`

### Setari in aplicatie

In `Settings` ai:
- **Use mock backend** (ON = fara backend)
- **API Base URL** (ex: `http://10.0.2.2:5000`)

Apasa **Save**, apoi revino in celelalte tab-uri si testele CRUD vor folosi sursa selectata.

### Pornire (CLI)

```bash
cd DroneDelivery.Mobile
dotnet restore
dotnet build -f net8.0-android
dotnet run -f net8.0-android
```

## Endpoints folosite de Mobile

- `POST /api/auth/login`
- `GET/POST/PUT/DELETE /api/addresses`
- `GET/POST/PUT/DELETE /api/orders`
- `GET/POST/PUT/DELETE /api/notification-rules`
- `GET/POST/PUT/DELETE /api/notifications`
