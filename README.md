## Autor
* **Estera Morozova** - nr albumu: 15743

## Link do repozytorium
* GitHub: (https://github.com/esteramorozova/PabLaboratory26.1.git)

## Lista zrealizowanych funkcji

Projekt został podzielony na warstwy (AppCore, Infrastructure, WebApi, UnitTest) i realizuje następujące zadania:

1. **Baza danych SQLite oraz EF Core:** Pełne wsparcie dla operacji na kontaktach przechowywanych lokalnie w pliku bazy danych.
2. **Rejestracja twórcy kontaktu:** Każdy dodawany kontakt automatycznie zapisuje ID użytkownika, który go dodał.
3. **Autoryzacja i ochrona zasobów:** Edycja i usuwanie kontaktów są zabezpieczone – operacje te może wykonać wyłącznie autor danego kontaktu lub Administrator.
4. **Uwierzytelnianie JWT & Refresh Token:** Logowanie generuje bezpieczny token JWT, a system obsługuje bezpieczne odświeżanie sesji przez Refresh Tokeny zapisywane w bazie.
5. **Moduł Administracyjny (Admin API):** Kontroler zarządzający kontami użytkowników (blokowanie, odblokowywanie, aktywacja, dezaktywacja oraz nadawanie/odbieranie ról).
6. **Obiekt Wartości (Value Object):** Klasa `EmailAddress` samodzielnie waliduje adresy e-mail i wyciąga z nich nazwę użytkownika oraz domenę.
7. **Seedowanie i testy:** Automatyczne tworzenie ról i użytkowników testowych przy starcie aplikacji oraz komplet testów integracyjnych przechodzących pomyślnie.
