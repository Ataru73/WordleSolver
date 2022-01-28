# Wordle Solver
This little program helps solve Wordle puzzles.
## Syntax
    Wordle <word list> <hints> [sort]
* 'word list' is a list of words in the language of interest. It doesn't have to consist only of 5 letter words, the program will filter them. It helps if the list is already ordered by word frequency. If not you can provide the 'sort' argument to sort it for charachter frequency (words with more common charachters will come first).
The list I'm currently using can be downloaded here: 
https://github.com/IlyaSemenov/wikipedia-word-frequency/blob/master/results/enwiki-20190320-words-frequency.txt

* 'hints' is a text file with the hints Wordle gives you. The format is explained in a later section.
* 'sort', if present, sorts the list for charachter frequency.
## Hints file format
This is am example hints.txt file:

    adieu: 0 0 1 0 0
    march: 0 1 1 1 0

Every line is a hint.
The part before the colon is the word that was given to Wordle.
The numbers meaning is:
* 0: gray letter (not present in the hidden word)
* 1: yellow letter (present, but in a different position)
* 2: green letter (present in that position)
## Usage
For every word you try in Wordle just add the word and the hint to the hints file. The program will output the most likely word to try next which is compatible with all the hints. Other 10 compatible words are also given as output.
### Example outupt
    Most likely word: crazy
    Other possibilities: crabs, crawl, crank, crags, crass, crabb, cragg, craps, crary, crapo

