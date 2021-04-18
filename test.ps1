Set-Location Contract.Architecture\Contract.Architecture.Backends\Contract.Architecture.Backend.Core

contractor add domain GegönntesBankwesen
contractor add entity GegönntesBankwesen.GegönnteBank:GegönnteBanken
contractor add property string:256 Name -e GegönntesBankwesen.GegönnteBank:GegönnteBanken
contractor add property string:256 GegönnterName -e GegönntesBankwesen.GegönnteBank:GegönnteBanken
contractor add property boolean GegönnterBoolean -e GegönntesBankwesen.GegönnteBank:GegönnteBanken
contractor add property datetime GegönntesDateTime -e GegönntesBankwesen.GegönnteBank:GegönnteBanken
contractor add property double GegönnterDouble -e GegönntesBankwesen.GegönnteBank:GegönnteBanken
contractor add property guid GegönnteGuid -e GegönntesBankwesen.GegönnteBank:GegönnteBanken
contractor add property integer GegönnterInteger -e GegönntesBankwesen.GegönnteBank:GegönnteBanken

contractor add domain GegönnterKundenstamm
contractor add entity GegönnterKundenstamm.GegönnterKunde:GegönnteKunden
contractor add property string:256 Name -e GegönnterKundenstamm.GegönnterKunde:GegönnteKunden
contractor add property string:256 GegönnterName -e GegönnterKundenstamm.GegönnterKunde:GegönnteKunden -o
contractor add property boolean GegönnterBoolean -e GegönnterKundenstamm.GegönnterKunde:GegönnteKunden -o
contractor add property datetime GegönntesDateTime -e GegönnterKundenstamm.GegönnterKunde:GegönnteKunden -o
contractor add property double GegönnterDouble -e GegönnterKundenstamm.GegönnterKunde:GegönnteKunden -o
contractor add property guid GegönnteGuid -e GegönnterKundenstamm.GegönnterKunde:GegönnteKunden -o
contractor add property integer GegönnterInteger -e GegönnterKundenstamm.GegönnterKunde:GegönnteKunden -o

contractor add relation 1:n GegönntesBankwesen.GegönnteBank:GegönnteBanken GegönnterKundenstamm.GegönnterKunde:GegönnteKunden -n BesteBank:BesteKunden

Set-Location ..\..\..
