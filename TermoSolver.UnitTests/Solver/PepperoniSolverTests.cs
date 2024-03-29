﻿using FluentAssertions;
using TermoSolver.Models;
using TermoSolver.Services.Solver;
using TermoSolver.Services.Solver.Models;
using TermoSolver.Services.Solver.SolverStrategies;

namespace TermoSolver.UnitTests.Solver
{
    public class PepperoniSolverTests
    {
        private WordSolver Sut;

        public PepperoniSolverTests()
        {
            Sut = new PepperoniSolver();
        }


        [Fact]
        public void GetBestWord_When_Multiple_ChooseWordWithHigherScore()
        {
            // arrange
            var words = new List<WordScore>() 
                { 
                    new WordScore ("MENTA", 3965),
                    new WordScore ("RUSSO", 3119),
                    new WordScore ("MINTA", 3769)
                };


            var prohibitedChars = new List<GroupChar>();
            var singleChars = new List<char>("");
            singleChars.ForEach(c => prohibitedChars.Add(new GroupChar(c, 1)));

            var filter = new WordFilter()
            {
                ProhibitedChars = prohibitedChars,
                AllowedChars = "-----".ToCharArray(),
                MisplacedChars = new List<PositionalChar>()
            };

            // act
            var result = Sut.GetBestWord(words, filter);

            // assert
            result.Should().NotBeNull();
            result.Should().Be("MENTA");
        }

        [Fact]
        public void GetBestWord_When_ProhibitedChar_FilterProhibitedWord()
        {
            // arrange
            var words = new List<WordScore>()
                {
                    new WordScore ("MENTA", 3965),
                    new WordScore ("RUSSO", 3119),
                    new WordScore ("MINTA", 3769)
                };


            var prohibitedChars = new List<GroupChar>();
            var singleChars = new List<char>("E");
            singleChars.ForEach(c => prohibitedChars.Add(new GroupChar(c, 1)));

            var filter = new WordFilter()
            {
                ProhibitedChars = prohibitedChars,
                AllowedChars = "-----".ToCharArray(),
                MisplacedChars = new List<PositionalChar>()
            };

            // act
            var result = Sut.GetBestWord(words, filter);

            // assert
            result.Should().NotBeNull();
            result.Should().Be("MINTA");
        }

        [Fact]
        public void GetBestWord_When_AllowedChars_FilterAllowedWords()
        {
            // arrange
            var words = new List<WordScore>()
                {
                    new WordScore ("MENTA", 3965),
                    new WordScore ("RUSSO", 3119),
                    new WordScore ("MINTA", 3769)
                };


            var prohibitedChars = new List<GroupChar>();
            var singleChars = new List<char>("");
            singleChars.ForEach(c => prohibitedChars.Add(new GroupChar(c, 1)));

            var filter = new WordFilter()
            {
                ProhibitedChars = prohibitedChars,
                AllowedChars = "MI---".ToCharArray(),
                MisplacedChars = new List<PositionalChar>()
            };

            // act
            var result = Sut.GetBestWord(words, filter);

            // assert
            result.Should().NotBeNull();
            result.Should().Be("MINTA");
        }

        [Fact]
        public void GetBestWord_When_NoWordsFound_ShouldReturnNull()
        {
            // arrange
            var words = new List<WordScore>()
                {
                    new WordScore ("MENTA", 3965),
                    new WordScore ("RUSSO", 3119),
                    new WordScore ("MINTA", 3769)
                };


            var prohibitedChars = new List<GroupChar>();
            var singleChars = new List<char>("");
            singleChars.ForEach(c => prohibitedChars.Add(new GroupChar(c, 1)));

            var filter = new WordFilter()
            {
                ProhibitedChars = prohibitedChars,
                AllowedChars = "MO---".ToCharArray(),
                MisplacedChars = new List<PositionalChar>()
            };

            // act
            var result = Sut.GetBestWord(words, filter);

            // assert
            result.Should().BeNull();
        }

        [Fact]
        public void GetBestWord_When_MisplacedChar_ShouldReturnCorrectWord()
        {
            // arrange
            var words = new List<WordScore>()
                {
                    new WordScore ("MENTA", 3965),
                    new WordScore ("RUIVA", 99999),
                    new WordScore ("MINTA", 3769)
                };


            var prohibitedChars = new List<GroupChar>();
            var singleChars = new List<char>("");
            singleChars.ForEach(c => prohibitedChars.Add(new GroupChar(c, 1)));

            var filter = new WordFilter()
            {
                ProhibitedChars = prohibitedChars,
                AllowedChars = "-----".ToCharArray(),
                MisplacedChars = new List<PositionalChar>() {
                    new PositionalChar()
                    {
                        Character = 'I',
                        Position = new List<int>()
                        {
                            2
                        }
                    },
                }
            };

            // act
            var result = Sut.GetBestWord(words, filter);

            // assert
            result.Should().NotBeNull();
            result.Should().Be("MINTA");
        }

