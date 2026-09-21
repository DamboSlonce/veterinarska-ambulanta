# VetKeep – Veterinarska ambulanta

Desktop WPF aplikacija za evidenciju veterinarske ambulante (ljubimci, veterinari, termini, pregledi, lekovi).

**Dokumentacija:** [Dokumentacija/Dokumentacija-VetKeep.docx](Dokumentacija/Dokumentacija-VetKeep.docx)

## Pokretanje

1. Visual Studio 2019/2022 (workload **.NET desktop development**)
2. SQL Server Express (`localhost\SQLEXPRESS`) ili LocalDB
3. Otvoriti `Veterinarska Ambulanta WPF.sln`
4. F5

Baza `Veterinarska_Ambulanta` kreira se automatski pri prvom pokretanju.

**Prijava:** `Veterinar` / `1234`

## Arhitektura

- **Veterinarska Ambulanta WPF** – korisnički interfejs
- **SlojServisa** – poslovna pravila
- **SlojPodataka** – SQL Server
- **Klase** – entiteti
