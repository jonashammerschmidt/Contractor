Set-Location Contract.Architecture\Contract.Architecture.Backends\Contract.Architecture.Backend.Core

contractor add domain Bankwesen
contractor add entity Bankwesen.Bank:Banken
contractor add property string:256 Name -e Bankwesen.Bank:Banken
contractor add property DateTime EroeffnetAm -e Bankwesen.Bank:Banken
contractor add property bool IsPleite -e Bankwesen.Bank:Banken

contractor add entity Bankwesen.Konto:Konten
contractor add property string:256 Name -e Bankwesen.Konto:Konten
contractor add property DateTime EroeffnetAm -e Bankwesen.Konto:Konten

contractor add domain Kundenstamm
contractor add entity Kundenstamm.Kunde:Kunden
contractor add property string:256 Name -e Kundenstamm.Kunde:Kunden
contractor add property int Balance -e Kundenstamm.Kunde:Kunden

contractor add relation 1:n Bankwesen.Bank:Banken Kundenstamm.Kunde:Kunden
contractor add relation 1:n Bankwesen.Konto:Konten Kundenstamm.Kunde:Kunden

Set-Location ..\..\..