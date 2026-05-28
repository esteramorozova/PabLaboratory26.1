## Autor
* **Estera Morozova** - nr albumu: 15743

## Link do repozytorium
* GitHub: (https://github.com/esteramorozova/PabLaboratory26.1.git)

## Lista zrealizowanych funkcji

### Architektura
- Podział na warstwy: `AppCore`, `Infrastructure`, `WebApi`, `UnitTest`
- Wzorzec Repository i Unit of Work
- Implementacja in-memory (dla testów) oraz EF Core (produkcja)

### Baza danych
- Implementacja EF Core z bazą SQLite
- Migracje bazy danych
- Mapowanie TPH (Table Per Hierarchy) dla kontaktów
- Konfiguracja relacji między encjami

### Kontakty
- CRUD dla kontaktów typu `Person`
- Dodawanie i usuwanie notatek do kontaktów
- Tagowanie kontaktów
- Paginacja wyników
- Każdy kontakt zapisuje ID użytkownika który go dodał (`CreatedByUserId`)
- Edycja i usuwanie kontaktu możliwe tylko przez twórcę lub administratora

### Użytkownicy i role
- ASP.NET Identity z niestandardowymi klasami `CrmUser` i `CrmRole`
- Role: `Administrator`, `SalesManager`, `Salesperson`, `SupportAgent`, `ReadOnly`
- Polityki autoryzacji (`AdminOnly`, `SalesAccess`, `SalesManagerAccess`, `SupportAccess`, `ReadOnlyAccess`)
- Aktywacja i dezaktywacja kont użytkowników

### Uwierzytelnianie JWT
- Logowanie z generowaniem tokenu JWT
- Refresh Token zapisywany w bazie
- Odświeżanie tokenu dostępu
- Unieważnianie tokenu (wylogowanie)
- Endpoint `/api/auth/me` zwracający dane zalogowanego użytkownika

### Moduł administracyjny
- Pobieranie listy wszystkich użytkowników
- Pobieranie użytkownika po ID
- Przypisywanie i cofanie ról
- Blokowanie i odblokowywanie kont
- Dezaktywacja i aktywacja użytkowników
- Usuwanie użytkowników

### Seedowanie danych
- `IdentityDbSeeder` — automatyczne tworzenie ról i użytkowników przy starcie
- `ContactsDbSeeder` — przykładowe kontakty typu Person

### Value Object
- `EmailAddress` — walidacja, parsowanie, odczyt użytkownika i domeny, formatowanie

### Obsługa błędów
- Globalny handler wyjątków (`ProblemDetailsExceptionHandler`)
- Własne wyjątki domenowe (`ContactNotFoundException`, `UserNotFoundException`)
- Poprawne kody HTTP (`400`, `401`, `403`, `404`, `204`)

### Testy
- Testy jednostkowe dla `MemoryGenericRepository`
- Testy jednostkowe dla `EmailAddress` (37 testów łącznie)
- Testy integracyjne dla uwierzytelniania i autoryzacji (`AuthIntegrationTest`)

