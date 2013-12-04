﻿using VKSharp.Core.EntityParsers;
using VKSharp.Core.Interfaces;

namespace VKSharp.Core.Entities {
    public class EntityList<T>:IVKEntity<EntityList<T>> where T:IVKEntity<T>, new() {
                public uint TotalCount { get; set; }
        public T[] Items { get; set; }

        public VKApi Context { get; set; }

        public IXmlVKEntityParser<EntityList<T>> GetParser() {
            return EntityListParser<T>.Instanse;
        }
    }
}
