﻿using System.Linq;
using DdfGuide.Core;
using DdfGuide.Core.Sorting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DdfGuide.Test.Sorting
{
    [TestClass]
    public class NameDescendingSorterTests
    {
        [TestMethod]
        public void CorrectMode()
        {
            var sorter = new NameDescendingSorter();
            Assert.AreEqual(EAudioDramaSortMode.NameDescending, sorter.SortMode);
        }

        [TestMethod]
        public void SortByNameDescending()
        {
            var provider = new MultipleAudioDramaProvider();
            var sorter = new NameDescendingSorter();
            var audioDramas = provider.Get().ToList();
            var expectedSort = audioDramas.OrderByDescending(x => x.AudioDramaDto.Name).ToList();

            var sort = sorter.Sort(audioDramas).ToList();

            CollectionAssert.AreEqual(expectedSort, sort);
        }
    }
}