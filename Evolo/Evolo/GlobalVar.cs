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
        static Vector2 _screenSize;
        static Vector2 _scaleSize;
        static int _score;
        static int _currency;
        static int _health;
        static int _sheild;
        static int _heat;

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

        public static int Currency
        {
            get
            {
                return _currency;
            }
            set
            {
                _currency = value;
            }
        }

        public static int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
            }
        }

        public static int Sheild
        {
            get
            {
                return _sheild;
            }
            set
            {
                _sheild = value;
            }
        }
        public static int Heat
        {
            get
            {
                return _heat;
            }
            set
            {
                _heat = value;
            }
        }
    }
}
