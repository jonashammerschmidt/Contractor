Set-Location Contract.Architecture\Contract.Architecture.Backend

contractor add domain Bankwesen
contractor add entity Bankwesen.Bank:Banken
contractor add property string:256 Name -e Bankwesen.Bank:Banken
contractor add property bool IsLiquide -e Bankwesen.Bank:Banken

contractor add domain Kundenstamm
contractor add entity Kundenstamm.Kunde:Kunden
contractor add property string:256 Name -e Kundenstamm.Kunde:Kunden
contractor add property DateTime Geburtstag -e Kundenstamm.Kunde:Kunden

contractor add relation 1:n Bankwesen.Bank:Banken Kundenstamm.Kunde:Kunden

Set-Location ..\..