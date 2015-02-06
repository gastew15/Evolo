using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Evolo
{
    public static class GlobalVar
    {
        static String _gameState;
        static String _previousGameState;
        static string[] _optionsArray;
        static Boolean _exitGame;
        static Boolean _customLevel;
        static Boolean _resetGameField;
        static Vector2 _screenSize;
        static Vector2 _scaleSize;
        static String _currentLevel = "1";
        static int _score;
        static int[] _highScore;
        static int _highestLevel;
        static int _playerProfile;

        public static String GameState
        {
            get
            {
                return _gameState;
            }
            set
            {
                _gameState = value;
            }
        }

        public static Boolean ResetGameField
        {
            get
            {
                return _resetGameField;
            }
            set
            {
                _resetGameField = value;
            }
        }

        public static Boolean CustomLevel
        {
            get
            {
                return _customLevel;
            }
            set
            {
                _customLevel = value;
            }
        }

        public static String CurrentLevel
        {
            get
            {
                return _currentLevel;
            }
            set
            {
                _currentLevel = value;
            }
        }

        public static int[] HighScore
        {
            get
            {
                return _highScore;
            }
            set
            {
                _highScore = value;
            }
        }

        public static int HighestLevel
        {
            get
            {
                return _highestLevel;
            }
            set
            {
                _highestLevel = value;
            }
        }

        public static string[] OptionsArray
        {
            get
            {
                return _optionsArray;
            }
            set
            {
                _optionsArray = value;
             }
        }

        public static int PlayerProfile
        {
            get
            {
                return _playerProfile;
            }
            set
            {
                _playerProfile = value;
            }
        }

        public static String PreviousGameState
        {
            get
            {
                return _previousGameState;
            }
            set
            {
                _previousGameState = value;
            }
        }

        public static Boolean ExitGame
        {
            get
            {
                return _exitGame;
            }
            set
            {
                _exitGame = value;
            }
        }

        public static Vector2 ScreenSize
        {
            get
            {
                return _screenSize;
            }
            set
            {
                _screenSize = value;
            }
        }

        public static Vector2 ScaleSize
        {
            get
            {
                return _scaleSize;
            }
            set
            {
                _scaleSize = value;
            }
        }

        public static int Score
        {
            get
            {
                return _score;
            }
            set
            {
                _score = value;
            }
        }
    }
}
