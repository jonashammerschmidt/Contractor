Set-Location Contract.Architecture\Contract.Architecture.Backend

contractor add domain Bankwesen
contractor add entity Bankwesen.Bank:Banken
contractor add property string:20 Typ -e Bankwesen.Bank:Banken

contractor add domain Kundenstamm
contractor add entity Kundenstamm.Kunde:Kunden
contractor add property string:256 Name -e Kundenstamm.Kunde:Kunden

contractor add domain Finanzsystem
contractor add entity Finanzsystem.Konto:Konten
contractor add property int Kontostand -e Finanzsystem.Konto:Konten

contractor add relation 1:n Bankwesen.Bank:Banken Kundenstamm.Kunde:Kunden
contractor add relation 1:n Kundenstamm.Kunde:Kunden Finanzsystem.Konto:Konten

Set-Location ..\..