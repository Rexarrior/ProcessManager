using System;
using System.Collections.Generic;



namespace CMValue
{
    
    
    public enum TypeValue { CPU, Memory };
    public enum LetterValue { B, Kb, Mb, Gb, Gc, KGc, MGc, GGc, Percent, PercentDevideTen, PercentDevideTenTwice };
    public class C_M_Value
    {
        //private members--------------------------------------------------------------------------------------------------
        private Int64 _size;
        private uint _count;
        private LetterValue _letter;
        private TypeValue _type;


        private void _letterCheck(LetterValue letter)
        {

            bool isCPUError = _type == TypeValue.CPU && letter < LetterValue.Gc;
            bool isMemoryError = _type == TypeValue.Memory && letter > LetterValue.Gc && letter < LetterValue.B;


            if (isMemoryError || isCPUError)
                throw new ArgumentException("Указанная размерность не соответствует типу: " + 
                    this.Type + ' ' + letter , "letter,type");

        }



        private bool _checkCountForPercent(uint count, LetterValue letter)
        {
            switch (letter)
            {
                case LetterValue.Percent:
                    return count <= 100;

                case LetterValue.PercentDevideTen:
                    return count <= 1000;

                case LetterValue.PercentDevideTenTwice:
                    return count <= 10000;
                default:
                    throw new ArgumentException("Суффикс(letter) задан неверно - не задает процент", "letter");
            } 
        }



        //public members---------------------------------------------------------------------------------------------------

        public static int InitialStringElemCount = 3; 
        //properties

        public Int64 Size
        {
            get
            {
                return _size;
            }
        }



        public uint Count
        {

            get
            {
                return _count;
            }
        }



        public TypeValue Type
        {
            get
            {
                return _type;
            }
        }



        public LetterValue Letter
        {
            get
            {
                return _letter;
            }
        }

        public C_M_Value ReInit(UInt64 size)
        {
            const UInt64 TERRABYTE = 1099511627776;
            if (size > TERRABYTE)
                throw new ArgumentException("Значения больше 1023G не поддерживаются.", "size");

            UInt64 value = size;
            LetterValue letter = _type == TypeValue.CPU ? LetterValue.Gc : LetterValue.B;

            while (value > 1024)
            {
                value /= 1024;
                letter++;
            }

            _letterCheck(letter);
            _letter = letter;
            _count = (uint)value;
            return this;

        }



        public C_M_Value ReInit(uint count, LetterValue letter)
        {
            _letterCheck(letter);
            if (letter >= LetterValue.Percent)
                if (_checkCountForPercent(count, letter))
                {
                    _count = count;
                    _letter = letter;
                    _size = -1;
                    return this;
                }
                else
                    throw new ArgumentException("Значение в процентах вышло за пределы 100%. Проверьте пару count-letter", "count");



            _letter = letter;
            _count = count;
            uint size = count;



            uint mn = (uint)(_type == TypeValue.CPU ? 1000 : 1024);
            LetterValue firstLetter = (_type == TypeValue.CPU) ? LetterValue.Gc : LetterValue.B;
            while (letter > firstLetter)
            {
               size *= mn;
               letter--;
            }
            





            _size = size;
            return this;
        }



        public C_M_Value(TypeValue type, UInt64 size = 0)
        {
            _type = type;
            ReInit(size);

        }


        public C_M_Value(TypeValue type, uint count, LetterValue letter)
        {
            _type = type;
            ReInit(count, letter);
        }

        public string GetInitialString()
        {
            return "" + (int)_type + ' ' + _count + ' ' + (int)_letter;
        }

        public C_M_Value(string initialString)
        {
            string[] initArrStr = initialString.Trim(' ').Split(' ');

           //System.IO.File.WriteAllText("log1.txt", System.IO.File.ReadAllText("log1.txt") + "Initial str: " + initialString);

            _type = int.Parse(initArrStr[0]) ==0 ? TypeValue.CPU : TypeValue.Memory ;
            uint count = uint.Parse(initArrStr[1]) ;
           
            LetterValue letter = (LetterValue)int.Parse(initArrStr[2]);
            ReInit(count, letter);

           
        }





    }
}