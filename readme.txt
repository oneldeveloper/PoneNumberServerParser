Avviamento della demo:
Aprire la soluzione con visual studio ed avviare il progetto.

Alla pagina html è possibile inserire un record da verificare tra la lista di record forniti.

La demo mette a disposizione una API che permette di fare l'upload del file csv come payload di una richiesta http put
I risultati sono ottenuti tramite una sucessiva richiesta get, in formato json.
Ogni record json è formato dalle informazioni di input, dall'id del numero, il numero di telefono e l'esito dell'operazione di processo.

L'end point della api è  /api/phoneparser