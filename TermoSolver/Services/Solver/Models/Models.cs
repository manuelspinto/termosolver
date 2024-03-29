﻿namespace TermoSolver.Services.Solver.Models
{
    public class WordFilter
    {
        public List<GroupChar> ProhibitedChars = new List<GroupChar>();
        public char[] AllowedChars = new char[5];
        public List<PositionalChar> MisplacedChars = new List<PositionalChar>();

        public WordFilter()
        {
            AllowedChars = new char[] { '-', '-', '-', '-', '-' };
        }

    }

    public class PositionalChar
    {
        public char Character;
        public List<int> Position = new List<int>();
        public int Frequency = 1;
    }

    public class GroupChar
    {
        public char Character;
        public int Frequency;

        public GroupChar(char character, int frequency)
        {
            Character = character;
            Frequency = frequency;
        }
    }

    public class WordState
    {
        public char Character;
        public CharacterState State;
    }

    public enum CharacterState
    {
        Unknown,
        WrongCharacter,
        RightPosition,
        WrongPosition,
    }
}
