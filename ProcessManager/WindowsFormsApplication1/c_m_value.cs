using System;
using System.Collections.Generic;



namespace CMValue
{
    
    
    public enum TypeValue { CPU, Memory };
    public enum LetterValue { Percent, B, Kb, Mb, Gb, Gc, Kgc, Mgc, Ggc };
    public class C_M_Value
    {
        //private members--------------------------------------------------------------------------------------------------
        private Int64 _size;
        private uint _count;
        private LetterValue _letter;
        private TypeValue _type;


        private void letterCheck(LetterValue letter)
        {

            bool isCPUError = _type == TypeValue.CPU && letter < LetterValue.Gc && letter != LetterValue.Percent;
            bool isMemoryError = _type == TypeValue.Memory && letter > LetterValue.Gc && letter < LetterValue.B;


            if (isMemoryError || isCPUError)
                throw new ArgumentException("Указанная размерность не соответствует типу: " + 
                    this.Type + ' ' + letter , "letter,type");

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

            letterCheck(letter);
            _letter = letter;
            _count = (uint)value;
            return this;

        }



        public C_M_Value ReInit(uint count, LetterValue letter)
        {
            letterCheck(letter);
            if (letter == LetterValue.Percent)
                if (count <= 100)
                {
                    _count = count;
                    _letter = letter;
                    _size = -1;
                    return this;
                }
                else
                    throw new ArgumentException("Значение в процентах не должно превышать 100", "count");



            _letter = letter;
            _count = count;
            uint size = count;

            //TODO работает только для памяти
            while (letter > LetterValue.B)
            {
                size *= 1024;
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

            LetterValue letter = LetterValue.B;
            if (initArrStr[2] == "0")
                letter = LetterValue.Percent;
            else
                for (int i = 1; i < int.Parse(initArrStr[2]); i++)
                    letter++;

            ReInit(count, letter);

           
        }





    }
}