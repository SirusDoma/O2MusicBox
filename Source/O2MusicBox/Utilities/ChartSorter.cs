using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CXO2;
using CXO2.Charting;

namespace O2MusicBox
{
    public enum ChartSortType
    {
        None     = -1,
        Title    = 0,
        Artist   = 1,
        Pattern  = 2
    }

    /// <summary>
    /// This class is an implementation of the 'IComparer' interface.
    /// </summary>
    public class ChartSorter : IComparer<Chart>
    {
        /// <summary>
        /// Gets or sets the number of the column to which to apply the sorting operation (Defaults to '0').
        /// </summary>
        public ChartSortType SortType
        {
            get; set;
        }

        /// <summary>
        /// Gets or sets the order of sorting to apply (for example, 'Ascending' or 'Descending').
        /// </summary>
        public SortOrder Order
        {
            get; set;
        }

        /// <summary>
        /// Case insensitive comparer object
        /// </summary>
        private NaturalComparer comparer;

        /// <summary>
        /// Class constructor.  Initializes various elements
        /// </summary>
        public ChartSorter()
        {
            // Initialize the column to '0'
            SortType = ChartSortType.None;

            // Initialize the sort order to 'none'
            Order = SortOrder.None;

            // Initialize the NaturalComparer object
            comparer = new NaturalComparer();
        }

        /// <summary>
        /// This method is inherited from the IComparer interface.  It compares the two objects passed using a case insensitive comparison.
        /// </summary>
        /// <param name="x">First object to be compared</param>
        /// <param name="y">Second object to be compared</param>
        /// <returns>The result of the comparison. "0" if equal, negative if 'x' is less than 'y' and positive if 'x' is greater than 'y'</returns>
        public int Compare(Chart x, Chart y)
        {
            // Cast the objects to be compared to ListViewItem objects
            string itemX = string.Empty, itemY = string.Empty;
            switch (SortType)
            {
                case ChartSortType.Title:
                    itemX = x.Title;
                    itemY = y.Title;
                    break;
                case ChartSortType.Artist:
                    itemX = x.Title;
                    itemY = y.Title;
                    break;
                case ChartSortType.Pattern:
                    itemX = x.Pattern;
                    itemY = y.Pattern;
                    break;
                default: break;
            }

            // Compare the two items
            int compareResult = comparer.Compare(itemX, itemY);

            // Calculate correct return value based on object comparison
            if (Order == SortOrder.Ascending)
            {
                // Ascending sort is selected, return normal result of compare operation
                return compareResult;
            }
            else if (Order == SortOrder.Descending)
            {
                // Descending sort is selected, return negative result of compare operation
                return (-compareResult);
            }
            else
            {
                // Return '0' to indicate they are equal
                return 0;
            }
        }
    }
}
