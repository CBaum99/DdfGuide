﻿using System.Collections.Generic;
using System.Linq;
using DdfGuide.Core;
using DdfGuide.Core.Searching;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DdfGuide.Test.Searching
{
    [TestClass]
    public class AudioDramaSearcherTests
    {
        private List<AudioDrama> _audioDramas;
        private AudioDramaSearcher _searcher;

        [TestInitialize]
        public void Init()
        {
            var provider = new MultipleAudioDramaProvider();
            _audioDramas = provider.Get().ToList();
            _searcher = new AudioDramaSearcher();
        }

        [TestMethod]
        public void WhenNoSearchTextIsGivenTheSearcherFindsAllAudioDramas()
        {
            var searchResult = _searcher.Search(_audioDramas, "").ToList();
            CollectionAssert.AreEqual(_audioDramas, searchResult);

            searchResult = _searcher.Search(_audioDramas, string.Empty).ToList();
            CollectionAssert.AreEqual(_audioDramas, searchResult);

            searchResult = _searcher.Search(_audioDramas, "  ").ToList();
            CollectionAssert.AreEqual(_audioDramas, searchResult);

            searchResult = _searcher.Search(_audioDramas, "   ").ToList();
            CollectionAssert.AreEqual(_audioDramas, searchResult);

            searchResult = _searcher.Search(_audioDramas, null).ToList();
            CollectionAssert.AreEqual(_audioDramas, searchResult);
        }

        [TestMethod]
        public void SearchTextWithPartOfIdReturnsSingleCorrectAudioDrama()
        {
            var audioDrama = _audioDramas.ElementAt(2);
            var searchText = audioDrama.AudioDramaDto.Id.ToString().Substring(4, 5);

            var searchResult = _searcher.Search(_audioDramas, searchText).ToList();

            Assert.AreEqual(1, searchResult.Count);
            Assert.AreEqual(audioDrama, searchResult.First());
        }

        [TestMethod]
        public void SearchForPartOfNameReturnsAllAudioDramasThatContainThatPart()
        {
            const string searchText = "Sample name";
            var expectedSearchResult = _audioDramas.Where(x => x.AudioDramaDto.Name.Contains("Sample name")).ToList();

            var searchResult = _searcher.Search(_audioDramas, searchText).ToList();

            Assert.AreEqual(3, searchResult.Count);
            CollectionAssert.AreEqual(expectedSearchResult, searchResult);
        }

        [TestMethod]
        public void SearchForPartOfNameWithMixedCasing_SearchShouldStillReturnSameResult()
        {
            const string searchText = "sAMPlE nAMe";
            var expectedSearchResult = _audioDramas.Where(x => x.AudioDramaDto.Name.Contains("Sample name")).ToList();

            var searchResult = _searcher.Search(_audioDramas, searchText).ToList();

            Assert.AreEqual(3, searchResult.Count);
            CollectionAssert.AreEqual(expectedSearchResult, searchResult);
        }
    }
}