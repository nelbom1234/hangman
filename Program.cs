using System.Reflection.Metadata;

namespace hangman;

class Program
{
    static void Main(string[] args)
    {
        // Initialize game variables
        string word = SetupGame();
        int len = word.Length;
        int lives = 6;  // Number of lives
        string guessedLetters = "";
        string hints = "".PadRight(len, '_');  // Placeholder for guessed letters
        bool hasWon = false;

        // Main game loop
        while (DrawScreen(lives, guessedLetters, hints, hasWon)) {
            if (LetterOrWord()) {  // Check if the player wants to guess the whole word
                string guess = ReceiveWord();
                if (guess == word) {
                    hasWon = true;
                    hints = word;
                    continue;
                }
                else {
                    lives--;  // Reduce lives if the guess is incorrect
                    continue;
                }
            }
            else {  // Guessing a single letter
                char guess = ReceiveLetter();
                if (guessedLetters.Contains(guess)) {
                    Console.WriteLine("Du har allerede gættet det bogstav");
                    Console.WriteLine("Du har ikke mistet et lives. tryk enter for at starte runden forfra");
                    Console.ReadKey(true);
                    continue;
                }
                guessedLetters += guess;
                if (word.Contains(guess)) {  // Check if the guessed letter is in the word
                    for (int i = 0; i < len; i++) {
                        if (word[i] == guess) {
                            hints = hints.Substring(0, i) + guess + hints.Substring(i + 1);
                        }
                    }
                    if (hints == word) {
                        hasWon = true;
                    }
                }
                else {
                    lives--;  // Incorrect letter guessed
                    if (lives <= 0) {
                        hints = word;
                    }
                }
            }
        }
    }

    const string alphabet = "abcdefghijklmnopqrstuvwxyzæøå";

    // Sets up the game by prompting the user to choose a word
    static string SetupGame() {
        Console.WriteLine("Velkommen til mit hangman spil.");
        return ReceiveWord();
    }

    // Receives a letter input from the user and validates it
    static char ReceiveLetter() {
        while (true) {
            Console.WriteLine("Vælg et bogstav");
            string? result = Console.ReadLine();
            if (result == null) {
                Console.WriteLine("Der var et problem med dit input. tryk enter for at prøve igen");
                Console.ReadKey(true);
                continue;
            }
            string letter = result.ToLower();
            if (letter.Length != 1) {
                Console.WriteLine("Du må kun skrive et bogstav. tryk enter for at prøve igen");
                Console.ReadKey(true);
                continue;
            }
            if (!vaildateWord(letter)) {
                Console.WriteLine("du må kun bruge bogstaver i det danske alfabet. tryk enter for at prøve igen");
                Console.ReadKey(true);
                continue;
            }
            return letter[0];
        }
    }

    // Receives a word input from the user and validates it
    static string ReceiveWord() {
        while (true) {
            Console.WriteLine("Vælg et ord");
            Console.WriteLine("Det skal være mellem 4 og 9 bogstaver");
            string? result = Console.ReadLine();
            if (result == null) {
                Console.WriteLine("Der var et problem med dit input. tryk enter for at prøve igen");
                Console.ReadKey(true);
                continue;
            }
            string word = result.ToLower();
            int len = word.Length;
            if (len < 4 || len > 9) {
                Console.WriteLine("Dit ord er ikke den korrekte længde. tryk enter for at prøve igen.");
                Console.ReadKey(true);
                continue;
            }
            if (!vaildateWord(word)) {
                Console.WriteLine("Dit ord må kun indeholde bogstaver i det danske alfabet. tryk enter for at prøve igen");
                Console.ReadKey(true);
                continue;
            }
            return word;
        }
    }

    // Validates if the input contains only Danish alphabet characters
    static bool vaildateWord(string word) {
        foreach (char c in word) {
            if (!alphabet.Contains(c)) {
                return false;
            }
        }
        return true;
    }

    // Determines whether the user wants to guess a letter or a word
    static bool LetterOrWord() {
        while (true) {
            Console.WriteLine("Vil du gætte ordet eller et bogstav. skriv tallet");
            Console.WriteLine("1. bogstav");
            Console.WriteLine("2. ord");

            string? result = Console.ReadLine();
            if (result == null) {
                Console.WriteLine("noget gik galt med inputtet. tryk enter for at prøve igen");
                Console.ReadKey(true);
                continue;
            }
            if (!int.TryParse(result, out int valg)) {
                Console.WriteLine("du skal taste 1 eller 2");
                Console.ReadKey(true);
                continue;
            }
            return valg == 2;
        }
    }

    // Displays the current game state
    static bool DrawScreen(int lives, string guessedLetters, string hints, bool hasWon) {
        Console.Clear();
        string[] animationer = // Hangman visuals
            [
                "  +---+\n  |   |\n  O   |\n /|\\  |\n / \\  |\n      |\n=========",
                "  +---+\n  |   |\n  O   |\n /|\\  |\n /    |\n      |\n=========",
                "  +---+\n  |   |\n  O   |\n /|\\  |\n      |\n      |\n=========",
                "  +---+\n  |   |\n  O   |\n /|   |\n      |\n      |\n=========",
                "  +---+\n  |   |\n  O   |\n  |   |\n      |\n      |\n=========",
                "  +---+\n  |   |\n  O   |\n      |\n      |\n      |\n=========",
                "  +---+\n  |   |\n      |\n      |\n      |\n      |\n=========",
            ]; 

        Console.WriteLine(animationer[lives]);
        Console.WriteLine("");
        Console.WriteLine("allerede gættede bogstaver: " + guessedLetters);
        Console.WriteLine("");
        Console.WriteLine(hints);
        Console.WriteLine("");
        if (hasWon) {
            Win();
            return false;
        }
        else if (lives <= 0) {
            Lose();
            return false;
        }
        return true;
    }

    // Displays winning message
    static void Win() {
        Console.WriteLine("DET ER RIGTIGT!!! DU HAR VUNDET");
    }

    // Displays losing message
    static void Lose() {
        Console.WriteLine("Du er desværre løbet tør for liv");
    }
}