        [Fact]
        public void GetBestWord_When_MisplacedChar_MultipleResults_ShouldReturnCorrectWord()
        {
            // arrange
            var words = new List<WordScore>()
                {
                    new WordScore ("MENTA", 3965),
                    new WordScore ("RUIVA", 99999),
                    new WordScore ("MINTA", 3769)
                };


            var prohibitedChars = new List<GroupChar>();
            var singleChars = new List<char>("");
            singleChars.ForEach(c => prohibitedChars.Add(new GroupChar(c, 1)));

            var filter = new WordFilter()
            {
                ProhibitedChars = prohibitedChars,
                AllowedChars = "-----".ToCharArray(),
                MisplacedChars = new List<PositionalChar>() {
                    new PositionalChar()
                    {
                        Character = 'I',
                        Position = new List<int>()
                        {
                            3
                        }
                    },
                }
            };

            // act
            var result = Sut.GetBestWord(words, filter);

            // assert
            result.Should().NotBeNull();
            result.Should().Be("RUIVA");
        }

        [Fact]
        public void GetBestWord_When_MultipleMisplacedChar_ShouldReturnCorrectWord()
        {
            // arrange
            var words = new List<WordScore>()
                {
                    new WordScore ("MENTA", 3965),
                    new WordScore ("RUIVA", 99999),
                    new WordScore ("MINTA", 3769)
                };


            var prohibitedChars = new List<GroupChar>();
            var singleChars = new List<char>("");
            singleChars.ForEach(c => prohibitedChars.Add(new GroupChar(c, 1)));

            var filter = new WordFilter()
            {
                ProhibitedChars = prohibitedChars,
                AllowedChars = "-----".ToCharArray(),
                MisplacedChars = new List<PositionalChar>() {
                    new PositionalChar()
                    {
                        Character = 'I',
                        Position = new List<int>()
                        {
                            2, 3, 4
                        }
                    },
                    new PositionalChar()
                    {
                        Character = 'A',
                        Position = new List<int>()
                        {
                            3
                        }
                    },
                }
            };

            // act
            var result = Sut.GetBestWord(words, filter);

            // assert
            result.Should().NotBeNull();
            result.Should().Be("MINTA");
        }

        [Fact]
        public void GetBestWord_When_DuplicatedChar_AllowedAndPositional_ShouldReturnCorrectWord()
        {
            // arrange
            var words = new List<WordScore>()
                {
                    new WordScore ("CABRA", 11111),
                    new WordScore ("RUIVA", 99999),
                    new WordScore ("FALSA", 55555)
                };


            var prohibitedChars = new List<GroupChar>();
            var singleChars = new List<char>("");
            singleChars.ForEach(c => prohibitedChars.Add(new GroupChar(c, 1)));

            var filter = new WordFilter()
            {
                ProhibitedChars = prohibitedChars,
                AllowedChars = "----A".ToCharArray(),
                MisplacedChars = new List<PositionalChar>() {
                    new PositionalChar()
                    {
                        Character = 'A',
                        Position = new List<int>()
                        {
                            3
                        }
                    },
                }
            };

            // act
            var result = Sut.GetBestWord(words, filter);

            // assert
            result.Should().NotBeNull();
            result.Should().Be("FALSA");
        }

        [Fact]
        public void GetBestWord_When_DuplicatedChar_OnlyFixed_ShouldReturnCorrectWord()
        {
            // arrange
            var words = new List<WordScore>()
                {
                    new WordScore ("CABRA", 11111),
                    new WordScore ("RUIVA", 99999),
                    new WordScore ("FALSA", 55555)
                };


            var prohibitedChars = new List<GroupChar>();
            var singleChars = new List<char>("A");
            singleChars.ForEach(c => prohibitedChars.Add(new GroupChar(c, 1)));
            prohibitedChars.Where(c => c.Character == 'A').First().Frequency = 2;

            var filter = new WordFilter()
            {
                ProhibitedChars = prohibitedChars,
                AllowedChars = "-----".ToCharArray(),
                MisplacedChars = new List<PositionalChar>() {
                    new PositionalChar()
                    {
                        Character = 'A',
                        Position = new List<int>()
                        {
                            3
                        }
                    },
                }
            };

            // act
            var result = Sut.GetBestWord(words, filter);

            // assert
            result.Should().NotBeNull();
            result.Should().Be("RUIVA");
        }

