
// This class credit goes to Doaz
// This magic list class!! xD

// Thank you Doaz!

using System;
using System.Collections.Generic;
using System.Text;

namespace CXO2.Data
{
    public class XList<T> : List<T>
    {
        private int p = 0;
        private int litr = 0;
        private int pprev = -2;
        private int pnext = -2;
        public int Pointer { get { return p; } set { p = value; } }

        public XList()
            : base()
        {
            Pointer = 0;
            pprev = -2;
            pnext = -2;
        }

        public IEnumerable<T> AllElements
        {
            get
            {
                for (int i = 0; i < Count; i++)
                    yield return this[i];
            }
        }

        public IEnumerable<T> AllElementsAtOrAfterPointer
        {
            get
            {
                for (int i = Pointer; i < Count; i++)
                    yield return this[i];
            }
        }

        public XList<T> ListIterator()
        {
            XList<T> result = new XList<T>();
            result.litr = 0;
            result.AddRange(AllElements);

            return result;
        }

        public XList<T> ListIterator(int index)
        {
            XList<T> result = new XList<T>();
            result.litr = index;
            result.AddRange(AllElements);

            return result;
        }

        public bool HasPrevious
        {
            get
            {
                if (pprev == -2)
                    pprev = litr;

                return (pprev > 0);
            }
        }

        public T Previous
        {
            get
            {
                if (pprev == -2)
                    pprev = litr;

                if (HasPrevious)
                    pprev--;

                T result = this[pprev];
                return result;
            }
        }

        public bool HasNext
        {
            get
            {
                if (pnext == -2)
                    pnext = litr;

                return (pnext < Count);
            }
        }

        public T Next
        {
            get
            {
                if (pnext == -2)
                    pnext = litr;

                T result = this[pnext];

                if (HasNext)
                    pnext++;

                return result;
            }
        }

        public bool HasElementLeft { get { return Pointer < Count; } }

        public bool HasPreviousElement { get { return Pointer > 0; } }

        public int NextIndex { get { if (pnext < (Count - 1)) return pnext; else return Count; } }

        public int PreviousIndex { get { if (pprev == 0) return -1; else return pprev; } }

        public void ResetNextIndex() { pnext = litr; }

        public void ResetPreviousIndex() { pprev = litr; }

        public void ResetPointer() { Pointer = 0; }

        public T ElementAtPointer { get { return (Pointer < (Count - 1)) ? this[Pointer] : this[Count - 1]; } }

        public void IncreasePointer() { Pointer++; }

        public void DecreasePointer() { Pointer--; }
    }
}
