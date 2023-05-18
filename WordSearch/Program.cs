using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace WordSearch
{
    class Program
    {
        static char[,] Grid = new char[,] {
            {'C', 'P', 'K', 'X', 'O', 'I', 'G', 'H', 'S', 'F', 'C', 'H'},
            {'Y', 'G', 'W', 'R', 'I', 'A', 'H', 'C', 'Q', 'R', 'X', 'K'},
            {'M', 'A', 'X', 'I', 'M', 'I', 'Z', 'A', 'T', 'I', 'O', 'N'},
            {'E', 'T', 'W', 'Z', 'N', 'L', 'W', 'G', 'E', 'D', 'Y', 'W'},
            {'M', 'C', 'L', 'E', 'L', 'D', 'N', 'V', 'L', 'G', 'P', 'T'},
            {'O', 'J', 'A', 'A', 'V', 'I', 'O', 'T', 'E', 'E', 'P', 'X'},
            {'C', 'D', 'B', 'P', 'H', 'I', 'A', 'W', 'V', 'X', 'U', 'I'},
            {'L', 'G', 'O', 'S', 'S', 'B', 'R', 'Q', 'I', 'A', 'P', 'K'},
            {'E', 'O', 'I', 'G', 'L', 'P', 'S', 'D', 'S', 'F', 'W', 'P'},
            {'W', 'F', 'K', 'E', 'G', 'O', 'L', 'F', 'I', 'F', 'R', 'S'},
            {'O', 'T', 'R', 'U', 'O', 'C', 'D', 'O', 'O', 'F', 'T', 'P'},
            {'C', 'A', 'R', 'P', 'E', 'T', 'R', 'W', 'N', 'G', 'V', 'Z'}
        };

        static string[] Words = new string[] 
        {
            "CARPET",
            "CHAIR",
            "DOG",
            "BALL",
            "DRIVEWAY",
            "FISHING",
            "FOODCOURT",
            "FRIDGE",
            "GOLF",
            "MAXIMIZATION",
            "PUPPY",
            "SPACE",
            "TABLE",
            "TELEVISION",
            "WELCOME",
            "WINDOW"
        };

        static void Main(string[] args)
        {
            Console.WriteLine("Word Search");

            for (int y = 0; y < 12; y++)
            {
                for (int x = 0; x < 12; x++)
                {
                    Console.Write(Grid[y, x]);
                    Console.Write(' ');
                }
                Console.WriteLine("");

            }

            Console.WriteLine("");
            Console.WriteLine("Found Words");
            Console.WriteLine("------------------------------");

            var foundWords = FindWords();
            
            foundWords.OrderBy(v => v.WordSearch).ToList().ForEach(t => Console.WriteLine($"{t.WordSearch} found at ({t.StartX}, {t.StartY}) to ({t.EndX}, {t.EndY}) "));

            Console.WriteLine("------------------------------");
            Console.WriteLine("");
            Console.WriteLine("Press any key to end");
            Console.ReadKey();
        }

        private static List<SearchResult> FindWords()
        {
            //Find each of the words in the grid, outputting the start and end location of
            //each word, e.g.
            //PUPPY found at (10,7) to (10, 3) 
            var foundWords = new List<SearchResult>();

            //todo: move these numbers into constants
            for (int x = 0; x < 12; x++)
            {
                for (int y = 0;y < 12; y++)
                {
                    for (int z = 0;z < Words.Length; z++)
                    {
                        var searchResult = FindWordFromLocation(Words[z], x, y);
                        if (searchResult.IsFound)
                        {
                            foundWords.Add(searchResult);
                        }
                    }
                }
            }

            return foundWords;
        }

        private static SearchResult FindWordFromLocation(string word, int x, int y)
        {
            //takes one of the words from the list of words and tries to find it based on the starting x/y
            //todo: array out of bounds check
            var currentLetter = Grid[y, x].ToString();
            
            //if the current letter is the same as the first letter in a word, do a traverse in all directions to try and match the word
            if (string.Equals(currentLetter, word[0].ToString()))
            {
                //it matched, traverse
                return TraverseGrid(word, x, y);
            }

            return new SearchResult()
            {
                IsFound = false,
                StartX = x,
                StartY = y,
            };
        }

        
        private static SearchResult TraverseGrid(string word, int x, int y)
        {

            var searchParams = new SearchResult()
            {
                WordSearch = word,
	            IsFound = false,
	            StartX = x,
	            StartY = y,
            };

            //we know the first letter of the word should match the passed in location
            foreach (Direction direction in (Direction[]) Enum.GetValues(typeof(Direction)))
            {
  
                var searchResult = TraverseGrid(word, 0, direction, x, y, searchParams);
                if (searchResult.IsFound)
                {
                    return searchResult;
                }
            }

            return searchParams;
        }
		
        private static SearchResult TraverseGrid(string word, int wordIndex, Direction direction, int x, int y, SearchResult searchResult)
        {
            bool matches = false;
            var nextLocation = GetNextLocation(direction, x, y);
            var nextIndex = wordIndex + 1; 
            if (nextLocation.IsValidLocation)
            {
				matches = string.Equals(Grid[nextLocation.Y, nextLocation.X].ToString(), word[nextIndex].ToString());  //do i need to handle case sensitivity?
                if (matches)
                {
                    if ((nextIndex + 1) == word.Length)
					{
						searchResult.IsFound = true;
						searchResult.EndX = nextLocation.X;
						searchResult.EndY = nextLocation.Y;
					} 
                    else if ((nextIndex) < word.Length)
                    {
                        searchResult = TraverseGrid(word, nextIndex, direction, nextLocation.X, nextLocation.Y, searchResult);
                    } 
                }
            }
            return searchResult;
        }

        private static Location GetNextLocation(Direction direction, int x, int y)
        {
            Location result = new Location()
            {
                X = x,
                Y = y,
            };
            switch (direction)
            {
                case Direction.N:
                    {
                        result.Y = y - 1;
                        break;
                    }
                case Direction.NE:
	                {
		                result.Y = y - 1;
                        result.X = x + 1;
		                break;
	                }
                case Direction.E:
	                {
		                result.X = x + 1;
		                break;
	                }
                case Direction.SE:
	                {
                        result.Y = y + 1;
		                result.X = x + 1;
		                break;
	                }
                case Direction.S:
                    {
		                result.Y = y + 1;
		                break;
                    }
                case Direction.SW:
                    {
		                result.Y = y + 1;
		                result.X = x - 1;
		                break;
                    }
                case Direction.W:
	                {
		                result.X = x - 1;
		                break;
	                }
                case Direction.NW:
	                {
		                result.Y = y - 1;
                        result.X = x - 1;
		                break;
	                }
			}
            result.IsValidLocation = result.X >= 0 && result.X < 12 && result.Y >= 0 && result.Y < 12;
            return result;
        }
    }


}