        [Fact]
        public void GetBestWord_When_OnlyMisplacedChar_ShouldReturnCorrectWord()
        {
            // arrange
            var words = new List<WordScore>()
                {
                    new WordScore ("UREIA", 99999),
                };


            var prohibitedChars = new List<GroupChar>();
            var singleChars = new List<char>("A");
            singleChars.ForEach(c => prohibitedChars.Add(new GroupChar(c, 1)));
            prohibitedChars.Where(c => c.Character == 'A').First().Frequency = 2;

            var filter = new WordFilter()
            {
                ProhibitedChars = prohibitedChars,
                AllowedChars = "-----".ToCharArray(),
                MisplacedChars = new List<PositionalChar>() {
                    new PositionalChar()
                    {
                        Character = 'A',
                        Position = new List<int>()
                        {
                            1
                        }
                    },
                }
            };

            // act
            var result = Sut.GetBestWord(words, filter);

            // assert
            result.Should().NotBeNull();
            result.Should().Be("UREIA");
        }

        [Fact]
        public void IterateFilter_When_ProhibitedAndRightAndMisplacedChars_AddToFilter()
        {
            // arrange
            WordState[] wordState = new WordState[5]
            {   new WordState(){ Character = 'B', State = CharacterState.WrongCharacter },
                new WordState(){ Character = 'O', State = CharacterState.WrongCharacter },
                new WordState(){ Character = 'L', State = CharacterState.WrongPosition },
                new WordState(){ Character = 'A', State = CharacterState.RightPosition },
                new WordState(){ Character = 'S', State = CharacterState.RightPosition },
            };


            var prohibitedChars = new List<GroupChar>();
            var singleChars = new List<char>("");
            singleChars.ForEach(c => prohibitedChars.Add(new GroupChar(c, 1)));

            WordFilter filter = new()
            {
                ProhibitedChars = prohibitedChars,
                AllowedChars = "-----".ToCharArray(),
                MisplacedChars = new List<PositionalChar>()
            };

            // act
            Sut.IterateFilter(filter, wordState);

            // assert
            var expectedProhibitedChar = new List<GroupChar>();
            var expectedSingleChar = new List<char>("BO");
            expectedSingleChar.ForEach(c => expectedProhibitedChar.Add(new GroupChar(c, 1)));

            var expectedFilter = new WordFilter() { 
                AllowedChars = "---AS".ToCharArray(),
                ProhibitedChars = expectedProhibitedChar,
                MisplacedChars= new List<PositionalChar>()
                {
                    new PositionalChar()
                    {
                        Character = 'L',
                        Position = new List<int>()
                        {
                            2
                        }
                    },
                }
            };

            filter.Should().NotBeNull();
            filter.Should().BeEquivalentTo(expectedFilter);
        }

        [Fact]
        public void IterateFilter_When_ExistingProhibitedChar_DoNotAddToFilter()
        {
            // arrange
            WordState[] wordState = new WordState[5]
            {   new WordState(){ Character = 'B', State = CharacterState.WrongCharacter },
                new WordState(){ Character = 'O', State = CharacterState.WrongCharacter },
                new WordState(){ Character = 'L', State = CharacterState.RightPosition },
                new WordState(){ Character = 'A', State = CharacterState.RightPosition },
                new WordState(){ Character = 'S', State = CharacterState.RightPosition },
            };


            var prohibitedChars = new List<GroupChar>();
            var singleChars = new List<char>("B");
            singleChars.ForEach(c => prohibitedChars.Add(new GroupChar(c, 1)));

            WordFilter filter = new()
            {
                ProhibitedChars = prohibitedChars,
                AllowedChars = "-----".ToCharArray(),
                MisplacedChars = new List<PositionalChar>()
            };

            // act
            Sut.IterateFilter(filter, wordState);

            // assert
            var expectedProhibitedChar = new List<GroupChar>();
            var expectedSingleChar = new List<char>("BO");
            expectedSingleChar.ForEach(c => expectedProhibitedChar.Add(new GroupChar(c, 1)));

            var expectedFilter = new WordFilter()
            {
                AllowedChars = "--LAS".ToCharArray(),
                ProhibitedChars = expectedProhibitedChar,
                MisplacedChars = new List<PositionalChar>()
            };

            filter.Should().NotBeNull();
            filter.Should().BeEquivalentTo(expectedFilter);
        }

