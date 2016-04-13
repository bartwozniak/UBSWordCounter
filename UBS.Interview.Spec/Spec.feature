Feature: Word counts
	As an author
	I want to know the number of times each word appears in a sentence
	So that I can make sure I'm not repeating myself
	
Scenario: Count word occurrences
	Given a sentence "This is a statement, and so is this."
	When the program is run
	Then the result should be
		| Word      | Count |
		| this      | 2     |
		| is        | 2     |
		| a         | 1     |
		| statement | 1     |
		| and       | 1     |
		| so        | 1     |