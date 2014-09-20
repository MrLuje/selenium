// <copyright file="ByOrdered.cs" company="WebDriver Committers">
// Copyright 2007-2012 WebDriver committers
// Copyright 2007-2012 Google Inc.
// Portions copyright 2012 Software Freedom Conservancy
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//     http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;

namespace OpenQA.Selenium.Support.PageObjects
{
    /// <summary>
    /// Mechanism used to locate elements within a document using a series of other lookups.  This class
    /// will find all DOM elements that matches one of the locators in order
    /// </summary>
    /// <example>
    /// The following code will find the first elements that match by1 or by2.
    /// <code>
    /// driver.findElements(new ByOrdered(by1, by2))
    /// </code>
    /// </example>
    public class ByOrdered : By
    {
        private readonly By[] bys;

        /// <summary>
        /// Initializes a new instance of the <see cref="ByOrdered"/> class with one or more <see cref="By"/> objects.
        /// </summary>
        /// <param name="bys">One or more <see cref="By"/> references</param>
        public ByOrdered(params By[] bys)
        {
            this.bys = bys;
        }

        /// <summary>
        /// Find a single element.
        /// </summary>
        /// <param name="context">Context used to find the element.</param>
        /// <returns>The element that matches</returns>
        public override IWebElement FindElement(ISearchContext context)
        {
            ReadOnlyCollection<IWebElement> elements = this.FindElements(context);
            if (elements.Count == 0)
            {
                throw new NoSuchElementException("Cannot locate an element using " + this.ToString());
            }

            return elements[0];
        }

        /// <summary>
        /// Finds many elements
        /// </summary>
        /// <param name="context">Context used to find the element.</param>
        /// <returns>A readonly collection of elements that match.</returns>
        public override ReadOnlyCollection<IWebElement> FindElements(ISearchContext context)
        {
            if (this.bys.Length == 0)
            {
                return new List<IWebElement>().AsReadOnly();
            }

            var elems = new List<IWebElement>();
            foreach (By by in this.bys)
            {
                elems.AddRange(by.FindElements(context));

                if (elems.Any())
                    break;
            }

            return elems.AsReadOnly();
        }

        /// <summary>
        /// Writes out a comma separated list of the <see cref="By"/> objects used in the ordered.
        /// </summary>
        /// <returns>Converts the value of this instance to a <see cref="System.String"/></returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (By by in this.bys)
            {
                if (stringBuilder.Length > 0)
                {
                    stringBuilder.Append(",");
                }

                stringBuilder.Append(by);
            }

            return string.Format(CultureInfo.InvariantCulture, "By.Ordered([{0}])", stringBuilder.ToString());
        }
    }
}