        [Fact]
        public void IterateFilter_When_MisplacedCharIsFound_RemoveFromMisplacedCharFilter()
        {
            // arrange
            WordState[] wordState = new WordState[5]
            {   new WordState(){ Character = 'B', State = CharacterState.WrongCharacter },
                new WordState(){ Character = 'O', State = CharacterState.WrongCharacter },
                new WordState(){ Character = 'L', State = CharacterState.RightPosition },
                new WordState(){ Character = 'A', State = CharacterState.RightPosition },
                new WordState(){ Character = 'S', State = CharacterState.RightPosition },
            };


            var prohibitedChars = new List<GroupChar>();
            var singleChars = new List<char>("B");
            singleChars.ForEach(c => prohibitedChars.Add(new GroupChar(c, 1)));

            WordFilter filter = new()
            {
                ProhibitedChars = prohibitedChars,
                AllowedChars = "-----".ToCharArray(),
                MisplacedChars = new List<PositionalChar>()
                {
                    new PositionalChar()
                    {
                        Character = 'L',
                        Position = new List<int>()
                        {
                            1
                        },
                        Frequency = 1
                    },
                }
            };

            // act
            Sut.IterateFilter(filter, wordState);

            // assert
            var expectedProhibitedChar = new List<GroupChar>();
            var expectedSingleChar = new List<char>("BO");
            expectedSingleChar.ForEach(c => expectedProhibitedChar.Add(new GroupChar(c, 1)));

            var expectedFilter = new WordFilter()
            {
                AllowedChars = "--LAS".ToCharArray(),
                ProhibitedChars = expectedProhibitedChar,
                MisplacedChars = new List<PositionalChar>()
            };

            filter.Should().NotBeNull();
            filter.Should().BeEquivalentTo(expectedFilter);
        }

        [Fact]
        public void IterateFilter_When_MisplacedCharIsFound_ReduceFrequencyFromMisplacedCharFilter()
        {
            // arrange
            WordState[] wordState = new WordState[5]
            {   new WordState(){ Character = 'B', State = CharacterState.WrongCharacter },
                new WordState(){ Character = 'A', State = CharacterState.WrongPosition },
                new WordState(){ Character = 'L', State = CharacterState.RightPosition },
                new WordState(){ Character = 'A', State = CharacterState.RightPosition },
                new WordState(){ Character = 'S', State = CharacterState.RightPosition },
            };


            var prohibitedChars = new List<GroupChar>();
            var singleChars = new List<char>("");
            singleChars.ForEach(c => prohibitedChars.Add(new GroupChar(c, 1)));

            WordFilter filter = new()
            {
                ProhibitedChars = prohibitedChars,
                AllowedChars = "-----".ToCharArray(),
                MisplacedChars = new List<PositionalChar>()
                {
                    new PositionalChar()
                    {
                        Character = 'A',
                        Position = new List<int>()
                        {
                            0, 4
                        },
                        Frequency = 2
                    },
                }
            };

            // act
            Sut.IterateFilter(filter, wordState);

            // assert
            var expectedProhibitedChar = new List<GroupChar>();
            var expectedSingleChar = new List<char>("B");
            expectedSingleChar.ForEach(c => expectedProhibitedChar.Add(new GroupChar(c, 1)));

            var expectedFilter = new WordFilter()
            {
                AllowedChars = "--LAS".ToCharArray(),
                ProhibitedChars = expectedProhibitedChar,
                MisplacedChars = new List<PositionalChar>()
                { new PositionalChar()
                    {
                        Character = 'A',
                        Position = new List<int>()
                            {
                                0, 1, 4
                            },
                        Frequency = 1
                    },
                }
            };

            filter.Should().NotBeNull();
            filter.Should().BeEquivalentTo(expectedFilter);
        }

        [Fact]
        public void IterateFilter_When_DuplicatedChar_IncreaseFilterFrequency()
        {
            // arrange
            WordState[] wordState = new WordState[5]
            {   new WordState(){ Character = 'B', State = CharacterState.WrongCharacter },
                new WordState(){ Character = 'A', State = CharacterState.WrongCharacter },
                new WordState(){ Character = 'L', State = CharacterState.RightPosition },
                new WordState(){ Character = 'A', State = CharacterState.RightPosition },
                new WordState(){ Character = 'S', State = CharacterState.RightPosition },
            };


            var prohibitedChars = new List<GroupChar>();
            var singleChars = new List<char>("B");
            singleChars.ForEach(c => prohibitedChars.Add(new GroupChar(c, 1)));

            WordFilter filter = new()
            {
                ProhibitedChars = prohibitedChars,
                AllowedChars = "-----".ToCharArray(),
                MisplacedChars = new List<PositionalChar>()
            };

            // act
            Sut.IterateFilter(filter, wordState);

            // assert
            var expectedProhibitedChar = new List<GroupChar>();
            var expectedSingleChar = new List<char>("BA");
            expectedSingleChar.ForEach(c => expectedProhibitedChar.Add(new GroupChar(c, 1)));
            expectedProhibitedChar.Where(c => c.Character == 'A').First().Frequency = 2;

            var expectedFilter = new WordFilter()
            {
                AllowedChars = "--LAS".ToCharArray(),
                ProhibitedChars = expectedProhibitedChar,
                MisplacedChars = new List<PositionalChar>()
            };

            filter.Should().NotBeNull();
            filter.Should().BeEquivalentTo(expectedFilter);
        }

