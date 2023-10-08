<!-- Kopfzeile -->
<p align="center">
  <img src="https://i.imgur.com/zmWGAFY.png" alt="Logo" height="150">

  <h2 align="center">Contractor</h2>

  <p align="center">
    <i>Eine CLI für die Contract Architektur.</i>
  </p>
</p>

# Allgemein

Der Contractor ist eine CLI um Domänen, Entitäten, Eigenschaften und Beziehungen zur Contract Architektur hinzuzufügen.

## Installation

```
dotnet tool install -g contractor
```

## Usage

```
contractor init [-y]
contractor execute <relative-path> [-v|--verbose]
contractor csv insert [-t|--login-type] [sql|integrated] [-s|--server-address] [-d|--database-name] [-u|--user] [-p|--password] [-v|--verbose]
contractor csv export [-t|--login-type] [sql|integrated] [-s|--server-address] [-d|--database-name] [-u|--user] [-p|--password] [-v|--verbose]
contractor migrate <relative-path>
```