/**
 * \file Pair.cs
*  \project INFO2180 - EMS System Term Project
*  \author The Char Stars - Tudor Lupu
*  \date 2018-12-4
*  \brief A tuple but with the possibility of changing the values
*  
*  This class works exactly like a tuple except it is possible to
*  change the values stored inside
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMS_Client
{
    /** 
    * \class Pair
    *
    * \brief <b>Brief Description</b> - This class is a simple tuple like collection
    * 
    * The Pair class is just like a tuple except it allows for its fields to be edited.
    * 
    * \author <i>The Char Stars - Tudor Lupu</i>
    */
    public class Pair<A, B>
    {
        public A First { get; set; }
        public B Second { get; set; }

        /**
        * \brief <b>Brief Description</b> - Pair <b><i>class constructor</i></b> - This is just the default constructor
        * \details <b>Details</b>
        *
        * This function is simply just the default constructor used when creating a blank pair object
        * 
        * \return <b>void</b>
        */
        public Pair() { }

        /**
        * \brief <b>Brief Description</b> - Pair <b><i>class constructor</i></b> - This takes two values and sets them
        * \details <b>Details</b>
        *
        * This function takes two values and sets them in order to create the pair that can be accessed.
        * 
        * \return <b>void</b>
        */
        public Pair(A first, B second)
        {
            First = first;
            Second = second;
        }
    }
}
