﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace DdfGuide.Core
{
    public class OnUserDataChangedInCacheSaver
    {
        private readonly ICache<IEnumerable<AudioDramaUserData>> _cache;
        private readonly ISource<IEnumerable<AudioDrama>> _source;
        private IEnumerable<AudioDramaUserData> _userDatas;

        public OnUserDataChangedInCacheSaver(
            ICache<IEnumerable<AudioDramaUserData>> cache,
            ISource<IEnumerable<AudioDrama>> source)
        {
            _cache = cache;
            _source = source;

            _source.Updated += (sender, args) => SetObservedUserDatas(args.Select(x => x.AudioDramaUserData));
        }

        public void SetObservedUserDatas(IEnumerable<AudioDramaUserData> userDatas)
        {
            _userDatas = userDatas;

            foreach (var userData in _userDatas)
            {
                userData.Changed -= OnUserDataChanged();
                userData.Changed += OnUserDataChanged();
            }
        }

        private EventHandler OnUserDataChanged()
        {
            return (sender, args) => { _cache.Save(_userDatas); };
        }
    }
}