# Wordle Solver
This little program helps solve wordle puzzles.
## Syntax
    Wordle <word list> <hints> [sort]
* <word list> is a list of words in the language of interest. It doesn't have to consist only of 5 letter words, the program will filter them. If the list is already ordered by word frequency helps, if not you can use the 'sort' argument to sort it for letter frequency.
The list I'm currently using can be downloaded here: 
https://github.com/IlyaSemenov/wikipedia-word-frequency/blob/master/results/enwiki-20190320-words-frequency.txt

* <hints> is a text file with the hints wordle gives you. The format is explained in a later section.
* [sort] if present sorts the list for letter frequency.
## Hints file format
This is am example hints.txt file:

    adieu: 0 0 1 0 0
    march: 0 1 1 1 0

Every line is a hint.
The part before the colon is the word that was given to Wordle.
The numbers meaning is:
* 0: gray letter (not present in the hidden word)
* 1: yellow letter (present, but in a wrong position)
* 2: green letter (present in that position)
