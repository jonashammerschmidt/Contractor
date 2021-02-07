--------- New ---------
contractor new Contract.Architecture


--------- Init ---------
contractor init
    - Backend-Destination from .
    - DB-Destination From search in folder: .        ../.      ../../.
    - DB-ProjectName From DbDestinationFolderName
    - ProjectName From BackendDestinationFolderName
contractor init -y


--------- Domain ---------
contractor add domain Finanzen

contractor rename domain Finanzen:Finanzwesen
contractor rename domain Finanzen --to Finanzwesen


--------- Entity ---------
contractor add entity Bank:Banken -d Finanzen.Bankwesen
contractor add entity Bank -p Banken -d Finanzen.Bankwesen

contractor remove entity Bank -d Finanzen.Bankwesen


-------- Property --------
contractor add property string?(256) KundenId -e Finanzen.Bankwesen.Bank
contractor add property string KundenId -r --extra=256 -e Finanzen.Bankwesen.Bank
contractor add property string KundenId --required --extra=256 --entity Finanzen.Bankwesen.Bank

contractor rename property KundenId:KundenNummer -e Finanzen.Bankwesen.Bank
contractor rename property KundenId --to KundenNummer -e Finanzen.Bankwesen.Bank

contractor remove property Finanzen.Bankwesen.Bank.KundenId 
contractor remove property Bank.KundenId -d Finanzen.Bankwesen
contractor remove property KundenId -e Finanzen.Bankwesen.Bank