        [Fact]
        public void IterateFilter_When_ExistingMisplacedChar_AddPosition()
        {
            // arrange
            WordState[] wordState = new WordState[5]
            {   new WordState(){ Character = 'B', State = CharacterState.WrongCharacter },
                new WordState(){ Character = 'A', State = CharacterState.WrongCharacter },
                new WordState(){ Character = 'L', State = CharacterState.WrongPosition },
                new WordState(){ Character = 'A', State = CharacterState.RightPosition },
                new WordState(){ Character = 'S', State = CharacterState.RightPosition },
            };


            var prohibitedChars = new List<GroupChar>();
            var singleChars = new List<char>("B");
            singleChars.ForEach(c => prohibitedChars.Add(new GroupChar(c, 1)));

            WordFilter filter = new()
            {
                ProhibitedChars = prohibitedChars,
                AllowedChars = "-----".ToCharArray(),
                MisplacedChars = new List<PositionalChar>()
                {
                    new PositionalChar()
                    {
                        Character = 'L',
                        Position = new List<int>()
                        {
                            0
                        }
                    },
                }
            };

            // act
            Sut.IterateFilter(filter, wordState);

            // assert
            var expectedProhibitedChar = new List<GroupChar>();
            var expectedSingleChar = new List<char>("BA");
            expectedSingleChar.ForEach(c => expectedProhibitedChar.Add(new GroupChar(c, 1)));
            expectedProhibitedChar.Where(c => c.Character == 'A').First().Frequency = 2;

            var expectedFilter = new WordFilter()
            {
                AllowedChars = "---AS".ToCharArray(),
                ProhibitedChars = expectedProhibitedChar,
                MisplacedChars = new List<PositionalChar>()
                {
                    new PositionalChar()
                    {
                        Character = 'L',
                        Position = new List<int>()
                        {
                            0, 2
                        }
                    },
                }
            };

            filter.Should().NotBeNull();
            filter.Should().BeEquivalentTo(expectedFilter);
        }

        [Fact]
        public void IterateFilter_When_MultipleMisplacedChar_AddPositionAndFrequency()
        {
            // arrange
            WordState[] wordState = new WordState[5]
            {   new WordState(){ Character = 'B', State = CharacterState.WrongCharacter },
                new WordState(){ Character = 'A', State = CharacterState.WrongPosition },
                new WordState(){ Character = 'L', State = CharacterState.WrongCharacter },
                new WordState(){ Character = 'A', State = CharacterState.WrongPosition },
                new WordState(){ Character = 'S', State = CharacterState.RightPosition },
            };


            var prohibitedChars = new List<GroupChar>();
            var singleChars = new List<char>("");
            singleChars.ForEach(c => prohibitedChars.Add(new GroupChar(c, 1)));

            WordFilter filter = new()
            {
                ProhibitedChars = prohibitedChars,
                AllowedChars = "-----".ToCharArray(),
                MisplacedChars = new List<PositionalChar>()
            };

            // act
            Sut.IterateFilter(filter, wordState);

            // assert
            var expectedProhibitedChar = new List<GroupChar>();
            var expectedSingleChar = new List<char>("BL");
            expectedSingleChar.ForEach(c => expectedProhibitedChar.Add(new GroupChar(c, 1)));

            var expectedFilter = new WordFilter()
            {
                AllowedChars = "----S".ToCharArray(),
                ProhibitedChars = expectedProhibitedChar,
                MisplacedChars = new List<PositionalChar>()
                {
                    new PositionalChar()
                    {
                        Character = 'A',
                        Position = new List<int>()
                        {
                            1, 3
                        },
                        Frequency = 2
                    },
                }
            };

            filter.Should().NotBeNull();
            filter.Should().BeEquivalentTo(expectedFilter);
        }
    }
}
